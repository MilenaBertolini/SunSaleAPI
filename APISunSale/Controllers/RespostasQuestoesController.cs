using Application.Interface.Services;
using AutoMapper;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MainViewModel = Domain.ViewModel.RespostasQuestoesViewModel;
using MainEntity = Domain.Entities.RespostasQuestoes;
using Service = Application.Interface.Services.IRespostasQuestoesService;
using RespostaUsuarioService = Application.Interface.Services.IRespostasUsuariosService;
using UserService = Application.Interface.Services.IUsuariosService;
using ProvaService = Application.Interface.Services.IProvaService;
using APISunSale.Utils;
using LoggerService = Application.Interface.Services.ILoggerService;
using QuestoesService = Application.Interface.Services.IQuestoesService;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RespostasQuestoesController
    {
        private readonly ILogger<RespostasQuestoesController> _logger;
        private readonly Service _service;
        private readonly IMapper _mapper;
        private readonly RespostaUsuarioService _respostaUsuarioService;
        private readonly MainUtils _utils;
        private readonly LoggerService _loggerService;
        private readonly UserService _userService;
        private readonly QuestoesService _questoesService;
        private readonly ProvaService _provaService;

        public RespostasQuestoesController(ILogger<RespostasQuestoesController> logger, Service service, IMapper mapper, RespostaUsuarioService respostaUsuarioService, IHttpContextAccessor httpContextAccessor, UserService userService, LoggerService loggerService, QuestoesService questoesService, ProvaService provaService)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _respostaUsuarioService = respostaUsuarioService;
            _userService = userService;
            _utils = new MainUtils(httpContextAccessor, userService);
            _loggerService = loggerService;
            _questoesService = questoesService;
            _provaService = provaService;
        }

        [HttpGet("pagged")]
        public async Task<ResponseBase<List<MainViewModel>>> GetAllPagged(int page, int quantity)
        {
            try
            {
                var result = await _service.GetAllPagged(page, quantity);
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

        [HttpPost]
        public async Task<ResponseBase<MainViewModel>> Add([FromBodyAttribute] MainViewModel main)
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

        [HttpGet("validaResposta")]
        public async Task<ResponseBase<MainViewModel>> ValidaResposta(int id)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                var result = await _service.GetById(id);

                await _respostaUsuarioService.Add(new Domain.Entities.RespostasUsuarios()
                {
                    CodigoResposta = result.Codigo,
                    CodigoUsuario = user.Id,
                    DataResposta = DateTime.Now,
                    CodigoQuestao = result.CodigoQuestao
                });

                var response = _mapper.Map<MainViewModel>(result);
                return new ResponseBase<MainViewModel>()
                {
                    Message = "Search success",
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

        [HttpGet("getRespostaCorreta")]
        public async Task<ResponseBase<MainViewModel>> GetRespostaCorreta(int questao)
        {
            try
            {
                var result = await _service.GetRespostaCorreta(questao);
                var response = _mapper.Map<MainViewModel>(result);

                return new ResponseBase<MainViewModel>()
                {
                    Message = "Search success",
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

        [HttpGet("reportDetail")]
        public async Task<ResponseBase<string>> GetReportDetaild(int? codigoUsuario)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();
                if (!codigoUsuario.HasValue)
                    codigoUsuario = user.Id;
                else if (user.Admin != "1")
                {
                    return new ResponseBase<string>()
                    {
                        Message = "Acesso não autorizado",
                        Success = false
                    };
                }

                user = await _userService.GetById(codigoUsuario.Value);
                var questoes = await _questoesService.GetQuestoesRespondidas(user.Id);
                var provas = await _provaService.GetAll();
                var respostasUsuarios = await _respostaUsuarioService.GetByUser(user.Id);

                var result = _service.CriaDocumentoDetalhado(questoes, user, provas.ToList(), respostasUsuarios.ToList());
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
    }
}
