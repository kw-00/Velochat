using Velochat.Backend.App.Infrastructure.DTOs;
using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.API.Domains.Identity;

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
