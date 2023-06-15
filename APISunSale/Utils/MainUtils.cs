using Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using UserService = Application.Interface.Services.IUsuariosService;
using UserCrudFormsService = Application.Interface.Services.IUsuariosCrudFormsService;

namespace APISunSale.Utils
{
    public class MainUtils
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserService _userService;
        private readonly UserCrudFormsService _userCrudFormsService;

        public MainUtils(IHttpContextAccessor httpContextAccessor, UserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public MainUtils(IHttpContextAccessor httpContextAccessor, UserCrudFormsService userCrudFormsService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userCrudFormsService = userCrudFormsService;
        }

        public async Task<Usuarios> GetUserFromContextAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var token = httpContext?.Request?.Headers["Authorization"].ToString().Substring("Bearer ".Length);
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            // Get the username from the "sub" claim
            string username = jwtToken.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            var user = await _userService.GetByEmail(username);
            return user;
        }

        public async Task<UsuariosCrudForms> GetUserCrudFormsFromContextAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var token = httpContext?.Request?.Headers["Authorization"].ToString().Substring("Bearer ".Length);
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            // Get the username from the "sub" claim
            string username = jwtToken.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            var user = await _userCrudFormsService.GetByEmail(username);
            return user;
        }
    }
}
