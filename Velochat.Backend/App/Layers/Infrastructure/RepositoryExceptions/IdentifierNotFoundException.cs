using System.Text;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Infrastructure;


public class IdentifierNotFoundException<T> 
    : RepositoryException
    where T : IMalleableModel
{
    public string Role { get; }

    private IdentifierNotFoundException(string message, string? role = null) 
        : base(message)
    {
        Role = role ?? typeof(T).Name;
    }
        

    public IdentifierNotFoundException(T model, string? role = null)
        : this(GetMessage(model, role)) { }


    public IdentifierNotFoundException(string? role, params object[] primaryKeyValues) 
        : this(GetMessage(role, primaryKeyValues)) { }
    

    private static string GetMessage(string? role, params object[] primaryKeyValues)
    {
        if (primaryKeyValues.Length == 0)
        {
            throw new ArgumentException("At least one primary key value must be provided.");
        }

        var messageBuilder = new StringBuilder(
            $"{Capitalized(role) ?? typeof(T).Name} does not exist. {typeof(T).Name} with ID of "
        ).Append(primaryKeyValues.Length > 1 ? '(' : "");

        return messageBuilder.Append(
            string.Join(
                ", ", 
                primaryKeyValues
                .Select(p =>
                {
                    var formattedValue = p switch
                    {
                        string s => $"\"{s}\"",
                        _ => p.ToString()
                    };
                    return $"{formattedValue}";
                })
            )
        )
        .Append(primaryKeyValues.Length > 1 ? ')' : "")
        .Append("does not exist.")
        .ToString();
    }

    private static string GetMessage(T model, string? role)
    {
        var primaryKeyProperties = model.GetPrimaryKeyProperties();
        var messageBuilder = new StringBuilder(
            $"{Capitalized(role) ?? typeof(T).Name} does not exist. {typeof(T).Name} with "
        );

        messageBuilder.Append(
            string.Join(
                ", ", 
                primaryKeyProperties
                .ToList()
                .Skip(2)
                .Select(p =>
                {
                    var formattedValue = p.Value switch
                    {
                        string s => $"\"{s}\"",
                        _ => p.Value.ToString()
                    };
                    return $"{p.Key} of {formattedValue}";
                })
            )
        ).Append(' ');

        messageBuilder.Append(
            string.Join(
                " and ", 
                primaryKeyProperties
                    .Take(2)
                    .Select(p => $"{p.Key} of {p.Value}")
            )
        );

        return messageBuilder
            .Append($" does not exist.")
            .ToString();
    }

    private static string? Capitalized(string? s)
    {
        if (s is null || s.Length == 0) return s;
        return char.ToUpper(s[0]) + s[1..];
    }
}