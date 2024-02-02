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

        //[HttpGet("criaQuestao")]
        //public async Task<ResponseBase<List<MainViewModel>>> CriaQuestao()
        //{
        //    try
        //    {
        //        var user = await _utils.GetUserFromContextAsync();

        //        if(user.Admin != "1")
        //        {
        //            return new ResponseBase<List<MainViewModel>>()
        //            {
        //                Message = "Acesso não autorizado",
        //                Success = false
        //            };
        //        }

        //        List<MainViewModel> list = new List<MainViewModel>();
        //        List<string> linhas = new List<string>();
        //        string materia = "DIREITO PREVIDENCIÁRIO";
        //        linhas.Add("100- Analise as proposições abaixo ( de I a V ) e assinale a alternativa correta, conforme sejam verdadeiras ou falsas:<br><br>I- a contribuição do empregador rural pessoa física, destinada à Seguridade Social, é de 2% da receita bruta proveniente da comercialização da sua produção e de 0,1% da receita bruta proveniente da comercialização da sua produção para financiamento das prestações por acidente do trabalho, havendo, também, para esta pessoa física, a contribuição facultativa do segurado contribuinte individual calculada sobre o salário-de)contribuição.<br>II- a Seguridade Social será financiada somente pelos seus segurados e pelas empresas.<br>III- o empregador doméstico poderá recolher a contribuição do segurado empregado a seu serviço e a parcela a seu cargo relativas à competência novembro até o dia 20 de dezembro, juntamente com a contribuição referente ao 13o salário, utilizando-se de um único documento de arrecadação.<br>IV- o direito de cobrar os créditos da Seguridade Social, constituídos na forma de sua Lei Orgânica, prescreve em 10 anos.<br>V- para ficar isenta das contribuições previdenciárias da empresa, é suficiente que a entidade beneficente de assistência social seja reconhecida como de utilidade pública federal e estadual ou do Distrito Federal ou municipal. ");
        //        linhas.Add("a) Apenas as proposições I e III são verdadeiras.");
        //        linhas.Add("b) Apenas as proposições I, II e V são verdadeiras.");
        //        linhas.Add("c) Apenas as proposições III e IV são verdadeiras. -X");
        //        linhas.Add("d) Apenas as proposições I, III e IV são verdadeiras.");
        //        linhas.Add("e) Todas as proposições são verdadeiras.");


        //        for (int i = 0, cont = 1; i < linhas.Count(); i+=6, cont++)
        //        {
        //            MainViewModel view = new MainViewModel();
        //            view.Ativo = "1";
        //            view.CodigoProva = 120;
        //            view.NumeroQuestao = cont;
        //            view.Materia = materia;
        //            view.ObservacaoQuestao = string.Empty;
        //            view.RespostasQuestoes = new List<RespostasQuestoesViewModel>();
        //            view.CampoQuestao = linhas[i];
        //            view.DataRegistro = DateTime.Now;
        //            view.UpdatedOn = DateTime.Now;

        //            for (int j = i+1; j <= i+5; j++)
        //            {
        //                RespostasQuestoesViewModel resposta = new RespostasQuestoesViewModel();
        //                resposta.Certa = linhas[j].Contains(" -X") ? "1" : "0";
        //                resposta.TextoResposta = linhas[j].Replace(" -X", "").Trim();
        //                resposta.DataRegistro = DateTime.Now;

        //                view.RespostasQuestoes.Add(resposta);
        //            }

        //            await _service.Add(_mapper.Map<MainEntity>(view), user.Id);

        //            list.Add(view);
        //        }

        //        return new ResponseBase<List<MainViewModel>>()
        //        {
        //            Message = "Search success",
        //            Success = true,
        //            Object = list,
        //            Quantity = 1
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
        //        await _loggerService.AddException(ex);

        //        return new ResponseBase<List<MainViewModel>>()
        //        {
        //            Message = ex.Message,
        //            Success = false
        //        };
        //    }
        //}

        [HttpGet("criaQuestao")]
        public async Task<ResponseBase<MainViewModel>> criaQuestao()
        {
            try
            {
                string[] texto = File.ReadAllLines("C:/provas/saida/json.json");
                var mainList = JsonConvert.DeserializeObject<List<MainViewModel>>(texto[0]);
                
                var user = await _utils.GetUserFromContextAsync();

                if (user.Admin != "1")
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Acesso não autorizado",
                        Success = false
                    };
                }

                foreach (var main in mainList)
                {
                    var result = await _service.Add(_mapper.Map<MainEntity>(main), user.Id);
                }
                return new ResponseBase<MainViewModel>()
                {
                    Message = "Created",
                    Success = true,
                    Quantity = mainList.Count
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
