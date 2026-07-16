using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Product.Application.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginRequest = Product.Application.DTOs.LoginRequest;

namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        if (request.Email == "admin@test.com" &&
           request.Password == "123456")
        {
            var token = GenerateToken(request.Email);

            return Ok(new LoginResponse
            {
                Token = token,
                Expiration = DateTime.Now.AddMinutes(60)
            });
        }

        return Unauthorized("Invalid username or password");
    }


    private string GenerateToken(string email)
    {
        var claims = new[]
        {
            new Claim(
                JwtRegisteredClaimNames.Email,
                email
            ),

            new Claim(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()
            )
        };


        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!
            ));


        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );


        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials
        );


        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}