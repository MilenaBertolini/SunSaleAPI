using AutoMapper;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MainViewModel = Domain.ViewModel.MeuDesempenhoViewModel;
using Service = Application.Interface.Services.IMeuDesempenhoService;
using LoggerService = Application.Interface.Services.ILoggerService;
using UserService = Application.Interface.Services.IUsuariosService;
using APISunSale.Utils;
using Domain.ViewModel;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MeuDesempenhoController
    {
        private readonly ILogger<MeuDesempenhoController> _logger;
        private readonly Service _service;
        private readonly IMapper _mapper;
        private readonly LoggerService _loggerService;
        private readonly MainUtils _utils;

        public MeuDesempenhoController(ILogger<MeuDesempenhoController> logger, Service service, IMapper mapper, LoggerService loggerService, IHttpContextAccessor httpContextAccessor, UserService userService)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _loggerService = loggerService;
            _utils = new MainUtils(httpContextAccessor, userService);
        }

        [HttpGet("analysis")]
        public async Task<ResponseBase<MainViewModel>> GetAllDados()
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                var result = await _service.GetAllDados(user.Id);
                var response = _mapper.Map<MainViewModel>(result);
                return new ResponseBase<MainViewModel>()
                {
                    Message = "List created",
                    Success = true,
                    Object = response,
                    Quantity = 1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                await _loggerService.AddException(ex);

                return new ResponseBase<MainViewModel>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
