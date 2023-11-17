using Application.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Service = Application.Interface.Services.IUsuariosService;
using ServiceCrudForms = Application.Interface.Services.IUsuariosCrudFormsService;
using CrudService = Application.Interface.Services.ICrudFormsInstaladorService;
using LoggerService = Application.Interface.Services.ILoggerService;

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
        private readonly ServiceCrudForms _serviceCrudForms;
        private readonly CrudService _crudService;
        private readonly LoggerService _loggerService;

        public TokenController(ILogger<TokenController> logger, Service service, IConfiguration builder, CrudService crudService, LoggerService loggerService, ServiceCrudForms serviceCrudForms)
        {
            _logger = logger;
            _service = service;
            _builder = builder;
            _crudService = crudService;
            _loggerService = loggerService;
            _serviceCrudForms = serviceCrudForms;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IResult> TokenAsync(User user) 
        {
            var userModel = await _service.GetByLogin(user.UserName, user.Password);
            
            if(userModel == null)
            {
                return Results.Unauthorized();
            }

            if(userModel.IsVerified != "1")
            {
                return Results.StatusCode(300);
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
                Expires = DateTime.UtcNow.AddDays(30),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return Results.Ok(new { token = stringToken, nome = userModel.Nome, username = userModel.Email, admin = userModel.Admin, Id = userModel.Id});
        }

        [AllowAnonymous]
        [HttpPost("crudforms")]
        public async Task<IResult> TokenCrudFormsAsync(User user)
        {
            var userModel = await _serviceCrudForms.GetByLogin(user.UserName, user.Password);

            if (userModel == null)
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
                    new Claim(JwtRegisteredClaimNames.Sub, userModel.Login),
                    new Claim(JwtRegisteredClaimNames.Email, userModel.Email),
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

            var crudVersao = await _crudService.GetLastVerion();

            return Results.Ok(new { token = stringToken, nome = userModel.Nome, username = userModel.Email, admin = userModel.Administrador, Id = userModel.Codigo, crudVersao = crudVersao });
        }
    }
}
