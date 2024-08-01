using Application.Interface.Services;
using AutoMapper;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MainEntity = Domain.Entities.DadosWpp;
using Service = Application.Interface.Services.IWppData;
using LoggerService = Application.Interface.Services.ILoggerService;
using System.IO.Compression;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WppController
    {
        private readonly ILogger<WppController> _logger;
        private readonly Service _service;
        private readonly IMapper _mapper;
        private readonly LoggerService _loggerService;

        public WppController(ILogger<WppController> logger, Service service, IMapper mapper, LoggerService loggerService)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseBase<List<MainEntity>>> Add(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return new ResponseBase<List<MainEntity>>()
                    {
                        Message = "File is empty",
                        Success = false
                    };
                }

                if (!file.FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    return new ResponseBase<List<MainEntity>>()
                    {
                        Message = "File must be a .zip file",
                        Success = false
                    };
                }

                await _loggerService.AddInfo("Chamando dados wpp");

                List<string> textFileContents = new List<string>();
                List<MainEntity> result = new List<MainEntity>();
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    result = _service.GetDadosWppsAsync(memoryStream);
                }

                return new ResponseBase<List<MainEntity>>()
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

                return new ResponseBase<List<MainEntity>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
