using Application.Interface.Services;
using AutoMapper;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MainViewModel = Domain.ViewModel.WhosThatPokemonResultViewModel;
using MainEntity = Domain.Entities.WhosThatPokemonResult;
using Service = Application.Interface.Services.IWhosThatPokemonResultService;
using LoggerService = Application.Interface.Services.ILoggerService;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WhosThatPokemonResultController : ControllerBase
    {
        private readonly ILogger<WhosThatPokemonResultController> _logger;
        private readonly Service _service;
        private readonly IMapper _mapper;
        private readonly LoggerService _loggerService;

        public WhosThatPokemonResultController(ILogger<WhosThatPokemonResultController> logger, Service service, IMapper mapper, LoggerService loggerService)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _loggerService = loggerService;
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
        [AllowAnonymous]
        public async Task<ResponseBase<MainViewModel>> Add([FromBodyAttribute] MainViewModel main)
        {
            try
            {
                var link = base.HttpContext.Request.Headers["Referer"];
                
                //if(link.Count == 0)
                //{
                //    return new ResponseBase<MainViewModel>()
                //    {
                //        Message = "Not authorized",
                //        Success = false
                //    };
                //}

                //if (!link[0].ToString().Contains("http://localhost:4200/") && !link[0].ToString().Contains("whatsthatpokemon") && !link[0].ToString().Contains("qualpokemon"))
                //{
                //    return new ResponseBase<MainViewModel>()
                //    {
                //        Message = "Not authorized",
                //        Success = false
                //    };
                //}


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

        [HttpGet("ranking")]
        [AllowAnonymous]
        public async Task<ResponseBase<RankingWhosThatPokemon>> GetRaking()
        {
            try
            {
                var resultCustom = await _service.GetRanking(true);
                var resultClassic = await _service.GetRanking(false);

                RankingWhosThatPokemon ranking = new RankingWhosThatPokemon()
                {
                    ListaClassic = _mapper.Map<List<MainViewModel>>(resultClassic),
                    ListaCustom = _mapper.Map<List<MainViewModel>>(resultCustom)
                };

                return new ResponseBase<RankingWhosThatPokemon>()
                {
                    Message = "Listed",
                    Success = true,
                    Object = ranking,
                    Quantity = 1,
                    Total = 1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                await _loggerService.AddException(ex);

                return new ResponseBase<RankingWhosThatPokemon>()
                {
                    Message = ex.Message,
                    Success = false,
                    Object = null
                };
            }
        }
    }
}
