using Application.Interface.Services;
using AutoMapper;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MainViewModel = Domain.ViewModel.UsuariosViewModel;
using MainEntity = Domain.Entities.Usuarios;
using Service = Application.Interface.Services.IUsuariosService;
using ServiceValidacao = Application.Interface.Services.IVerificacaoUsuarioService;
using RespostasService = Application.Interface.Services.IRespostasUsuariosService;
using EmailService = Application.Interface.Services.IEmailService;
using APISunSale.Utils;
using Domain.ViewModel;
using Domain.Entities;
using LoggerService = Application.Interface.Services.ILoggerService;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UsuariosController
    {
        private readonly ILogger<UsuariosController> _logger;
        private readonly Service _service;
        private readonly ServiceValidacao _serviceValidacao;
        private readonly IMapper _mapper;
        private readonly MainUtils _utils;
        private readonly RespostasService _respostasService;
        private readonly EmailService _emailService;
        private readonly LoggerService _loggerService;

        public UsuariosController(ILogger<UsuariosController> logger, Service service, IMapper mapper, IHttpContextAccessor httpContextAccessor, RespostasService respostasService, EmailService emailService, LoggerService loggerService, ServiceValidacao serviceValidacao)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _utils = new MainUtils(httpContextAccessor, service);
            _respostasService = respostasService;
            _emailService = emailService;
            _loggerService = loggerService;
            _serviceValidacao = serviceValidacao;
        }

        [HttpGet]
        [Authorize]
        public async Task<ResponseBase<List<MainViewModel>>> GetAll()
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();
                if (!user.Admin.Equals("1"))
                {
                    return new ResponseBase<List<MainViewModel>>()
                    {
                        Message = "Just for admin",
                        Success = false
                    };
                }

                var result = await _service.GetAll();
                result = result.Select(u => new MainEntity()
                {
                    Id = u.Id,
                    Nome = u.Nome,
                });
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
        [Authorize]
        public async Task<ResponseBase<List<MainViewModel>>> GetAllPagged(int page, int quantity)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();
                if (!user.Admin.Equals("1"))
                {
                    return new ResponseBase<List<MainViewModel>>()
                    {
                        Message = "Just for admin",
                        Success = false
                    };
                }

                var result = await _service.GetAllPagged(page, quantity);
                var quantidade = await _service.QuantidadeTotal();
                var response = _mapper.Map<List<MainViewModel>>(result);
                return new ResponseBase<List<MainViewModel>>()
                {
                    Message = "List created",
                    Success = true,
                    Object = response,
                    Quantity = response?.Count,
                    Total = quantidade
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
        [Authorize]
        public async Task<ResponseBase<MainViewModel>> GetById(int id)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();
                if (!user.Admin.Equals("1"))
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Just for admin",
                        Success = false
                    };
                }

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
        [AllowAnonymous]
        public async Task<ResponseBase<MainViewModel>> Add([FromBodyAttribute] MainViewModel main)
        {
            try
            {
                if (await _service.ExistsEmail(main.Email, true))
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Already exists a user with this email",
                        Success = false
                    };
                }

                if (await _service.ExistsLogin(main.Login, true))
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Already exists a user with this login",
                        Success = false
                    };
                }
                main.Admin = "0";

                var result = await _service.Add(_mapper.Map<MainEntity>(main));

                var validacao = await _serviceValidacao.Add(new VerificacaoUsuario()
                {
                    CodigoUsuario = result.Id
                });

                var email = new EmailViewModel()
                {
                    Assunto = "Bem vindo ao QuestoesAqui",
                    Destinatario = main.Email,
                    Status = "0",
                    Texto = Utils.CrieEmail.CriaEmailBoasVindas(result, validacao.GuidText)
                };

                await _emailService.Add(_mapper.Map<Email>(email));

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
        [Authorize]
        public async Task<ResponseBase<MainViewModel>> Update([FromBodyAttribute] MainViewModel main)
        {
            try
            {
                if(await _service.GetById(main.Id) == null)
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "User not found",
                        Success = false
                    };
                }

                var result = await _service.Update(_mapper.Map<MainEntity>(main));
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

        [HttpPut("updateName")]
        [Authorize]
        public async Task<ResponseBase<MainViewModel>> UpdateName(string name)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                var main = await _service.GetById(user.Id);
                main.Nome = name;

                var result = await _service.Update(_mapper.Map<MainEntity>(main));
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

        [HttpPut("updateSenha")]
        [Authorize]
        public async Task<ResponseBase<MainViewModel>> UpdatePass(string oldPass, string newPass)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                var main = await _service.GetById(user.Id);

                if(main.Pass != oldPass)
                {
                    return new ResponseBase<MainViewModel>()
                    {
                        Message = "Senha incorreta",
                        Success = false,
                    };
                }

                main.Pass = newPass;

                var result = await _service.Update(_mapper.Map<MainEntity>(main));

                var email = new EmailViewModel()
                {
                    Assunto = "Alteração de senha",
                    Destinatario = main.Email,
                    Status = "0",
                    Texto = Utils.CrieEmail.ConfirmaAlteracaoPass(result.Nome, Data.Helper.EnumeratorsTypes.TipoSistema.QuestoesAqui)
                };
                await _emailService.Add(_mapper.Map<Email>(email));

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
        [Authorize]
        public async Task<ResponseBase<bool>> Delete(int id)
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();
                if (!user.Admin.Equals("1"))
                {
                    return new ResponseBase<bool>()
                    {
                        Message = "Just for admin",
                        Success = false,
                        Object = false
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

        [HttpGet("getPerfil")]
        public async Task<ResponseBase<PerfilUsuario>> GetPerfilUsuario()
        {
            try
            {
                var user = await _utils.GetUserFromContextAsync();

                var result = await _service.GetPerfil(user.Id);
                result.QuantidadeQuestoesAcertadas = await _respostasService.GetQuantidadeQuestoesCertas(user.Id);
                result.QuantidadeQuestoesResolvidas = await _respostasService.GetQuantidadeQuestoesTentadas(user.Id);
                result.Usuario.Pass = string.Empty;

                return new ResponseBase<PerfilUsuario>()
                {
                    Message = "List created",
                    Success = true,
                    Object = result,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                await _loggerService.AddException(ex);

                return new ResponseBase<PerfilUsuario>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [AllowAnonymous]
        [HttpGet("reenviaEmailConfirmacao")]
        [Authorize]
        public async Task<ResponseBase<Object>> ReenviaEmailConfirmacao(string mail)
        {
            try
            {
                var user = await _service.GetByEmail(mail);

                if(user.IsVerified == "1")
                {
                    return new ResponseBase<Object>()
                    {
                        Message = "Usuário já autenticado!",
                        Success = false
                    };
                }

                var verificacao = await _serviceValidacao.GetByCodigoUsuario(user.Id);

                var email = new EmailViewModel()
                {
                    Assunto = "Bem vindo ao QuestoesAqui",
                    Destinatario = user.Email,
                    Status = "0",
                    Texto = Utils.CrieEmail.CriaEmailBoasVindas(user, verificacao.GuidText)
                };

                await _emailService.Add(_mapper.Map<Email>(email));

                return new ResponseBase<Object>()
                {
                    Message = "Validated",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                await _loggerService.AddException(ex);

                return new ResponseBase<Object>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [AllowAnonymous]
        [HttpGet("liberauser")]
        [Authorize]
        public async Task<ResponseBase<Object>> LiberaUsuario(string guid)
        {
            try
            {
                var verificacao = await _serviceValidacao.GetByGuid(guid);

                if(verificacao == null || verificacao?.IsActive != "1")
                {
                    return new ResponseBase<Object>()
                    {
                        Message = "Not found",
                        Success = false
                    };
                }

                var usuario = await _service.GetById(verificacao.CodigoUsuario);

                usuario.IsVerified = "1";

                await _service.Update(usuario);

                verificacao.IsActive = "0";
                await _serviceValidacao.Update(verificacao);

                var email = new EmailViewModel()
                {
                    Assunto = "Conta validada com sucesso",
                    Destinatario = usuario.Email,
                    Status = "0",
                    Texto = Utils.CrieEmail.CriaEmailUsuarioValidado(usuario)
                };
                await _emailService.Add(_mapper.Map<Email>(email));

                return new ResponseBase<Object>()
                {
                    Message = "Validated",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                await _loggerService.AddException(ex);

                return new ResponseBase<Object>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
