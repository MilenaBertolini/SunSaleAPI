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
using EmailService = Application.Interface.Services.IEmailService;
using ProvaService = Application.Interface.Services.IProvaService;
using Application.Model;
using APISunSale.Utils;
using Domain.Entities;
using Domain.ViewModel;
using System.Text;

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
        private readonly EmailService _emailService;
        private readonly ProvaService _provaService;
        private readonly MainUtils _utils;

        public SimuladoController(ILogger<SimuladoController> logger, Service service, IMapper mapper, LoggerService loggerService, IHttpContextAccessor httpContextAccessor, UserService userService, ServiceQuestoes serviceQuestoes, EmailService emailService, ProvaService provaService)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _loggerService = loggerService;
            _userService = userService;
            _utils = new MainUtils(httpContextAccessor, userService);
            _serviceQuestoes = serviceQuestoes;
            _emailService = emailService;
            _provaService = provaService;
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

        [HttpPost("sendReportEmail")]
        public async Task<ResponseBase<bool>> SendReportEmail(int codigoSimulado)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();
                var main = await _service.GetById(codigoSimulado);

                if (main == null)
                {
                    return new ResponseBase<bool>()
                    {
                        Message = "Simulado não encontrado",
                        Success = false,
                        Object = false
                    };
                }

                if(main.CodigoUsuario != user.Id && user.Admin != "1")
                {
                    return new ResponseBase<bool>()
                    {
                        Message = "Você não tem acesso a esse simulado!",
                        Success = false,
                        Object = false
                    };
                }

                var questoes = await _serviceQuestoes.GetQuestoesByProva(main.CodigoProva);
                List<Simulado> simulados = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Simulado>>(main.Respostas);

                var text = _service.CriaDocumentoDetalhado(questoes, _mapper.Map<Simulados>(main), user, simulados);

                text = text.Replace("data:application/json;base64,", "");
                text = Encoding.UTF8.GetString(Convert.FromBase64String(text));

                await _emailService.Add(_mapper.Map<Email>(new EmailViewModel()
                {
                    Assunto = "Detalhes Simulado - " + main.Prova.NomeProva,
                    Destinatario = user.Email,
                    Texto = text,
                    Status = "0"
                }));

                return new ResponseBase<bool>()
                {
                    Message = "Email enviado",
                    Success = true,
                    Object = true
                };
            }
            catch(Exception ex)
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

        [HttpGet("reportDetail")]
        public async Task<ResponseBase<string>> GetRepostDetaild(int codigoSimulado)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                var simulado = await _service.GetById(codigoSimulado);

                if(simulado == null)
                {
                    return new ResponseBase<string>()
                    {
                        Message = "Simulado não encontrado",
                        Success = false
                    };
                }

                if (simulado.CodigoUsuario != user.Id && user.Admin != "1")
                {
                    return new ResponseBase<string>()
                    {
                        Message = "Você não tem acesso a esse simulado!",
                        Success = false
                    };
                }

                List<Simulado> simulados = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Simulado>>(simulado.Respostas);
                var questoes = await _serviceQuestoes.GetQuestoesByProva(simulado.CodigoProva);

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
                var user = await _utils.GetUserFromContextAsync();

                if (user.Admin != "1")
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Acesso não autorizado",
                        Success = false
                    };
                }

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
                var user = await _utils.GetUserFromContextAsync();

                if (user.Admin != "1")
                {
                    return new ResponseBase<bool>()
                    {
                        Message = "Acesso não autorizado",
                        Success = false
                    };
                }

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
