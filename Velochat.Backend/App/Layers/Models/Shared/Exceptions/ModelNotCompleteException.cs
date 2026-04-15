using Velochat.Backend.App.Shared.Exceptions;

namespace Velochat.Backend.App.Layers.Models;

public class ModelNotCompleteException() 
    : VelochatException(
        "Model cannot be conveted to its complete form"
        + " due to null or invalid values in certain fields."
    );