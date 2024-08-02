using Application.Interface.Services;
using AutoMapper;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MainEntity = Domain.Entities.RelatorioGrupoWpp;
using Service = Application.Interface.Services.IWppData;
using LoggerService = Application.Interface.Services.ILoggerService;
using System.IO.Compression;
using Domain.ViewModel;

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
        public async Task<ResponseBase<MainEntity>> Add(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return new ResponseBase<MainEntity>()
                    {
                        Message = "File is empty",
                        Success = false
                    };
                }

                if (!file.FileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    return new ResponseBase<MainEntity>()
                    {
                        Message = "File must be a .zip file",
                        Success = false
                    };
                }

                await _loggerService.AddInfo("Chamando dados wpp");

                List<string> textFileContents = new List<string>();
                MainEntity result = new MainEntity();
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    result = await _service.GetDadosWppsAsync(memoryStream);
                }

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

        [HttpGet]
        [AllowAnonymous]
        public async Task<ResponseBase<MainEntity>> GetAll(string id)
        {
            try
            {
                var result = await _service.GetRelatorioByToken(id);
                return new ResponseBase<MainEntity>()
                {
                    Message = "List created",
                    Success = true,
                    Object = result,
                    Quantity = 0
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
