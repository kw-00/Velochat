using System.Text.Json;
using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.Shared.TypeHelpers;

public static class ObjectExtensions {

    private static readonly JsonSerializerOptions _options = new();

    static ObjectExtensions()
    {
        _options.PropertyNameCaseInsensitive = true;
    }
    
    public static T MapTo<T>(this object input)
    {
        try
        {
            var json = JsonSerializer.Serialize(input, _options);
            var result = JsonSerializer.Deserialize<T>(json, _options);

            var targetType = typeof(T);

            // If result is null, check if T allows null
            if (result is null && !AllowsNull(targetType))
            {
                throw new ObjectMappingException(
                    $"Cannot convert {input.GetType().Name} to {typeof(T).Name}");
            }

            return result!;
        }
        catch (Exception ex) when (ex is not ObjectMappingException)
        {
            throw new ObjectMappingException(
                $"Cannot convert {input.GetType().Name} to {typeof(T).Name}");
        }
    }

    private static bool AllowsNull(Type type)
    {
        if (!type.IsValueType)
            return true;

        return Nullable.GetUnderlyingType(type) != null;
    }
}

public class ObjectMappingException(string message) : VelochatException(message);
