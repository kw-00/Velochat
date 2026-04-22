using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.Infrastructure.Models;

public class ModelNotInsertableException() 
    : VelochatException(
        "Model is not ready for insertion. Make sure all required fields are filled,"
        + " and fields like auto-generated primary keys are absent in the model."
    );