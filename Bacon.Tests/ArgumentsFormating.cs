using System.Numerics;
using Bacon.Build;

namespace Bacon.Tests;

public class ArgumentsFormating
{
    [Test]
    public async Task Required()
    {
        var arguments = new TestRequiredArguments.Builder().SetRequired("RequiredValue").Build();

        await Assert.That(arguments.FormatToString()).IsEqualTo("req RequiredValue");
    }

    [Test]
    [Arguments("\"\\?", "esc \"\\\"\\?\"")]
    [Arguments("toto\\tutu", "esc toto\\tutu")]
    [Arguments("toto tutu", "esc \"toto tutu\"")]
    [Arguments("toto\"tutu", "esc \"toto\\\"tutu\"")]
    [Arguments("toto\\\"tutu", "esc \"toto\\\\\\\"tutu\"")]
    [Arguments("toto\\\\\"tutu", "esc \"toto\\\\\\\\\\\"tutu\"")]
    [Arguments("to\\to\\\\\"tutu", "esc \"to\\to\\\\\\\\\\\"tutu\"")]
    [Arguments("toto tutu\\", "esc \"toto tutu\\\\\"")]
    [Arguments("toto tutu\\\\", "esc \"toto tutu\\\\\\\\\"")]
    [Arguments("to\\to\\\\\"tutu\\\\", "esc \"to\\to\\\\\\\\\\\"tutu\\\\\\\\\"")]
    public async Task Escaped(string value, string expected)
    {
        var arguments = new TestEscapedArguments.Builder().SetEscaped(value).Build();

        await Assert.That(arguments.FormatToString()).IsEqualTo(expected);
    }

    [Test]
    public async Task AllTypesMax()
    {
        var arguments = new TestAllTypesArguments.Builder()
            .SetString("str")
            .SetByte(byte.MaxValue)
            .SetSByte(sbyte.MaxValue)
            .SetUShort(ushort.MaxValue)
            .SetShort(short.MaxValue)
            .SetUInt(uint.MaxValue)
            .SetInt(int.MaxValue)
            .SetULong(ulong.MaxValue)
            .SetLong(long.MaxValue)
            .SetUInt128(UInt128.MaxValue)
            .SetInt128(Int128.MaxValue)
            .SetHalf(Half.MaxValue)
            .SetFloat(float.MaxValue)
            .SetDouble(double.MaxValue)
            .SetDecimal(decimal.MaxValue)
            .SetBigInteger((BigInteger)UInt128.MaxValue + 1)
            .SetChar(char.MaxValue)
            .EnableBool()
            .SetEnum(TestEnum.Value3)
            .SetFormattable(new TestFormattable("woot"))
            .Build();

        await Assert.That(arguments.FormatToString()).IsEqualTo("all --string str --byte 255 --sbyte 127 --ushort 65535 --short 32767 --uint 4294967295 --int 2147483647 --ulong 18446744073709551615 --long 9223372036854775807 --uint128 340282366920938463463374607431768211455 --int128 170141183460469231731687303715884105727 --half 65500 --float \"3.4028235E+38\" --double \"1.7976931348623157E+308\" --decimal 79228162514264337593543950335 --bigInteger 340282366920938463463374607431768211456 --char \"\uffff\" --bool --enum value3 --formattable \"(woot:{null})\"");
    }

