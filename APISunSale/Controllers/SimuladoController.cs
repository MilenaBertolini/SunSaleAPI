using Application.Interface.Services;
using AutoMapper;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MainViewModel = Domain.ViewModel.SimuladosViewModel;
using MainEntity = Domain.Entities.Simulados;
using Service = Application.Interface.Services.ISimuladoService;
using ServiceQuestoes = Application.Interface.Services.IQuestoesService;
using LoggerService = Application.Interface.Services.ILoggerService;
using UserService = Application.Interface.Services.IUsuariosService;
using Application.Model;
using APISunSale.Utils;
using Domain.Entities;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SimuladoController
    {
        private readonly ILogger<SimuladoController> _logger;
        private readonly Service _service;
        private readonly ServiceQuestoes _serviceQuestoes;
        private readonly UserService _userService;
        private readonly IMapper _mapper;
        private readonly LoggerService _loggerService;
        private readonly MainUtils _utils;

        public SimuladoController(ILogger<SimuladoController> logger, Service service, IMapper mapper, LoggerService loggerService, IHttpContextAccessor httpContextAccessor, UserService userService, ServiceQuestoes serviceQuestoes)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _loggerService = loggerService;
            _userService = userService;
            _utils = new MainUtils(httpContextAccessor, userService);
            _serviceQuestoes = serviceQuestoes;
        }

        [HttpGet("pagged")]
        public async Task<ResponseBase<List<MainViewModel>>> GetAllPagged(int page, int quantity, int? codeUser)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                var result = await _service.GetAllPagged(page, quantity, codeUser.HasValue ? codeUser.Value : user.Id);
                var response = _mapper.Map<List<MainViewModel>>(result);
                return new ResponseBase<List<MainViewModel>>()
                {
                    Message = "List created",
                    Success = true,
                    Object = response,
                    Quantity = response?.Count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                await _loggerService.AddException(ex);

                return new ResponseBase<List<MainViewModel>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet("getById")]
        public async Task<ResponseBase<MainViewModel>> GetById(int id)
        {
            try
            {
                var result = await _service.GetById(id);
                var response = _mapper.Map<MainViewModel>(result);
                return new ResponseBase<MainViewModel>()
                {
                    Message = "Search success",
                    Success = true,
                    Object = response,
                    Quantity = response != null ? 1 : 0
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

        [HttpPost]
        public async Task<ResponseBase<MainViewModel>> Add([FromBodyAttribute] MainViewModel main)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();
                List<Simulado> simulados = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Simulado>>(main?.Respostas);

                main.CodigoUsuario = user.Id;
                main.QuantidadeQuestoes = simulados?.Count();
                main.QuantidadeCertas = simulados?.Where(s => s.certa.Equals("1")).Count();

                var result = await _service.Add(_mapper.Map<MainEntity>(main));
                return new ResponseBase<MainViewModel>()
                {
                    Message = "Created",
                    Success = true,
                    Object = _mapper.Map<MainViewModel>(result),
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

        [HttpGet("reportDetail")]
        public async Task<ResponseBase<string>> GetRepostDetaild(int codigoProva, int? codigoUsuario)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();
                if (!codigoUsuario.HasValue)
                    codigoUsuario = user.Id;
                else if(user.Admin != "1")
                {
                    return new ResponseBase<string>()
                    {
                        Message = "Acesso não autorizado",
                        Success = false
                    };
                }

                user = await _userService.GetById(codigoUsuario.Value);
                var simulado = await _service.GetByProvaUser(codigoProva, codigoUsuario.Value);

                if(simulado == null)
                {
                    return new ResponseBase<string>()
                    {
                        Message = "Simulado não encontrado",
                        Success = false
                    };
                }


                List<Simulado> simulados = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Simulado>>(simulado.Respostas);
                var questoes = await _serviceQuestoes.GetQuestoesByProva(codigoProva);

                var result = _service.CriaDocumentoDetalhado(questoes, simulado, user, simulados);
                return new ResponseBase<string>()
                {
                    Message = "Listed",
                    Success = true,
                    Object = result,
                    Quantity = 1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                await _loggerService.AddException(ex);

                return new ResponseBase<string>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpPut]
        public async Task<ResponseBase<MainViewModel>> Update([FromBodyAttribute] MainViewModel main)
        {
            try
            {
                var result = await _service.Update(_mapper.Map<MainEntity>(main));
                return new ResponseBase<MainViewModel>()
                {
                    Message = "Updated",
                    Success = true,
                    Object = _mapper.Map<MainViewModel>(result),
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

        [HttpDelete]
        public async Task<ResponseBase<bool>> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteById(id);
                return new ResponseBase<bool>()
                {
                    Message = "Deleted",
                    Success = true,
                    Object = result,
                    Quantity = 1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                await _loggerService.AddException(ex);

                return new ResponseBase<bool>()
                {
                    Message = ex.Message,
                    Success = false,
                    Object = false
                };
            }
        }
    }
}
