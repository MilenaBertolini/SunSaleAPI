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
        public async Task<ResponseBase<List<MainViewModel>>> GetAllPagged(int page, int quantity, bool anexos, string? subject, string? bancas, string? provas, string? materias, int? codigoProva)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                var result = await _service.GetAllPagged(page, quantity, user.Id, anexos, subject, bancas, provas, materias, codigoProva);
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

        //[HttpGet("criaQuestao")]
        //public async Task<ResponseBase<List<MainViewModel>>> CriaQuestao()
        //{
        //    try
        //    {
        //        var user = await _utils.GetUserFromContextAsync();

        //        if (user.Admin != "1")
        //        {
        //            return new ResponseBase<List<MainViewModel>>()
        //            {
        //                Message = "Acesso não autorizado",
        //                Success = false
        //            };
        //        }

        //        List<MainViewModel> list = new List<MainViewModel>();
        //        List<string> linhas = new List<string>();
        //        string materia = "HISTÓRIA";
        //        linhas.Add("1. Qual era caracterizada pelo surgimento dos primeiros seres humanos e ausência de registros escritos?");
        //        linhas.Add("   - [ ] A) Antiguidade");
        //        linhas.Add("   - [ ] B) Idade Média");
        //        linhas.Add("   - [ ] C) Pré-história");
        //        linhas.Add("   - [ ] D) Idade Contemporânea");
        //        linhas.Add("   - [x] E) Nenhuma das alternativas anteriores");
        //        linhas.Add("2. Quais civilizações são incluídas na Antiguidade?");
        //        linhas.Add("   - [ ] A) Egípcia e Grega");
        //        linhas.Add("   - [x] B) Grega e Romana");
        //        linhas.Add("   - [ ] C) Romana e Viking");
        //        linhas.Add("   - [ ] D) Egípcia e Mesopotâmica");
        //        linhas.Add("   - [ ] E) Persa e Suméria");
        //        linhas.Add("3. Qual período é marcado pela ascensão do cristianismo e desenvolvimento do feudalismo?");
        //        linhas.Add("   - [ ] A) Pré-história");
        //        linhas.Add("   - [x] B) Idade Média");
        //        linhas.Add("   - [ ] C) Antiguidade");
        //        linhas.Add("   - [ ] D) Idade Moderna");
        //        linhas.Add("   - [ ] E) Idade Contemporânea");
        //        linhas.Add("4. Qual movimento artístico teve como características a perfeição estética e harmonia?");
        //        linhas.Add("   - [ ] A) Barroco");
        //        linhas.Add("   - [ ] B) Impressionismo");
        //        linhas.Add("   - [x] C) Renascimento");
        //        linhas.Add("   - [ ] D) Realismo");
        //        linhas.Add("   - [ ] E) Cubismo");
        //        linhas.Add("5. Qual período viu o surgimento da ópera e a disseminação da imprensa?");
        //        linhas.Add("   - [ ] A) Pré-história");
        //        linhas.Add("   - [ ] B) Antiguidade");
        //        linhas.Add("   - [ ] C) Idade Média");
        //        linhas.Add("   - [ ] D) Idade Moderna");
        //        linhas.Add("   - [x] E) Idade Contemporânea");
        //        linhas.Add("6. Qual movimento artístico é conhecido por sua ênfase no realismo e grandiosidade?");
        //        linhas.Add("   - [ ] A) Renascimento");
        //        linhas.Add("   - [x] B) Arte Romana");
        //        linhas.Add("   - [ ] C) Barroco");
        //        linhas.Add("   - [ ] D) Impressionismo");
        //        linhas.Add("   - [ ] E) Cubismo");
        //        linhas.Add("7. Onde foram encontradas as pinturas rupestres, principal forma de expressão artística na Pré-história?");
        //        linhas.Add("   - [ ] A) Grécia");
        //        linhas.Add("   - [ ] B) Egito");
        //        linhas.Add("   - [x] C) Cavernas");
        //        linhas.Add("   - [ ] D) Mesopotâmia");
        //        linhas.Add("   - [ ] E) Roma");
        //        linhas.Add("8. Quais eventos marcaram a Idade Contemporânea?");
        //        linhas.Add("   - [ ] A) Renascimento e Reforma Protestante");
        //        linhas.Add("   - [ ] B) Revoluções Francesa e Industrial");
        //        linhas.Add("   - [ ] C) Império Romano e expansão marítima europeia");
        //        linhas.Add("   - [x] D) Guerras Mundiais e Guerra Fria");
        //        linhas.Add("   - [ ] E) Iluminismo e Revolução Industrial");
        //        linhas.Add("9. Qual é o principal estilo arquitetônico associado à Idade Média?");
        //        linhas.Add("   - [ ] A) Neoclássico");
        //        linhas.Add("   - [x] B) Gótico");
        //        linhas.Add("   - [ ] C) Renascentista");
        //        linhas.Add("   - [ ] D) Barroco");
        //        linhas.Add("   - [ ] E) Românico");
        //        linhas.Add("10. Quais são algumas das formas de arte que experimentaram mudanças significativas na Idade Contemporânea?");
        //        linhas.Add("   - [x] A) Música, cinema, literatura");
        //        linhas.Add("   - [ ] B) Escultura, pintura, teatro");
        //        linhas.Add("   - [ ] C) Dança, arquitetura, fotografia");
        //        linhas.Add("   - [ ] D) Cerâmica, poesia, gravura");
        //        linhas.Add("   - [ ] E) Teatro, dança, arquitetura");


        //        for (int i = 0, cont = 1; i < linhas.Count(); i += 6, cont++)
        //        {
        //            MainViewModel view = new MainViewModel();
        //            view.Ativo = "1";
        //            view.CodigoProva = 155;
        //            view.NumeroQuestao = cont;
        //            view.Materia = materia;
        //            view.ObservacaoQuestao = string.Empty;
        //            view.RespostasQuestoes = new List<RespostasQuestoesViewModel>();
        //            view.CampoQuestao = linhas[i];
        //            view.DataRegistro = DateTime.Now;
        //            view.UpdatedOn = DateTime.Now;

        //            for (int j = i + 1; j <= i + 5; j++)
        //            {
        //                RespostasQuestoesViewModel resposta = new RespostasQuestoesViewModel();
        //                resposta.Certa = linhas[j].Contains("   - [x] ") ? "1" : "0";
        //                resposta.TextoResposta = linhas[j].Replace("   - [x] ", "").Replace("   - [ ] ", "").Trim();
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
