using Velochat.Backend.App.Infrastructure.DTOs;
using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.API.Auth;

public class SessionInitData
{
    public required EncodedTokenPair TokenPair;
    public required CompleteUser User;
}