using Velochat.Backend.App.Layers.DTOs;
using Velochat.Backend.App.Layers.Models;

namespace Velochat.Backend.App.Layers.Domains.User;

public interface IUserOrchestration
{
    Task<(
        CompleteUser User, 
        EncodedTokenPair EncodedTokenPair
    )> RegisterAsync(Credentials credentials);

    Task<(
        CompleteUser User, 
        EncodedTokenPair EncodedTokenPair
    )> LogInAsync(Credentials credentials);

    Task<EncodedTokenPair> RefreshTokenAsync(string refreshTokenString);

    Task LogOutAsync(string refreshTokenString);

}
