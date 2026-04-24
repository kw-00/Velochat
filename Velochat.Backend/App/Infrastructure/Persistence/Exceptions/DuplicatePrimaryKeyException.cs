using System.Text;
using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.Infrastructure.Persistence;

public class DuplicatePrimaryKeyException<T>(T model)
    : RepositoryException(GetMessage(model))
    where T : IModel
{
    private static string GetMessage(T model)
    {
        var primaryKeyProperties = model.GetPrimaryKeyProperties();
        var messageBuilder = new StringBuilder(
            $"Duplicate primary key. {typeof(T).Name} with "
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
            .Append(" already exists in the database.")
            .ToString();
    }
}