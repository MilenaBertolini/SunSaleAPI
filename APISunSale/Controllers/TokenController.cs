using Application.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly IConfiguration _builder;

        public TokenController(ILogger<TokenController> logger, IConfiguration builder)
        {
            _logger = logger;
            _builder = builder;
        }

        [AllowAnonymous]
        [HttpPost]
        public IResult Token(User user) 
        {
            string username = _builder.GetValue<string>("Jwt:UserName");
            string pass = _builder.GetValue<string>("Jwt:Password");

            if (user.UserName.Equals(username) && user.Password.Equals(pass))
            {
                var issuer = _builder.GetValue<string>("Jwt:Issuer");
                var audience = _builder.GetValue<string>("Jwt:Audience");
                var key = Encoding.ASCII.GetBytes(_builder.GetValue<string>("Jwt:Key"));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var stringToken = tokenHandler.WriteToken(token);
                return Results.Ok(new { token = stringToken });
            }
            return Results.Unauthorized();
        }
    }
}
