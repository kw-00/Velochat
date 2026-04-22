namespace Velochat.Backend.App.Shared.RealtimeCommunication;

public class ChannelCategory(string name)
{
    public string Name { 
        get
        {
            if (name.Contains(':')) 
                throw new ArgumentException("Name cannot contain \":\"");
            return name;
        }
    }

    public string GetChannel(object localChannelId) => $"{Name}:{localChannelId}";

    public string GetLocalChannelId(string channel)
    {
        if (!channel.StartsWith($"{Name}:"))
            throw new ArgumentException(
                "Full channel name does not start with the expected category name."
                + $" Expected: \"{Name}\"."
            );
        return channel[(Name.Length + 1)..];
    }
}