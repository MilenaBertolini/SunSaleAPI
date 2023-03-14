using Application.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Service = Application.Interface.Services.IUsuariosService;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly IConfiguration _builder;
        private readonly Service _service;

        public TokenController(ILogger<TokenController> logger, Service service, IConfiguration builder)
        {
            _logger = logger;
            _service = service;
            _builder = builder;
        }

        [AllowAnonymous]
        [HttpPost]
        public IResult Token(User user) 
        {
            var userModel = _service.GetByLogin(user.UserName, user.Password);
            
            if(userModel == null)
            {
                return Results.Unauthorized();
            }
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
            return Results.Ok(new { token = stringToken, username = userModel.Result.Email});
        }
    }
}