    [Test]
    public async Task AllTypesMin()
    {
        // A command expecting negative will either parse it correctly or expect --option=-42 to avoid confusion. Here we format without the = as this is not the goal of the test.
        var arguments = new TestAllTypesArguments.Builder()
            .SetString("")
            .SetByte(byte.MinValue)
            .SetSByte(sbyte.MinValue)
            .SetUShort(ushort.MinValue)
            .SetShort(short.MinValue)
            .SetUInt(uint.MinValue)
            .SetInt(int.MinValue)
            .SetULong(ulong.MinValue)
            .SetLong(long.MinValue)
            .SetUInt128(UInt128.MinValue)
            .SetInt128(Int128.MinValue)
            .SetHalf(Half.MinValue)
            .SetFloat(float.MinValue)
            .SetDouble(double.MinValue)
            .SetDecimal(decimal.MinValue)
            .SetBigInteger((BigInteger)Int128.MinValue - 1)
            .SetChar(char.MinValue)
            .SetEnum(TestEnum.Value0)
            .SetFormattable(new TestFormattable(""))
            .Build();

        await Assert.That(arguments.FormatToString()).IsEqualTo("all --string \"\" --byte 0 --sbyte -128 --ushort 0 --short -32768 --uint 0 --int -2147483648 --ulong 0 --long -9223372036854775808 --uint128 0 --int128 -170141183460469231731687303715884105728 --half -65500 --float \"-3.4028235E+38\" --double \"-1.7976931348623157E+308\" --decimal -79228162514264337593543950335 --bigInteger -170141183460469231731687303715884105729 --char \"\u0000\" --enum value0 --formattable \"(:{null})\"");
    }

    [Test]
    public async Task AllTypesOptionalMissing()
    {
        var arguments = new TestAllTypesOptionalArguments.Builder().Build();

        await Assert.That(arguments.FormatToString()).IsEqualTo("opt");
    }

    [Test]
    public async Task AllTypesOptionalPresent()
    {
        var arguments = new TestAllTypesOptionalArguments.Builder()
            .SetString("abc")
            .SetByte(1)
            .SetSByte(2)
            .SetUShort(3)
            .SetShort(4)
            .SetUInt(5)
            .SetInt(6)
            .SetULong(7)
            .SetLong(8)
            .SetUInt128(9)
            .SetInt128(10)
            .SetHalf((Half)11)
            .SetFloat(12)
            .SetDouble(13)
            .SetDecimal(14)
            .SetBigInteger(15)
            .SetChar(' ')
            .SetEnum(TestEnum.Value1)
            .SetFormattable(new TestFormattable("zyx"))
            .Build();

        await Assert.That(arguments.FormatToString()).IsEqualTo("opt --string abc --byte 1 --sbyte 2 --ushort 3 --short 4 --uint 5 --int 6 --ulong 7 --long 8 --uint128 9 --int128 10 --half 11 --float 12 --double 13 --decimal 14 --bigInteger 15 --char \" \" --enum value1 --formattable \"(zyx:{null})\"");
    }

    [Test]
    public async Task Booleans()
    {
        var arguments = new TestBooleanArguments.Builder()
            .EnableOptional0()
            .SetNullableFalse(false)
            .SetNullableTrue(true)
            .Build();

        await Assert.That(arguments.FormatToString()).IsEqualTo("bool --optional0 --nullable-false False --nullable-true True");
    }

    [Test]
    public async Task Collections()
    {
        var arguments = new TestCollectionArguments.Builder()
            .AddListStrings("str0", "st+r1")
            .AddListNumerics(0, 1)
            .AddListEnums(TestEnum.Value1, TestEnum.Value3)
            .AddDicStrings("k0", "v0")
            .AddDicNumerics("k1", 2)
            .AddDicEnums("k2", TestEnum.Value2)
            .Build();

        await Assert.That(arguments.FormatToString()).IsEqualTo("col --list-strings str0 --list-strings \"st+r1\" --list-numerics 0 --list-numerics 1 --list-enums value1 --list-enums value3 --dic-strings k0=v0 --dic-numerics k1=2 --dic-enums k2=value2");
    }

    [Test]
    public async Task ValueColumnDash()
    {
        var arguments = new TestValueColumnDashArguments.Builder()
            .SetString("str0")
            .SetNumeric(0)
            .SetEnum(TestEnum.Value0)
            .AddListStrings("str1", "st+r2")
            .AddListNumerics(1, 2)
            .AddListEnums(TestEnum.Value1, TestEnum.Value2)
            .AddDicStrings("k0", "v0")
            .AddDicNumerics("k1", 2)
            .AddDicEnums("k2", TestEnum.Value2)
            .Build();

        await Assert.That(arguments.FormatToString()).IsEqualTo("vcd -string:\"str0\" -numeric:\"0\" -enum:\"value0\" -list-strings:\"str1\" -list-strings:\"st+r2\" -list-numerics:\"1\" -list-numerics:\"2\" -list-enums:\"value1\" -list-enums:\"value2\" -dic-strings:\"k0=v0\" -dic-numerics:\"k1=2\" -dic-enums:\"k2=value2\"");
    }

