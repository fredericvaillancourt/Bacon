using System.Reflection;

namespace Bacon.Build;

public sealed record InputInfo(PropertyInfo Property, InputAttribute Attribute, string Name)
{
    public void ApplyValue(object context, object? value)
    {
        Property.SetValue(context, Convert.ChangeType(value, Property.PropertyType));
    }
}