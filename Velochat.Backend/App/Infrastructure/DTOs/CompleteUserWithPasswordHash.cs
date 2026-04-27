using Velochat.Backend.App.Infrastructure.Models;

namespace Velochat.Backend.App.Infrastructure.DTOs
{
    public class CompleteUserWithPasswordHash : CompleteUser
    {
        public required string PasswordHash { get; set; }
    }
}