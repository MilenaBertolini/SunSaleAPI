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
        public async Task<ResponseBase<List<MainViewModel>>> GetAllPagged(int page, int quantity, bool anexos, int? codigoProva, string? subject)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                var result = await _service.GetAllPagged(page, quantity, user.Id, anexos, codigoProva, subject);
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

        [HttpPost]
        public async Task<ResponseBase<MainViewModel>> Add([FromBodyAttribute] MainViewModel main)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

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
                if(await _service.GetById(main.Codigo) == null)
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Questão doesn't exists",
                        Success = false
                    };
                }

                var user = await _utils.GetUserFromContextAsync();
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

                var result = await _service.GetById(id);

                if (result == null)
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Not registered",
                        Success = false
                    };
                }

                result.Ativo = "0";
                await _service.Update(result, user.Id);

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
        /*

        [HttpGet("criaQuestao")]
        public async Task<ResponseBase<List<MainViewModel>>> CriaQuestao()
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();
                List<MainViewModel> list = new List<MainViewModel>();
                List<string> linhas = new List<string>();
                linhas.Add("<b>Questão {NUMQ}</b><br><br>Qual é a unidade básica da vida?");
                linhas.Add("a) Átomo.");
                linhas.Add("X) b) Célula.");
                linhas.Add("c) Molécula.");
                linhas.Add("d) Organismo.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>Qual é a função do sistema respiratório?");
                linhas.Add("a) Realizar a digestão dos alimentos.");
                linhas.Add("X) b) Permitir a troca gasosa entre o organismo e o ambiente.");
                linhas.Add("c) Transportar oxigênio e nutrientes pelo corpo.");
                linhas.Add("d) Controlar o funcionamento do corpo.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>O que é uma reação química?");
                linhas.Add("a) A transformação de um sólido em líquido.");
                linhas.Add("X) b) A transformação de substâncias em outras substâncias diferentes.");
                linhas.Add("c) A produção de luz e calor.");
                linhas.Add("d) A formação de moléculas complexas.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>Qual é a camada mais externa da Terra?");
                linhas.Add("a) Núcleo interno.");
                linhas.Add("b) Núcleo externo.");
                linhas.Add("c) Manto.");
                linhas.Add("X) d) Crosta.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>O que é um ecossistema?");
                linhas.Add("a) A cadeia alimentar de um organismo.");
                linhas.Add("X) b) Um conjunto de seres vivos e o ambiente em que vivem.");
                linhas.Add("c) A transformação de energia em matéria.");
                linhas.Add("d) A reprodução de organismos.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>O que é a fotossíntese?");
                linhas.Add("X) a) Processo em que as plantas convertem luz solar em energia química.");
                linhas.Add("b) Processo de respiração das plantas.");
                linhas.Add("c) Processo de digestão das plantas.");
                linhas.Add("d) Processo de reprodução das plantas.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>Qual é a principal função do sistema nervoso?");
                linhas.Add("a) Transportar oxigênio e nutrientes pelo corpo.");
                linhas.Add("b) Realizar a respiração celular.");
                linhas.Add("X) c) Controlar as funções do corpo e a interação com o ambiente.");
                linhas.Add("d) Produzir hormônios.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>O que é a genética?");
                linhas.Add("a) O estudo dos animais.");
                linhas.Add("X) b) O estudo dos genes e da hereditariedade.");
                linhas.Add("c) O estudo das rochas e minerais.");
                linhas.Add("d) O estudo das reações químicas.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>Qual é a função do sistema circulatório?");
                linhas.Add("a) Realizar a fotossíntese.");
                linhas.Add("b) Controlar as funções do corpo.");
                linhas.Add("c) Permitir a troca gasosa.");
                linhas.Add("X) d) Transportar oxigênio e nutrientes pelo corpo.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>O que são células-tronco?");
                linhas.Add("a) Células especializadas para a reprodução.");
                linhas.Add("b) Células que formam os tecidos.");
                linhas.Add("c) Células que realizam a fotossíntese.");
                linhas.Add("X) d) Células com capacidade de se diferenciar em diferentes tipos de células.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>O que é a evolução biológica?");
                linhas.Add("X) a) O processo pelo qual os seres vivos mudam ao longo do tempo.");
                linhas.Add("b) O processo de reprodução dos seres vivos.");
                linhas.Add("c) O processo de formação das células.");
                linhas.Add("d) O processo de interação entre os seres vivos.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>O que é um ácido?");
                linhas.Add("a) Uma substância com pH neutro.");
                linhas.Add("X) b) Uma substância que libera íonsH+ quando dissolvida em água.");
                linhas.Add("c) Uma substância que libera íons OH- quando dissolvida em água.");
                linhas.Add("d) Uma substância que não reage com outras substâncias.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>Qual é o órgão responsável pela produção de insulina no corpo humano?");
                linhas.Add("a) Pâncreas.");
                linhas.Add("b) Fígado.");
                linhas.Add("X) c) Glândula adrenal.");
                linhas.Add("d) Tireoide.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>O que é a gravidade?");
                linhas.Add("a) A força que faz os objetos flutuarem.");
                linhas.Add("X) b) A força que atrai os objetos em direção ao centro da Terra.");
                linhas.Add("c) A força que mantém os objetos em movimento.");
                linhas.Add("d) A força que repele os objetos uns dos outros.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>O que é uma cadeia alimentar?");
                linhas.Add("a) O ciclo de vida de um organismo.");
                linhas.Add("X) b) A sequência de transferência de energia entre os seres vivos em um ecossistema.");
                linhas.Add("c) A formação de novos indivíduos por reprodução.");
                linhas.Add("d) O processo de respiração das plantas.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>Qual é a principal função do sistema endócrino?");
                linhas.Add("a) Controlar as funções do corpo e a interação com o ambiente.");
                linhas.Add("b) Transportar oxigênio e nutrientes pelo corpo.");
                linhas.Add("c) Realizar a troca gasosa.");
                linhas.Add("X) d) Produzir e liberar hormônios no organismo.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>O que é a matéria?");
                linhas.Add("X) a) Tudo o que possui massa e ocupa espaço.");
                linhas.Add("b) Uma forma de energia.");
                linhas.Add("c) A capacidade de fazer trabalho.");
                linhas.Add("d) A quantidade de matéria em um objeto.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>O que é um vetor?");
                linhas.Add("a) Uma quantidade que possui apenas magnitude.");
                linhas.Add("X) b) Uma quantidade que possui magnitude e direção.");
                linhas.Add("c) Uma quantidade que possui apenas direção.");
                linhas.Add("d) Uma quantidade que não pode ser medida.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>O que é um eclipse lunar?");
                linhas.Add("a) Quando a Lua se alinha entre a Terra e o Sol.");
                linhas.Add("X) b) Quando a Terra se posiciona entre o Sol e a Lua, impedindo a luz solar de alcançar a Lua.");
                linhas.Add("c) Quando a Lua fica completamente iluminada pelo Sol.");
                linhas.Add("d) Quando a Lua fica totalmente escura.");
                linhas.Add("<b>Questão {NUMQ}</b><br><br>O que é um ecossistema aquático?");
                linhas.Add("a) Um ecossistema formado por seres vivos terrestres.");
                linhas.Add("b) Um ecossistema formado por seres vivos marinhos.");
                linhas.Add("X) c) Um ecossistema formado por seres vivos de água doce.");
                linhas.Add("d) Um ecossistema formado por seres vivos subterrâneos.");

                Random randon = new Random();

                for (int i = 0, cont = 1; i < linhas.Count(); i+=5, cont++)
                {
                    MainViewModel view = new MainViewModel();
                    view.Ativo = "1";
                    view.CodigoProva = 80;
                    view.NumeroQuestao = cont.ToString();
                    view.Materia = "Ciências";
                    view.ObservacaoQuestao = string.Empty;
                    view.RespostasQuestoes = new List<RespostasQuestoesViewModel>();
                    view.CampoQuestao = linhas[i].Replace("{NUMQ}", cont.ToString("00")).Replace("Questao", "Questão");
                    
                    for (int j = i+1; j < i+5; j++)
                    {
                        RespostasQuestoesViewModel resposta = new RespostasQuestoesViewModel();
                        resposta.Certa = linhas[j].Contains("X)") ? "1" : "0";
                        resposta.TextoResposta = linhas[j].Replace("X)", "").Trim();

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
