﻿using Application.Model;
using AutoMapper;
using Domain.Entities;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Service = Application.Interface.Services.IQuestoesService;
using ServiceProva = Application.Interface.Services.IProvaService;
using ServiceRespostas = Application.Interface.Services.IRespostasQuestoesService;
using LoggerService = Application.Interface.Services.ILoggerService;
using Domain.ViewModel;
using System.Collections.Generic;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]

    public class PublicQuestoesController
    {
        private readonly ILogger<PublicQuestoesController> _logger;
        private readonly Service _service;
        private readonly ServiceRespostas _serviceResposta;
        private readonly ServiceProva _serviceProva;
        private readonly IMapper _mapper;
        private readonly LoggerService _loggerService;

        public PublicQuestoesController(ILogger<PublicQuestoesController> logger, Service service, IMapper mapper, ServiceRespostas serviceResposta, ServiceProva serviceProva, LoggerService loggerService)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _serviceResposta = serviceResposta;
            _serviceProva = serviceProva;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Método que busca todas as provas
        /// </summary>
        /// <param name="id">Você pode passar o parâmtro para selecionar pelo código da prova ou pode não passar para retornar todas</param>
        /// <returns>Listagem de todas as provas</returns>
        [HttpGet("GetTests")]
        public async Task<ActionResult> GetProvas(int? id)
        {
            try
            {
                var result = await _service.GetTests(id);
                await _loggerService.AddInfo("Buscando prova" + (id.HasValue ? $" filtrando por {id.Value}" : " sem filtro"));

                return new OkObjectResult(
                    new ResponseBase<List<Test>>()
                    {
                        Message = "List created",
                        Success = true,
                        Object = result?.ToList(),
                        Quantity = result?.ToList()?.Count
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                await _loggerService.AddException(ex);

                return new BadRequestObjectResult(
                    new
                    {
                        Message = ex.Message,
                        Success = false
                    }
                );
            }
        }

        /// <summary>
        /// Busca todas as matérias que tem questões cadastradas
        /// </summary>
        /// <param name="prova">Você pode passar o parâmetro para selecionar pelo código da prova ou pode não passar para retornar todas</param>
        /// <returns>Uma lista de string com as matérias</returns>
        [HttpGet("GetMaterias")]
        public async Task<ActionResult> GetMaterias(int? prova)
        {
            try
            {
                var result = await _service.GetMaterias(prova);
                await _loggerService.AddInfo("Buscando matérias de provas");

                return new OkObjectResult(
                    new ResponseBase<List<string>>()
                    {
                        Message = "List created",
                        Success = true,
                        Object = result.ToList(),
                        Quantity = result?.ToList()?.Count
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                await _loggerService.AddException(ex);

                return new BadRequestObjectResult(
                    new
                    {
                        Message = ex.Message,
                        Success = false
                    }
                );
            }
        }

        /// <summary>
        /// Busca as questões dado um filtro
        /// </summary>
        /// <param name="codigoQuestao">Para filtrar a questão pelo seu código</param>
        /// <param name="codigoProva">Filtra questões de uma prova</param>
        /// <param name="materia">Fitlra questão de uma matéria</param>
        /// <param name="numeroQuestao">Filtra a questão pelo número quando preenchido também o código da prova</param>
        /// <returns></returns>
        [HttpGet("GetQuestoes")]
        public async Task<ActionResult> GetQuestoes(int? codigoQuestao, int? codigoProva, string? materia, int? numeroQuestao)
        {
            try
            {
                if(codigoProva == null && materia == null && codigoQuestao == null && numeroQuestao == null)
                {
                    return new BadRequestObjectResult(new { message = "Precisa passar ao menos algum parâmetro" });
                }
                else if (numeroQuestao != null && codigoProva == null) 
                { 
                    return new BadRequestObjectResult(new { message = "Foi passado o número da questão mas o mesmo não foi listado" });
                }

                await _loggerService.AddInfo("Buscando questões");

                List<Questoes> result = new List<Questoes>();

                if(codigoQuestao != null)
                {
                    result.Add(await _service.GetById(codigoQuestao.Value));
                }
                else if(codigoProva != null)
                {
                    result.AddRange(_service.GetQuestoesByProva(codigoProva.Value).Result);
                    if(materia != null)
                    {
                        result = result.Where(p => p.Materia.Equals(materia)).ToList();
                    }

                    if (numeroQuestao != null)
                        result = result.Where(q => q.NumeroQuestao.Equals(numeroQuestao)).ToList();
                }
                else if(materia != null)
                {
                    result.AddRange(_service.GetQuestoesByMateria(materia).Result);
                }

                return new OkObjectResult( 
                    new ResponseBase<List<QuestoesViewModel>>()
                    {
                        Message = result.ToList().Count > 0 ? "List created" : "No returns by the given filter",
                        Success = true,
                        Object = _mapper.Map<List<QuestoesViewModel>>(result),
                        Quantity = result.ToList().Count
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                await _loggerService.AddException(ex);

                return new BadRequestObjectResult(
                    new
                    {
                        Message = ex.Message,
                        Success = false
                    }
                );
            }
        }

        /// <summary>
        /// Valida se a resposta está correta
        /// </summary>
        /// <param name="codigoResposta">Código da resposta para ser validado</param>
        /// <returns></returns>
        [HttpGet("GetResposta")]
        public async Task<ActionResult> ValidaResposta(int codigoResposta)
        {
            try
            {
                var result = await _serviceResposta.GetById(codigoResposta);
                await _loggerService.AddInfo("Valida se resposta está correta");

                if (result == null)
                {
                    return new BadRequestObjectResult(new { message = "Não existe a resposta selecionada" });
                }

                return new OkObjectResult(
                    new
                    {
                        Correta = result.Certa.Equals("1")
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                await _loggerService.AddException(ex);

                return new BadRequestObjectResult(
                    new
                    {
                        Message = ex.Message,
                        Success = false
                    }
                );
            }
        }
    }
}
