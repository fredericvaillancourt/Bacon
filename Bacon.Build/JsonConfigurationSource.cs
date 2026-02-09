using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;

namespace Bacon.Build;

// This is so complicated because System.Text.Json.JsonSerializer does not support deserializing in an existing object.
// https://github.com/dotnet/runtime/issues/29538
public sealed class JsonConfigurationSource<T>(string filename, JsonSerializerOptions? options = null) : IConfigurationSource<T>
    where T : class
{
    private static Type? _jsonSerializerType;

    public async Task ApplyAsync(T context, IReadOnlyList<InputInfo> inputsInfo, BuildConfiguration buildConfiguration)
    {
        _jsonSerializerType ??= GenerateType(inputsInfo);

        await using var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
        if (await JsonSerializer.DeserializeAsync(stream, _jsonSerializerType, options) is not IApply<T> deserialized)
        {
            throw new InvalidOperationException($"Could not deserialize {filename}");
        }

        deserialized.Apply(context);
    }

    private static Type GenerateType(IReadOnlyList<InputInfo> inputsInfo)
    {
        string assemblyNameString = $"Assembly{typeof(T).Name}";
        var assemblyName = new AssemblyName(assemblyNameString);
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyNameString);
        var typeBuilder = moduleBuilder.DefineType("TypeForJsonSerializer", TypeAttributes.Public);

        var methodBuilder = typeBuilder.DefineMethod(
            "Apply",
            MethodAttributes.Private | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask,
            CallingConventions.Standard | CallingConventions.HasThis,
            typeof(void),
            [typeof(T)]);

        typeBuilder.AddInterfaceImplementation(typeof(IApply<T>));
        typeBuilder.DefineMethodOverride(methodBuilder, typeof(IApply<T>).GetMethod(nameof(IApply<>.Apply), BindingFlags.Public | BindingFlags.Instance) ?? throw new InvalidOperationException("Missing apply method"));

        var methodGenerator = methodBuilder.GetILGenerator();

        foreach (InputInfo inputInfo in inputsInfo)
        {
            var propertyType = AsNullable(inputInfo.Property.PropertyType);

            var fieldBuilder = typeBuilder.DefineField(inputInfo.Name, propertyType, FieldAttributes.Private);

            var getBuilder = typeBuilder.DefineMethod(
                $"get_{inputInfo.Name}",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                propertyType,
                Type.EmptyTypes);

            var getIlGenerator = getBuilder.GetILGenerator();
            getIlGenerator.Emit(OpCodes.Ldarg_0);
            getIlGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            getIlGenerator.Emit(OpCodes.Ret);

            var setBuilder = typeBuilder.DefineMethod(
                $"set_{inputInfo.Name}",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null,
                [propertyType]);

            var setIlGenerator = setBuilder.GetILGenerator();
            setIlGenerator.Emit(OpCodes.Ldarg_0);
            setIlGenerator.Emit(OpCodes.Ldarg_1);
            setIlGenerator.Emit(OpCodes.Stfld, fieldBuilder);
            setIlGenerator.Emit(OpCodes.Ret);

            var propertyBuilder = typeBuilder.DefineProperty(inputInfo.Name, PropertyAttributes.HasDefault, propertyType, Type.EmptyTypes);
            propertyBuilder.SetGetMethod(getBuilder);
            propertyBuilder.SetSetMethod(setBuilder);

            if (propertyType.IsValueType)
            {
                methodGenerator.Emit(OpCodes.Ldarg_0); // this
                methodGenerator.Emit(OpCodes.Ldflda, fieldBuilder); // field
                methodGenerator.Emit(OpCodes.Call, propertyType.GetProperty(nameof(Nullable<>.HasValue), BindingFlags.Instance | BindingFlags.Public)!.GetMethod ?? throw new InvalidOperationException("Missing HasValue.get"));
            }
            else
            {
                methodGenerator.Emit(OpCodes.Ldarg_0); // this
                methodGenerator.Emit(OpCodes.Ldfld, fieldBuilder); // field
                methodGenerator.Emit(OpCodes.Ldnull);
                methodGenerator.Emit(OpCodes.Cgt_Un); // !=
            }

            var label = methodGenerator.DefineLabel();
            methodGenerator.Emit(OpCodes.Brfalse_S, label);

            methodGenerator.Emit(OpCodes.Ldarg_1); // context
            methodGenerator.Emit(OpCodes.Ldarg_0); // this

            if (inputInfo.Property.PropertyType.IsValueType &&
                Nullable.GetUnderlyingType(inputInfo.Property.PropertyType) == null)
            {
                methodGenerator.Emit(OpCodes.Ldflda, fieldBuilder);
                methodGenerator.Emit(OpCodes.Call, propertyType.GetProperty(nameof(Nullable<>.Value), BindingFlags.Instance | BindingFlags.Public)!.GetMethod ?? throw new InvalidOperationException("Missing Value.get"));
            }
            else
            {
                methodGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            }

            methodGenerator.Emit(OpCodes.Callvirt, inputInfo.Property.SetMethod ?? throw new InvalidOperationException("Missing set method")); //TODO: Should we just skip those?
            methodGenerator.MarkLabel(label);
        }

        methodGenerator.Emit(OpCodes.Ret);

        return typeBuilder.CreateType();
    }

    private static Type AsNullable(Type type)
    {
        if (!type.IsValueType)
        {
            return type;
        }

        if (Nullable.GetUnderlyingType(type) != null)
        {
            return type;
        }

        return typeof(Nullable<>).MakeGenericType(type);
    }
}