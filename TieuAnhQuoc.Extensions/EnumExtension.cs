using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TieuAnhQuoc.Extensions;

public static class EnumHelper
{
    public static string GetDisplayName(this Enum enu)
    {
        if (enu == null)
            return null;
        var attr = GetDisplayAttribute(enu);
        return attr != null ? attr.Name : enu.ToString();
    }

    public static string GetDescription(this Enum enu)
    {
        var attr = GetDisplayAttribute(enu);
        return attr != null ? attr.Description : enu.ToString();
    }

    public static List<int> GetIntValues<T>()
    {
        var results = new List<int>();
        foreach (var itemType in Enum.GetValues(typeof(T))) results.Add((int) itemType);
        return results;
    }

    private static DisplayAttribute GetDisplayAttribute(object value)
    {
        if (value == null)
            return null;
        var type = value.GetType();
        if (!type.IsEnum) throw new ArgumentException($"Type {type} is not an enum");

        // Get the enum field.
        var field = type.GetField(value.ToString() ?? string.Empty);
        return field == null ? null : field.GetCustomAttribute<DisplayAttribute>();
    }
}