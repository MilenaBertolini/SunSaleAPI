using AutoMapper;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MainViewModel = Domain.ViewModel.AdminDataViewModel;
using Service = Application.Interface.Services.IAdminService;
using LoggerService = Application.Interface.Services.ILoggerService;
using UserService = Application.Interface.Services.IUsuariosService;
using APISunSale.Utils;
using Domain.ViewModel;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AdminController
    {
        private readonly ILogger<AdminController> _logger;
        private readonly Service _service;
        private readonly IMapper _mapper;
        private readonly LoggerService _loggerService;
        private readonly MainUtils _utils;

        public AdminController(ILogger<AdminController> logger, Service service, IMapper mapper, LoggerService loggerService, IHttpContextAccessor httpContextAccessor, UserService userService)
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

                if (!user.Admin.Equals("1"))
                {
                    return new ResponseBase<MainViewModel>() 
                    { 
                        Message = "You don't have access!",
                        Object = null,
                        Quantity = 0,
                        Success = false,
                        Total = 0
                    };
                }

                var result = await _service.GetAllDados();
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

        [HttpGet("questoespararevisao")]
        public async Task<ResponseBase<List<QuestoesViewModel>>> GetAllQuestoesParaRevisao(int page, int quantity)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                if (!user.Admin.Equals("1"))
                {
                    return new ResponseBase<List<QuestoesViewModel>>()
                    {
                        Message = "You don't have access!",
                        Object = null,
                        Quantity = 0,
                        Success = false,
                        Total = 0
                    };
                }

                var result = await _service.BuscaQuestoesSolicitadasRevisao(page, quantity);
                var response = _mapper.Map<List<QuestoesViewModel>>(result);
                return new ResponseBase<List<QuestoesViewModel>>()
                {
                    Message = "List created",
                    Success = true,
                    Object = response,
                    Quantity = 1,
                    Total = response.Count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                await _loggerService.AddException(ex);

                return new ResponseBase<List<QuestoesViewModel>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet("provaspararevisao")]
        public async Task<ResponseBase<List<ProvaViewModel>>> GetAllProvasParaRevisao(int page, int quantity)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                if (!user.Admin.Equals("1"))
                {
                    return new ResponseBase<List<ProvaViewModel>>()
                    {
                        Message = "You don't have access!",
                        Object = null,
                        Quantity = 0,
                        Success = false,
                        Total = 0
                    };
                }

                var result = await _service.BuscaProvasSolicitadasRevisao(page, quantity);
                var response = _mapper.Map<List<ProvaViewModel>>(result);
                return new ResponseBase<List<ProvaViewModel>>()
                {
                    Message = "List created",
                    Success = true,
                    Object = response,
                    Quantity = 1,
                    Total = response.Count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                await _loggerService.AddException(ex);

                return new ResponseBase<List<ProvaViewModel>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