    [Test]
    public async Task WholeEqualSlash()
    {
        var arguments = new TestWholeEqualSlashArguments.Builder()
            .SetString("str0")
            .SetNumeric(0)
            .SetEnum(TestEnum.Value0)
            .AddListStrings("str1", "st+r2")
            .AddListNumerics(1, 2)
            .AddListEnums(TestEnum.Value1, TestEnum.Value2)
            .AddDicStrings("k0", "v0")
            .AddDicNumerics("k1", 2)
            .AddDicEnums("k2", TestEnum.Value2)
            .Build();

        await Assert.That(arguments.FormatToString()).IsEqualTo("wes \"/string=str0\" \"/numeric=0\" \"/enum=value0\" \"/list-strings=str1\" \"/list-strings=st+r2\" \"/list-numerics=1\" \"/list-numerics=2\" \"/list-enums=value1\" \"/list-enums=value2\" \"/dic-strings=k0=v0\" \"/dic-numerics=k1=2\" \"/dic-enums=k2=value2\"");
    }

    [Test]
    public async Task Join()
    {
        var arguments = new TestJoinArguments.Builder()
            .AddListStrings("str0", "st+r1")
            .AddListNumerics(0, 1)
            .AddListEnums(TestEnum.Value0, TestEnum.Value1)
            .AddDicStrings("k0", "v0")
            .AddDicStrings("k1", "v1")
            .AddDicNumerics("k2", 2)
            .AddDicNumerics("k3", 3)
            .AddDicEnums("k4", TestEnum.Value2)
            .AddDicEnums("k5", TestEnum.Value3)
            .Build();

        //TODO: How are quotes expected to work with join and join string
        await Assert.That(arguments.FormatToString()).IsEqualTo("join --list-strings str0;\"st+r1\" --list-numerics 0+1 --list-enums value0<->value1 --dic-strings k0=v0@k1=v1 --dic-numerics k2=2;k3=3 --dic-enums k4=value2 - k5=value3");
    }

    [Test]
    public async Task Secret()
    {
        var arguments = new TestSecretArguments.Builder()
            .AddSecret0("abc&123", "def")
            .SetParam0("pa0")
            .SetSecret1(456)
            .SetParam1("pa1")
            .SetSecret2(false)
            .SetParam2("pa2")
            .Build();

        var handler = new ArgumentsStringHandler();
        arguments.AppendToStringHandler(ref handler);
        //TODO: Should redacted only be put once for collections ...?
        await Assert.That(handler.GetRedactedString()).IsEqualTo("secret --secret0 [redacted] --secret0 [redacted] --param0 pa0 --secret1 [redacted] --param1 pa1 --secret2 [redacted] --param2 pa2");
        await Assert.That(handler.GetSecretString()).IsEqualTo("secret --secret0 \"abc&123\" --secret0 def --param0 pa0 --secret1 456 --param1 pa1 --secret2 False --param2 pa2");
    }

    [Test]
    public async Task SecretQuotes()
    {
        var arguments = new TestSecretQuotesArguments.Builder()
            .AddSecret0("abc&123", "def")
            .SetParam0("pa0")
            .SetSecret1(456)
            .SetParam1("pa1")
            .SetSecret2(false)
            .SetParam2("pa2")
            .Build();

        var handler = new ArgumentsStringHandler();
        arguments.AppendToStringHandler(ref handler);

        await Assert.That(handler.GetRedactedString()).IsEqualTo("secret --secret0=\"[redacted]\" --secret0=\"[redacted]\" --param0=\"pa0\" --secret1=\"[redacted]\" --param1=\"pa1\" --secret2=\"[redacted]\" --param2=\"pa2\"");
        await Assert.That(handler.GetSecretString()).IsEqualTo("secret --secret0=\"abc&123\" --secret0=\"def\" --param0=\"pa0\" --secret1=\"456\" --param1=\"pa1\" --secret2=\"False\" --param2=\"pa2\"");
    }
}