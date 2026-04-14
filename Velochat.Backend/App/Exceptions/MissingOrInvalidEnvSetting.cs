namespace Velochat.Backend.App.Exceptions.StatusExceptions;

public class MissingOrInvalidEnvSetting(string envVarName) : Exception($"Missing or invalid environment variable: {envVarName}");