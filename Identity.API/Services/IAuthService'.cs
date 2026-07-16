using System.Security.Claims;

namespace Identity.API.Services;

public interface IAuthService
{
    Task<ClaimsPrincipal?> ValidateUserAsync(string email, string password);
}