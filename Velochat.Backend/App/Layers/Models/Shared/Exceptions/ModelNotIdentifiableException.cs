namespace Velochat.Backend.App.Layers.Models;

public class ModelNotIdentifiableException() 
    : VelochatException(
        "Model is not identifiable, as its primary key is null or incomplete."
    );