using Application.Interface.Services;
using AutoMapper;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MainEntity = Domain.Entities.Imagem;
using Service = Application.Interface.Services.IImageMagicService;
using LoggerService = Application.Interface.Services.ILoggerService;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImageController
    {
        private readonly ILogger<ImageController> _logger;
        private readonly Service _service;
        private readonly IMapper _mapper;
        private readonly LoggerService _loggerService;

        public ImageController(ILogger<ImageController> logger, Service service, IMapper mapper, LoggerService loggerService)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseBase<MainEntity>> Add([FromBodyAttribute] MainEntity input)
        {
            try
            {
                var result = await _service.TreatAsync(input);
                return new ResponseBase<MainEntity>()
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

                return new ResponseBase<MainEntity>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
