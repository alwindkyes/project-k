using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace project_k.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    IConfiguration _configuration;
    public LoginController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (!String.IsNullOrEmpty(request.username) && !String.IsNullOrEmpty(request.password))
        {
            var response = new Token();
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, request.username),
                new Claim(ClaimTypes.Role , "Admin")
            };
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: signingCredentials
            );
            response.token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.statusCode = 200;
            response.statusMessage = "Login Success";
            return Ok(response);
        }
        else
        {
            var response = new Status();
            response.statusCode = 401;
            response.statusMessage = "Invalid credentials";
            return Unauthorized(response);
        }
    }
}