using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.Infrastructure.Models;

public class ModelNotIdentifiableException() 
    : VelochatException(
        "Model is not identifiable, as its primary key is null or incomplete."
    );