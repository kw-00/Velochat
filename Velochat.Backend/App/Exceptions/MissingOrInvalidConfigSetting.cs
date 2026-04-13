namespace Velochat.Backend.App.Exceptions;

public class MissingOrInvalidConfigSetting(string key) : Exception($"Missing or invalid setting: {key}");