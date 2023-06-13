using AutoMapper;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MainViewModel = Domain.ViewModel.QuestoesViewModel;
using MainEntity = Domain.Entities.Questoes;
using Service = Application.Interface.Services.IQuestoesService;
using UserService = Application.Interface.Services.IUsuariosService;
using RespostasUserService = Application.Interface.Services.IRespostasUsuariosService;
using APISunSale.Utils;
using Domain.ViewModel;
using static Data.Helper.EnumeratorsTypes;
using LoggerService = Application.Interface.Services.ILoggerService;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuestoesController
    {
        private readonly ILogger<QuestoesController> _logger;
        private readonly Service _service;
        private readonly IMapper _mapper;
        private readonly MainUtils _utils;
        private readonly RespostasUserService _respostasUserService;
        private readonly LoggerService _loggerService;

        public QuestoesController(ILogger<QuestoesController> logger, Service service, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserService userService, RespostasUserService respostasUserService, LoggerService loggerService)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _utils = new MainUtils(httpContextAccessor, userService);
            _respostasUserService = respostasUserService;
            _loggerService = loggerService;
        }

        [HttpGet("pagged")]
        public async Task<ResponseBase<List<MainViewModel>>> GetAllPagged(int page, int quantity, bool anexos, int? codigoProva, string? subject)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                var result = await _service.GetAllPagged(page, quantity, user.Id, anexos, codigoProva, subject);
                var response = _mapper.Map<List<MainViewModel>>(result.Item1);

                var temp = _mapper.Map<IList<RespostasUsuariosViewModel>>(await _respostasUserService.GetByUserQuestao(user.Id));
                foreach(var item in response)
                {
                    item.RespostasUsuarios = temp.Where(r => item.RespostasQuestoes.Where(re => re.Codigo.Equals(r.CodigoResposta)).Count() > 0).ToList();
                }

                return new ResponseBase<List<MainViewModel>>()
                {
                    Message = "List created",
                    Success = true,
                    Object = response,
                    Quantity = response?.Count,
                    Total = result?.Item2
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
                var user = await _utils.GetUserFromContextAsync();

                var result = await _service.GetById(id);

                if(result == null)
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Not registered",
                        Success = false
                    };
                }

                var response = _mapper.Map<MainViewModel>(result);

                var temp = await _respostasUserService.GetByUserQuestao(user.Id, response.Codigo);
                response.RespostasUsuarios = _mapper.Map<IList<RespostasUsuariosViewModel>>(temp);

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

        [HttpGet("getQuestao")]
        public async Task<ResponseBase<MainViewModel>> GetQuestao(int codigoProva, int? numeroQuestao)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                int questao = numeroQuestao.HasValue ? numeroQuestao.Value : 0;

                var result = await _service.GetQuestoesByProva(codigoProva, questao);

                if (result == null)
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Not registered",
                        Success = false
                    };
                }

                var response = _mapper.Map<MainViewModel>(result);
                var temp = await _respostasUserService.GetByUserQuestao(user.Id, response.Codigo);
                response.RespostasUsuarios = _mapper.Map<IList<RespostasUsuariosViewModel>>(temp);

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

                var result = await _service.Add(_mapper.Map<MainEntity>(main), user.Id);
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
                if(await _service.GetById(main.Codigo) == null)
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Questão doesn't exists",
                        Success = false
                    };
                }

                var user = await _utils.GetUserFromContextAsync();
                var result = await _service.Update(_mapper.Map<MainEntity>(main), user.Id);
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

        [HttpGet]
        [Route("GetAllMaterias")]
        public async Task<ResponseBase<List<string>>> GetAllMaterias()
        {
            try
            {
                var result = await _service.GetAllMateris();

                return new ResponseBase<List<string>>()
                {
                    Message = "List created",
                    Success = true,
                    Object = result?.ToList(),
                    Quantity = result?.Count()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                await _loggerService.AddException(ex);

                return new ResponseBase<List<string>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet("getQuestaoAleatoria")]
        public async Task<ResponseBase<MainViewModel>> GetQuestaoAleatoria(TipoQuestoes tipo, string? subject, string? bancas)
        {
            try
            {
                var result = await _service.GetQuestoesAleatoria(tipo, subject, bancas);

                if (result == null)
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Not registered",
                        Success = false
                    };
                }

                var user = await _utils.GetUserFromContextAsync();
                var response = _mapper.Map<MainViewModel>(result);
                var temp = await _respostasUserService.GetByUserQuestao(user.Id, response.Codigo);
                response.RespostasUsuarios = _mapper.Map<IList<RespostasUsuariosViewModel>>(temp);

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

        [HttpGet("solicitaVerificacao")]
        public async Task<ResponseBase<MainViewModel>> SolicitaVerificacao(int id)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                var result = await _service.GetById(id);

                if (result == null)
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Not registered",
                        Success = false
                    };
                }

                result.Ativo = "0";
                await _service.Update(result, user.Id);

                var response = _mapper.Map<MainViewModel>(result);

                var temp = await _respostasUserService.GetByUserQuestao(user.Id, response.Codigo);
                response.RespostasUsuarios = _mapper.Map<IList<RespostasUsuariosViewModel>>(temp);

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
    }
}
