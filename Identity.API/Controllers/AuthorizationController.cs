using Identity.API.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Identity.API.Controllers;

[ApiController]
public class AuthorizationController : Controller
{
    private readonly IAuthService _authService;

    public AuthorizationController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("~/connect/token")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();

        if (request == null)
            return BadRequest();

        if (!request.IsPasswordGrantType())
            return BadRequest("Only Password Grant is supported.");

        var principal = await _authService.ValidateUserAsync(
            request.Username!,
            request.Password!);

        if (principal == null)
        {
            return Forbid(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        return SignIn(
            principal,
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}