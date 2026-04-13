namespace Velochat.Backend.App.Layers.Models;

public class ModelNotInsertableException() 
    : Exception("Model is not insertable. Check whether it does not have non-null values in fields such as ID.");