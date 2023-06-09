using Application.Interface.Services;
using AutoMapper;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MainEntity = Domain.Entities.DadosEstagiario;
using Service = Application.Interface.Services.IEstagioService;
using LoggerService = Application.Interface.Services.ILoggerService;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EstagiarioController
    {
        private readonly ILogger<EstagiarioController> _logger;
        private readonly Service _service;
        private readonly IMapper _mapper;
        private readonly LoggerService _loggerService;

        public EstagiarioController(ILogger<EstagiarioController> logger, Service service, IMapper mapper, LoggerService loggerService)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseBase<string>> Add([FromBodyAttribute] MainEntity input)
        {
            try
            {
                var result = _service.CriaDocumento(input);
                return new ResponseBase<string>()
                {
                    Message = "Created",
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
