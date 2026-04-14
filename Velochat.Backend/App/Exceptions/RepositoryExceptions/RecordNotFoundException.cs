using System.Text;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Exceptions.RepositoryExceptions;


public class RecordNotFoundException<T>(T model) 
    : RepositoryException(GetMessage(model))
    where T : IMalleableModel
{
    private static string GetMessage(T model)
    {
        var primaryKeyProperties = model.GetPrimaryKeyProperties();
        var messageBuilder = new StringBuilder("Duplicate primary key. Model with ");

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
        ).Append(" ");

        messageBuilder.Append(
            string.Join(
                " and ", 
                primaryKeyProperties
                    .Take(2)
                    .Select(p => $"{p.Key} of {p.Value}")
            )
        );

        return messageBuilder.ToString();
    }
}