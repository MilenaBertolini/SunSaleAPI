using Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using UserService = Application.Interface.Services.IUsuariosService;

namespace APISunSale.Utils
{
    public class MainUtils
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserService _userService;

        public MainUtils(IHttpContextAccessor httpContextAccessor, UserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
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
    }
}
