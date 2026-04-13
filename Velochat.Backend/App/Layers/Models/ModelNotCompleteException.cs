namespace Velochat.Backend.App.Layers.Models;

public class ModelNotCompleteException() 
    : Exception("Model is not complete. Check whether all required fields are filled.");