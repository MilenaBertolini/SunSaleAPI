using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MainViewModel = Domain.ViewModel.TeamViewModel;
using Service = Application.Interface.Services.IRandomRaxaService;
using LoggerService = Application.Interface.Services.ILoggerService;
using AutoMapper;
using Domain.Responses;
using Domain.ViewModel;
using System.Reflection;
using Domain.Entities;
using System.Collections.Generic;
using Application.Implementation.Services;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class RandomRaxaController
    {
        private readonly ILogger<RandomRaxaController> _logger;
        private readonly Service _service;
        private readonly IMapper _mapper;
        private readonly LoggerService _loggerService;

        public RandomRaxaController(ILogger<RandomRaxaController> logger, Service service, IMapper mapper, LoggerService loggerService)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        [HttpPost("getRandomTeam")]
        public ResponseBase<List<TeamResponse>> GetRandomTeam(List<Players> playears, int numeroJogadoresLinha = 4)
        {
            try
            {
                _loggerService.AddInfo("Buscando time random");
                var result = _service.GetTeams(playears, numeroJogadoresLinha);

                List<TeamResponse> toReturn = new List<TeamResponse>();
                foreach (var item in result)
                {
                    var temp = new TeamResponse()
                    {
                        Playears = new List<string>()
                    };

                    foreach(var item2 in item.Players) 
                    {
                        temp.Playears.Add(item2.Nome);
                    }

                    toReturn.Add(temp);
                }

                return new ResponseBase<List<TeamResponse>>()
                {
                    Message = "List created",
                    Success = true,
                    Object = toReturn,
                    Quantity = 1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                _loggerService.AddException(ex);

                return new ResponseBase<List<TeamResponse>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
