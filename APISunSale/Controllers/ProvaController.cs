﻿using AutoMapper;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MainViewModel = Domain.ViewModel.ProvaViewModel;
using MainEntity = Domain.Entities.Prova;
using Service = Application.Interface.Services.IProvaService;
using QuestoesService = Application.Interface.Services.IQuestoesService;
using UserService = Application.Interface.Services.IUsuariosService;
using RespostasUserService = Application.Interface.Services.IRespostasUsuariosService;
using APISunSale.Utils;
using LoggerService = Application.Interface.Services.ILoggerService;

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
        private readonly RespostasUserService _respostasUserService;
        private readonly LoggerService _loggerService;

        public ProvaController(ILogger<ProvaController> logger, Service service, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserService userService, QuestoesService questoesService, RespostasUserService respostasUserService, LoggerService loggerService)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _utils = new MainUtils(httpContextAccessor, userService);
            _questoesService = questoesService;
            _respostasUserService = respostasUserService;
            _loggerService = loggerService;
        }

        [HttpGet]
        public async Task<ResponseBase<List<MainViewModel>>> GetAll()
        {
            try
            {
                var result = await _service.GetSimulados();
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

        [HttpGet("pagged")]
        public async Task<ResponseBase<List<MainViewModel>>> GetAllPagged(int page, int quantity, string? bancas, string? provas, string? tipos)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                var result = await _service.GetAllPagged(page, quantity, bancas, provas, tipos, user.Admin.Equals("1"));
                var response = _mapper.Map<List<MainViewModel>>(result.Item1);

                foreach (var item in response)
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

        [HttpPost]
        public async Task<ActionResult> Add([FromBodyAttribute] MainViewModel main)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                if (user.Admin != "1")
                {
                    return new OkObjectResult(
                        new ResponseBase<MainViewModel>()
                        {
                            Message = "Acesso não autorizado",
                            Success = false
                        });
                }

                var result = await _service.Add(_mapper.Map<MainEntity>(main), user.Id);

                await _loggerService.AddInfo($"Inserindo prova {result.Codigo}-{result.NomeProva} pelo usuário {user.Id}-{user.Nome}");

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

        [HttpPut]
        public async Task<ResponseBase<MainViewModel>> Update([FromBodyAttribute] MainViewModel main)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                if (user.Admin != "1")
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Acesso não autorizado",
                        Success = false
                    };
                }

                if (await _service.GetById(main.Codigo) == null)
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Prova doesn't exists",
                        Success = false
                    };
                }

                var result = await _service.Update(_mapper.Map<MainEntity>(main), user.Id);

                await _loggerService.AddInfo($"Alterando prova {result.Codigo}-{result.NomeProva} pelo usuário {user.Id}-{user.Nome}");

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

                if (user.Admin != "1")
                {
                    return new ResponseBase<bool>()
                    {
                        Message = "Acesso não autorizado",
                        Success = false
                    };
                }

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

        [HttpGet]
        [Route("GetAllBancas")]
        public async Task<ResponseBase<List<string>>> GetAllBancas(string provas="", string materias="", string assuntos = "", string tipos = "")
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                var result = await _service.GetBancas(provas, materias, assuntos, tipos, user.Admin.Equals("1"));
                List<string> response = _mapper.Map<List<string>>(result);

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
                await _loggerService.AddException(ex);

                return new ResponseBase<List<string>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet]
        [Route("GetAllProvasName")]
        public async Task<ResponseBase<List<string>>> GetAllProvasName(string bancas = "", string materias = "", string assuntos = "", string tipos = "")
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();
                var result = await _service.GetProvas(bancas, materias, assuntos, tipos, user.Admin.Equals("1"));
                List<string> response = _mapper.Map<List<string>>(result);

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
                await _loggerService.AddException(ex);

                return new ResponseBase<List<string>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet]
        [Route("GetAllMaterias")]
        public async Task<ResponseBase<List<string>>> GetAllMaterias(string bancas = "", string provas = "", string assuntos = "", string tipos = "")
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();
                var result = await _service.GetMaterias(bancas, provas, assuntos, tipos, user.Admin.Equals("1"));
                List<string> response = _mapper.Map<List<string>>(result);

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
                await _loggerService.AddException(ex);

                return new ResponseBase<List<string>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet]
        [Route("GetAllAssuntos")]
        public async Task<ResponseBase<List<string>>> GetAllAssuntos(string bancas = "", string provas = "", string materias = "", string tipos = "")
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();
                var result = await _service.GetAssuntos(bancas, provas, materias, tipos, user.Admin.Equals("1"));
                List<string> response = _mapper.Map<List<string>>(result);

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
                await _loggerService.AddException(ex);

                return new ResponseBase<List<string>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet]
        [Route("GetAllTipos")]
        public async Task<ResponseBase<List<string>>> GetAllTipos(string bancas = "", string provas = "", string materias = "")
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();
                var result = await _service.GetTipos(bancas, provas, materias, user.Admin.Equals("1"));
                List<string> response = _mapper.Map<List<string>>(result);

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
                await _loggerService.AddException(ex);

                return new ResponseBase<List<string>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet]
        [Route("GetSimulados")]
        public async Task<ResponseBase<List<PorvasReturn>>> GetSimulados()
        {
            try
            {
                var result = await _service.GetSimulados();
                List<PorvasReturn> response = _mapper.Map<List<PorvasReturn>>(
                result.Select(r => 
                new PorvasReturn() 
                { 
                    Codigo = r.Codigo, 
                    Nome = r.NomeProva 
                } 
                )).Distinct().OrderBy(t => t.Nome).ToList();

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
                await _loggerService.AddException(ex);

                return new ResponseBase<List<PorvasReturn>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet("downloadProva")]
        public async Task<ResponseBase<string>> GetProvaFile(int codigo)
        {
            try
            {
                var prova = await _service.CriaDocumentoProva(codigo);

                if (prova == null)
                {
                    return new ResponseBase<string>()
                    {
                        Message = "Simulado não encontrado",
                        Success = false
                    };
                }

                return new ResponseBase<string>()
                {
                    Message = "Listed",
                    Success = true,
                    Object = prova,
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

        [HttpGet("downloadGabarito")]
        public async Task<ResponseBase<string>> GetGabaritoFile(int codigo)
        {
            try
            {
                var prova = await _service.CriaDocumentoGabarito(codigo);

                if (prova == null)
                {
                    return new ResponseBase<string>()
                    {
                        Message = "Gabarito não encontrado",
                        Success = false
                    };
                }

                return new ResponseBase<string>()
                {
                    Message = "Listed",
                    Success = true,
                    Object = prova,
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

        [HttpPut("updateStatus")]
        public async Task<ResponseBase<bool>> UpdateStatus(int id, bool active)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                if (user.Admin != "1")
                {
                    return new ResponseBase<bool>()
                    {
                        Message = "Acesso não autorizado",
                        Success = false
                    };
                }

                var result = await _service.UpdateStatus(id, active);
                await _loggerService.AddInfo($"Atualizando status da prova {id} para {active} pelo usuário {user.Id}-{user.Nome}");

                return new ResponseBase<bool>()
                {
                    Message = "Updated",
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
