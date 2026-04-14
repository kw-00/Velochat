
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Velochat.Backend.App.Layers.Models;

/// <summary>
/// Indicates a primary key for classes implementing 
/// <see cref="IModel"/> or <see cref="IModel"/> 
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class PrimaryKeyAttribute : Attribute;

public static class PrimaryKeyAttributeExtensions
{
    public static IReadOnlyDictionary<string, object?> GetPrimaryKeyProperties(this IModel model)
    {
        return model.GetPrimaryKeyPropertiesForObject();
    }

    private static IReadOnlyDictionary<string, object?> GetPrimaryKeyPropertiesForObject(this object obj)
    {
        var pkProperties = obj.GetType()
            .GetProperties()
            .Where(p => Attribute.IsDefined(p, typeof(PrimaryKeyAttribute)));

        Dictionary<string, object?> result = new();
        foreach (var prop in pkProperties)
        {
            result.Add(prop.Name, prop.GetValue(obj));
        }

        return result;
    }
}