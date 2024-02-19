using Application.Interface.Services;
using AutoMapper;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MainViewModel = Domain.ViewModel. AvaliacaoViewModel;
using MainEntity = Domain.Entities.Avaliacao;
using Service = Application.Interface.Services.IAvaliacaoService;
using LoggerService = Application.Interface.Services.ILoggerService;
using UserService = Application.Interface.Services.IUsuariosService;
using APISunSale.Utils;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AvaliacaoController
    {
        private readonly ILogger<AvaliacaoController> _logger;
        private readonly Service _service;
        private readonly IMapper _mapper;
        private readonly LoggerService _loggerService;
        private readonly MainUtils _utils;

        public AvaliacaoController(ILogger<AvaliacaoController> logger, Service service, IMapper mapper, LoggerService loggerService, IHttpContextAccessor httpContextAccessor, UserService userService)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _loggerService = loggerService;
            _utils = new MainUtils(httpContextAccessor, userService);
        }

        [HttpGet("pagged")]
        public async Task<ResponseBase<List<MainViewModel>>> GetAllPagged(int page, int quantity, string? chave, int user = -1)
        {
            try
            {
                var result = await _service.GetAllPagged(page, quantity, chave, user);
                var response = _mapper.Map<List<MainViewModel>>(result);

                response.ForEach(item =>
                {
                    item.NotaTotal = item.QuestoesAvaliacao?.Sum(q => q.NotaQuestao);
                });

                return new ResponseBase<List<MainViewModel>>()
                {
                    Message = "List created",
                    Success = true,
                    Object = response,
                    Quantity = response?.Count,
                    Total = await _service.QuantidadeTotal()
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

                response.NotaTotal = response.QuestoesAvaliacao?.Sum(q => q.NotaQuestao);

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

        [HttpGet("getAll")]
        public async Task<ResponseBase<List<MainViewModel>>> GetAll()
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                if (user.Admin != "1" && user.Admin != "2")
                {
                    return new ResponseBase<List<MainViewModel>>()
                    {
                        Message = "You don't have access!",
                        Object = null,
                        Quantity = 0,
                        Success = false,
                        Total = 0
                    };
                }

                var result = await _service.GetByUserId(user.Id);

                var response = _mapper.Map<List<MainViewModel>>(result);
                return new ResponseBase<List<MainViewModel>>()
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

                return new ResponseBase<List<MainViewModel>>()
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

                if(user.Admin != "1" && user.Admin != "2")
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

                await _loggerService.AddInfo($"Inserindo nova avaliação pelo usuário {user.Nome}");

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

        [HttpPost("Edit")]
        public async Task<ResponseBase<MainViewModel>> Edit([FromBodyAttribute] MainViewModel main)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                if (user.Admin != "1" && user.Admin != "2")
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

                await _loggerService.AddInfo($"Inserindo nova avaliação pelo usuário {user.Nome}");

                var result = await _service.Update(_mapper.Map<MainEntity>(main), user.Id);
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

                if (user.Admin != "1" && user.Admin != "2")
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
                
                await _loggerService.AddInfo($"Atualização nova avaliação pelo usuário {user.Nome}");

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
                var user = await _utils.GetUserFromContextAsync();

                if (user.Admin != "1" && user.Admin != "2")
                {
                    return new ResponseBase<bool>()
                    {
                        Message = "You don't have access!",
                        Object = false,
                        Quantity = 0,
                        Success = false,
                        Total = 0
                    };
                }

                await _loggerService.AddInfo($"Apagando nova avaliação pelo usuário {user.Nome}");

                var result = await _service.DeleteById(id, user.Id);
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
