using AutoMapper;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MainViewModel = Domain.ViewModel.QuestoesViewModel;
using MainEntity = Domain.Entities.Questoes;
using Service = Application.Interface.Services.IQuestoesService;
using UserService = Application.Interface.Services.IUsuariosService;
using RespostasUserService = Application.Interface.Services.IRespostasUsuariosService;
using APISunSale.Utils;
using Domain.ViewModel;
using static Data.Helper.EnumeratorsTypes;
using LoggerService = Application.Interface.Services.ILoggerService;
using System.Data;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Application.Model;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuestoesController
    {
        private readonly ILogger<QuestoesController> _logger;
        private readonly Service _service;
        private readonly IMapper _mapper;
        private readonly MainUtils _utils;
        private readonly RespostasUserService _respostasUserService;
        private readonly LoggerService _loggerService;

        public QuestoesController(ILogger<QuestoesController> logger, Service service, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserService userService, RespostasUserService respostasUserService, LoggerService loggerService)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _utils = new MainUtils(httpContextAccessor, userService);
            _respostasUserService = respostasUserService;
            _loggerService = loggerService;
        }

        [HttpGet("pagged")]
        public async Task<ResponseBase<List<MainViewModel>>> GetAllPagged(int page, int quantity, bool anexos, string? assuntos, string? bancas, string? provas, string? materias, int? codigoProva)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                var result = await _service.GetAllPagged(page, quantity, user.Id, anexos, assuntos, bancas, provas, materias, codigoProva);
                var response = _mapper.Map<List<MainViewModel>>(result.Item1);

                var temp = _mapper.Map<IList<RespostasUsuariosViewModel>>(await _respostasUserService.GetByUserQuestao(user.Id));
                foreach(var item in response)
                {
                    item.RespostasUsuarios = temp.Where(r => item.RespostasQuestoes.Where(re => re.Codigo.Equals(r.CodigoResposta)).Count() > 0).ToList();
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
                var user = await _utils.GetUserFromContextAsync();

                var result = await _service.GetById(id);

                if(result == null)
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Not registered",
                        Success = false
                    };
                }

                var response = _mapper.Map<MainViewModel>(result);

                var temp = await _respostasUserService.GetByUserQuestao(user.Id, response.Codigo);
                response.RespostasUsuarios = _mapper.Map<IList<RespostasUsuariosViewModel>>(temp);

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

        [HttpGet("getQuestao")]
        public async Task<ResponseBase<MainViewModel>> GetQuestao(int codigoProva, int? numeroQuestao)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                int questao = numeroQuestao.HasValue ? numeroQuestao.Value : 0;

                var result = await _service.GetQuestoesByProva(codigoProva, questao);

                if (result == null)
                {
                    var last = await _service.GetLastByProva(codigoProva);
                    while (last.NumeroQuestao > questao && result == null)
                    {
                        result = await _service.GetQuestoesByProva(codigoProva, questao + 1);
                    }

                    if(result == null)
                    {
                        return new ResponseBase<MainViewModel>()
                        {
                            Message = "Not registered",
                            Success = false
                        };
                    }
                }

                var response = _mapper.Map<MainViewModel>(result);
                var temp = await _respostasUserService.GetByUserQuestao(user.Id, response.Codigo);
                response.RespostasUsuarios = _mapper.Map<IList<RespostasUsuariosViewModel>>(temp);

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
        public async Task<ResponseBase<MainViewModel>> Add([FromBodyAttribute] MainViewModel main)
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

                var result = await _service.Add(_mapper.Map<MainEntity>(main), user.Id);
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
                        Message = "Questão doesn't exists",
                        Success = false
                    };
                }

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
        [Route("GetAllMaterias")]
        public async Task<ResponseBase<List<string>>> GetAllMaterias()
        {
            try
            {
                var result = await _service.GetAllMateris();

                return new ResponseBase<List<string>>()
                {
                    Message = "List created",
                    Success = true,
                    Object = result?.ToList(),
                    Quantity = result?.Count()
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

        [HttpGet("getQuestaoAleatoria")]
        public async Task<ResponseBase<MainViewModel>> GetQuestaoAleatoria(TipoQuestoes tipo, string? subject, string? bancas)
        {
            try
            {
                var result = await _service.GetQuestoesAleatoria(tipo, subject, bancas);

                if (result == null)
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Not registered",
                        Success = false
                    };
                }

                var user = await _utils.GetUserFromContextAsync();
                var response = _mapper.Map<MainViewModel>(result);
                var temp = await _respostasUserService.GetByUserQuestao(user.Id, response.Codigo);
                response.RespostasUsuarios = _mapper.Map<IList<RespostasUsuariosViewModel>>(temp);

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

        [HttpGet("solicitaVerificacao")]
        public async Task<ResponseBase<MainViewModel>> SolicitaVerificacao(int id)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                var result = await _service.UpdateAtivo(id, false, user.Id);

                var response = _mapper.Map<MainViewModel>(result);

                var temp = await _respostasUserService.GetByUserQuestao(user.Id, response.Codigo);
                response.RespostasUsuarios = _mapper.Map<IList<RespostasUsuariosViewModel>>(temp);

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

        [HttpGet("revisar")]
        public async Task<ResponseBase<MainViewModel>> Revisar(int id)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                if (user.Admin != "1")
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "No access!",
                        Success = false
                    };
                }

                var result = await _service.UpdateAtivo(id, true, user.Id);

                await _loggerService.AddInfo($"Questão {id} revisada pelo usuário {user.Id}-{user.Nome}.");
                
                var response = _mapper.Map<MainViewModel>(result);
                var temp = await _respostasUserService.GetByUserQuestao(user.Id, response.Codigo);
                response.RespostasUsuarios = _mapper.Map<IList<RespostasUsuariosViewModel>>(temp);

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

        [HttpGet("getByAvaliacao")]
        public async Task<ResponseBase<MainViewModel>> GetByAvaliacao(int avaliacao, int? numeroQuestao)
        {
            try
            {
                var result = await _service.GetQuestoesByAvaliacao(avaliacao, numeroQuestao);

                if (result == null)
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Not registered",
                        Success = false
                    };
                }

                var user = await _utils.GetUserFromContextAsync();
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

        [HttpPut("updateAssunto")]
        public async Task<ResponseBase<MainViewModel>> UpdateAssunto(UpdateAssuntoQuestao assuntoQuestao)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                if (user.Admin != "1" && user.Admin != "2")
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "No access!",
                        Success = false
                    };
                }

                var result = await _service.UpdateAssunto(assuntoQuestao.CodigoQuestao, assuntoQuestao.Assunto, user.Id);

                await _loggerService.AddInfo($"Assunto da questão {assuntoQuestao.CodigoQuestao} alterado pelo usuário {user.Id}-{user.Nome}.");

                var response = _mapper.Map<MainViewModel>(result);
                var temp = await _respostasUserService.GetByUserQuestao(user.Id, response.Codigo);
                response.RespostasUsuarios = _mapper.Map<IList<RespostasUsuariosViewModel>>(temp);

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

        [HttpGet("criaQuestao")]
        public async Task<ResponseBase<List<MainViewModel>>> CriaQuestao()
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                if (user.Admin != "1")
                {
                    return new ResponseBase<List<MainViewModel>>()
                    {
                        Message = "Acesso não autorizado",
                        Success = false
                    };
                }

                List<MainViewModel> list = new List<MainViewModel>();
                List<string> linhas = new List<string>();
                string materia = "BIOLOGIA";
                string assunto = "Introdução à Biologioa";

                linhas.Add("O que é a principal função da membrana celular?");
                linhas.Add("a) Armazenar energia");
                linhas.Add("Xb) Regular a entrada e saída de substâncias na célula");
                linhas.Add("c) Produzir proteínas");
                linhas.Add("d) Realizar a fotossíntese");
                linhas.Add("e) Transportar oxigênio");
                linhas.Add("Qual é a função do retículo endoplasmático rugoso?");
                linhas.Add("a) Síntese de lipídios");
                linhas.Add("b) Produção de ATP");
                linhas.Add("c) Armazenamento de água");
                linhas.Add("Xd) Síntese de proteínas");
                linhas.Add("e) Digestão celular");
                linhas.Add("O que são os lisossomos?");
                linhas.Add("a) Organelas responsáveis pela fotossíntese");
                linhas.Add("b) Estruturas que produzem energia");
                linhas.Add("c) Vesículas que armazenam água");
                linhas.Add("Xd) Organelas digestivas");
                linhas.Add("e) Estruturas envolvidas na síntese de lipídios");
                linhas.Add("Qual é a principal função do núcleo da célula?");
                linhas.Add("   a) Produção de ATP");
                linhas.Add("   b) Armazenamento de glicose");
                linhas.Add("   Xc) Controle das atividades celulares e armazenamento do material genético");
                linhas.Add("   d) Síntese de proteínas");
                linhas.Add("   e) Transporte de substâncias");
                linhas.Add("O que é o complexo de Golgi?");
                linhas.Add("   a) Organela responsável pela fotossíntese");
                linhas.Add("   b) Estrutura envolvida na respiração celular");
                linhas.Add("   c) Organela que produz ATP");
                linhas.Add("   Xd) Responsável pela secreção celular e modificação de proteínas");
                linhas.Add("   e) Estrutura que armazena água");
                linhas.Add("Qual é a função das mitocôndrias?");
                linhas.Add("   a) Síntese de proteínas");
                linhas.Add("   Xb) Produção de ATP (energia)");
                linhas.Add("   c) Armazenamento de água");
                linhas.Add("   d) Digestão celular");
                linhas.Add("   e) Respiração celular");
                linhas.Add("O que são os ribossomos?");
                linhas.Add("   a) Vesículas que armazenam enzimas");
                linhas.Add("   b) Estruturas responsáveis pela fotossíntese");
                linhas.Add("   Xc) Organelas responsáveis pela síntese de proteínas");
                linhas.Add("   d) Componentes do citoesqueleto");
                linhas.Add("   e) Estruturas que armazenam glicose");
                linhas.Add("Qual é a principal função do citoesqueleto?");
                linhas.Add("   a) Produção de energia");
                linhas.Add("   b) Armazenamento de substâncias");
                linhas.Add("   Xc) Sustentação e movimentação celular");
                linhas.Add("   d) Digestão de nutrientes");
                linhas.Add("   e) Secreção celular");
                linhas.Add("O que é o vacúolo?");
                linhas.Add("   Xa) Estrutura que armazena água, íons e nutrientes");
                linhas.Add("   b) Organela responsável pela respiração celular");
                linhas.Add("   c) Vesícula que produz ATP");
                linhas.Add("   d) Estrutura envolvida na fotossíntese");
                linhas.Add("   e) Componente do citoesqueleto");
                linhas.Add("Qual é a função dos centríolos?");
                linhas.Add("    a) Síntese de proteínas");
                linhas.Add("    b) Produção de ATP");
                linhas.Add("    Xc) Divisão celular e formação do fuso mitótico");
                linhas.Add("    d) Armazenamento de água");
                linhas.Add("    e) Respiração celular");

                for (int i = 0, cont = 1; i < linhas.Count(); i += 6, cont++)
                {
                    MainViewModel view = new MainViewModel();
                    view.Ativo = "1";
                    view.CodigoProva = 157;
                    view.NumeroQuestao = cont + 10;
                    view.Materia = materia;
                    view.Assunto = assunto;
                    view.ObservacaoQuestao = string.Empty;
                    view.RespostasQuestoes = new List<RespostasQuestoesViewModel>();
                    view.CampoQuestao = $"<b>Questão {view.NumeroQuestao}</b><br><br>{linhas[i]}";
                    view.DataRegistro = DateTime.Now;
                    view.UpdatedOn = DateTime.Now;

                    for (int j = i + 1; j <= i + 5; j++)
                    {
                        RespostasQuestoesViewModel resposta = new RespostasQuestoesViewModel();
                        resposta.Certa = linhas[j].Contains("X") ? "1" : "0";
                        resposta.TextoResposta = linhas[j].Replace("X", "").Trim();
                        resposta.DataRegistro = DateTime.Now;

                        view.RespostasQuestoes.Add(resposta);
                    }

                    await _service.Add(_mapper.Map<MainEntity>(view), user.Id);

                    list.Add(view);
                }

                return new ResponseBase<List<MainViewModel>>()
                {
                    Message = "Search success",
                    Success = true,
                    Object = list,
                    Quantity = 1
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

        //[HttpGet("criaQuestao")]
        //public async Task<ResponseBase<MainViewModel>> criaQuestao()
        //{
        //    try
        //    {
        //        string[] texto = File.ReadAllLines("C:/provas/saida/json.json");
        //        var mainList = JsonConvert.DeserializeObject<List<MainViewModel>>(texto[0]);

        //        var user = await _utils.GetUserFromContextAsync();

        //        if (user.Admin != "1")
        //        {
        //            return new ResponseBase<MainViewModel>()
        //            {
        //                Message = "Acesso não autorizado",
        //                Success = false
        //            };
        //        }

        //        foreach (var main in mainList)
        //        {
        //            var result = await _service.Add(_mapper.Map<MainEntity>(main), user.Id);
        //        }
        //        return new ResponseBase<MainViewModel>()
        //        {
        //            Message = "Created",
        //            Success = true,
        //            Quantity = mainList.Count
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
        //        await _loggerService.AddException(ex);

        //        return new ResponseBase<MainViewModel>()
        //        {
        //            Message = ex.Message,
        //            Success = false
        //        };
        //    }
        //}

        /*

        [HttpGet("criaQuestao")]
        public async Task<ResponseBase<List<MainViewModel>>> CriaQuestao()
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();
                List<MainViewModel> list = new List<MainViewModel>();

                Random randon = new Random();

                for (int i = 3; i <= 50; i++)
                {
                    MainViewModel view = new MainViewModel();
                    view.Ativo = "1";
                    view.CodigoProva = 72;
                    view.NumeroQuestao = i.ToString();
                    view.Materia = "Matemática";
                    view.ObservacaoQuestao = string.Empty;
                    view.RespostasQuestoes = new List<RespostasQuestoesViewModel>();

                    int a = randon.Next(5);

                    while (a == 0) a = randon.Next(5);

                    int b = randon.Next(10);

                    while (b == 0) b = randon.Next(10);

                    int x = randon.Next(10);

                    int resultado = i % 2 == 0 ? a * x + b : a * x - b;
                    view.CampoQuestao = $"<b>Questão {i.ToString("00")}</b><br><br>Qual o resultado da equação <b>{(a > 1 ? a + "" : " ")}x {(i % 2 == 0 ? '+' : '-')} {b} = {resultado}</b>?";

                    int op = randon.Next(5);
                    while (op == 0) op = randon.Next(5);

                    for (int j = 1; j <= 5; j++)
                    {
                        RespostasQuestoesViewModel resposta = new RespostasQuestoesViewModel();
                        if (j == op)
                        {
                            resposta.Certa = "1";
                            resposta.TextoResposta = x.ToString();
                        }
                        else
                        {
                            resposta.Certa = "0";
                            resposta.TextoResposta = (x + j).ToString();
                        }
                        view.RespostasQuestoes.Add(resposta);
                    }

                    await _service.Add(_mapper.Map<MainEntity>(view), user.Id);

                    list.Add(view);
                }

                return new ResponseBase<List<MainViewModel>>()
                {
                    Message = "Search success",
                    Success = true,
                    Object = list,
                    Quantity = 1
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
        }*/
    }
}
