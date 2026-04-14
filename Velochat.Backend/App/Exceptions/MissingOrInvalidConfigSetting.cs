namespace Velochat.Backend.App.Exceptions.StatusExceptions;

public class MissingOrInvalidConfigSetting(string key) : Exception($"Missing or invalid setting: {key}");