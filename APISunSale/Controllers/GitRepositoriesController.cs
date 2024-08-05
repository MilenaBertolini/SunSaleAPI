using Application.Interface.Services;
using Domain.Entities;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GitRepositoriesController
    {
        private readonly ILogger<GitRepositoriesController> _logger;
        private readonly IGitApiService _service;
        private readonly ILoggerService _loggerService;

        public GitRepositoriesController(ILogger<GitRepositoriesController> logger, IGitApiService gitApiService, ILoggerService loggerService)
        {
            _logger = logger;
            _service = gitApiService;
            _loggerService = loggerService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ResponseBase<List<Postagem>>> GetAllPagged(int page, int quantity, int id = 0)
        {
            try
            {
                var result = await _service.BuscaInformacoesPessoais(page, quantity, id);
                return new ResponseBase<List<Postagem>>()
                {
                    Message = "List created",
                    Success = true,
                    Object = result.Item1,
                    Quantity = result?.Item1.Count() ?? 0,
                    Total = result?.Item2
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                await _loggerService.AddException(ex);

                return new ResponseBase<List<Postagem>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
