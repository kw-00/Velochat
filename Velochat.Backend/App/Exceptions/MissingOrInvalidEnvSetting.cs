namespace Velochat.Backend.App.Exceptions;

public class MissingOrInvalidEnvSetting(string envVarName) : Exception($"Missing or invalid environment variable: {envVarName}");