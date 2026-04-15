namespace Velochat.Backend.App.Layers.Models;

public class ModelNotInsertableException() 
    : VelochatException(
        "Model is not ready for insertion. Make sure all required fields are filled,"
        + " and fields like auto-generated primary keys are absent in the model."
    );