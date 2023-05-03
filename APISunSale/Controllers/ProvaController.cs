using AutoMapper;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MainViewModel = Domain.ViewModel.ProvaViewModel;
using MainEntity = Domain.Entities.Prova;
using Service = Application.Interface.Services.IProvaService;
using QuestoesService = Application.Interface.Services.IQuestoesService;
using UserService = Application.Interface.Services.IUsuariosService;
using APISunSale.Utils;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProvaController 
    {
        private readonly ILogger<ProvaController> _logger;
        private readonly Service _service;
        private readonly IMapper _mapper;
        private readonly MainUtils _utils;
        private readonly QuestoesService _questoesService;

        public ProvaController(ILogger<ProvaController> logger, Service service, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserService userService, QuestoesService questoesService)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _utils = new MainUtils(httpContextAccessor, userService);
            _questoesService = questoesService;
        }

        [HttpGet]
        public async Task<ResponseBase<List<MainViewModel>>> GetAll()
        {
            try
            {
                var result = await _service.GetAll();
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
                return new ResponseBase<List<MainViewModel>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet("pagged")]
        public async Task<ResponseBase<List<MainViewModel>>> GetAllPagged(int page, int quantity)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                var result = await _service.GetAllPagged(page, quantity);
                var response = _mapper.Map<List<MainViewModel>>(result.Item1);

                foreach(var item in response)
                {
                    item.QuantidadeQuestoesResolvidas = await _questoesService.QuantidadeQuestoes(item.Codigo, user.Id);
                    item.QuantidadeQuestoesTotal = await _questoesService.QuantidadeQuestoes(item.Codigo);
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
                return new ResponseBase<MainViewModel>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBodyAttribute] MainViewModel main)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                var result = await _service.Add(_mapper.Map<MainEntity>(main), user.Id);
                return new OkObjectResult(
                    new ResponseBase<MainViewModel>()
                    {
                        Message = "Created",
                        Success = true,
                        Object = _mapper.Map<MainViewModel>(result),
                        Quantity = 1
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                return new BadRequestObjectResult(
                    new
                    {
                        Message = ex.Message,
                        Success = false
                    }
                );
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
                        Message = "Prova doesn't exists",
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
                return new ResponseBase<bool>()
                {
                    Message = ex.Message,
                    Success = false,
                    Object = false
                };
            }
        }

        [HttpGet]
        [Route("GetAllBancas")]
        public async Task<ResponseBase<List<string>>> GetAllBancas()
        {
            try
            {
                var result = await _service.GetAll();
                List<string> response = _mapper.Map<List<string>>(result.Select(r => r.Banca)).Distinct().ToList();

                return new ResponseBase<List<string>>()
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
                return new ResponseBase<List<string>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet]
        [Route("GetAllProvas")]
        public async Task<ResponseBase<List<PorvasReturn>>> GetAllProvas()
        {
            try
            {
                var result = await _service.GetAll();
                List<PorvasReturn> response = _mapper.Map<List<PorvasReturn>>(
                result.Select(r => 
                new PorvasReturn() 
                { 
                    Codigo = r.Codigo, 
                    Nome = r.NomeProva 
                } 
                )).Distinct().ToList();

                return new ResponseBase<List<PorvasReturn>>()
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
                return new ResponseBase<List<PorvasReturn>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
