namespace Velochat.Backend.App.Layers.Models;

public class ModelNotCompleteException() 
    : Exception(
        "Model cannot be conveted to its complete form"
        + " due to null or invalid values in certain fields."
    );