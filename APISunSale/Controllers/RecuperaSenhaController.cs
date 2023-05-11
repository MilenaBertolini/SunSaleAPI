using Application.Interface.Services;
using AutoMapper;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using MainViewModel = Domain.ViewModel.RecuperaSenhaViewModel;
using MainEntity = Domain.Entities.RecuperaSenha;
using Service = Application.Interface.Services.IRecuperaSenhaService;
using UserService = Application.Interface.Services.IUsuariosService;
using EmailService = Application.Interface.Services.IEmailService;
using Domain.ViewModel;
using Domain.Entities;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecuperaSenhaController
    {
        private readonly ILogger<RecuperaSenhaController> _logger;
        private readonly Service _service;
        private readonly UserService _userService;
        private readonly EmailService _emailService;
        private readonly IMapper _mapper;
        public RecuperaSenhaController(ILogger<RecuperaSenhaController> logger, Service service, IMapper mapper, UserService userService, EmailService emailService)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _emailService = emailService;
        }

        [HttpGet("pagged")]
        public async Task<ResponseBase<List<MainViewModel>>> GetAllPagged(int page, int quantity)
        {
            try
            {
                var result = await _service.GetAllPagged(page, quantity);
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
                    Quantity = response != null ? 1 : 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
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
                var result = await _service.Add(_mapper.Map<MainEntity>(main));
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
                return new ResponseBase<bool>()
                {
                    Message = ex.Message,
                    Success = false,
                    Object = false
                };
            }
        }

        [AllowAnonymous]
        [HttpPost("recovery-pass")]
        public async Task<ResponseBase<bool>> RecoveryPass(string email)
        {
            try
            {
                var user = await _userService.GetByEmail(email);
                if(user == null)
                {
                    return new ResponseBase<bool>()
                    {
                        Message = "User doesn't exists",
                        Object = false,
                        Quantity = 0,
                        Success = false,
                        Total = 0
                    };
                }

                MainEntity mainEntity = new MainEntity()
                {
                    EmailUser = email,
                    Validated = "0"
                };

                var result = await _service.Add(mainEntity);

                var mail = new EmailViewModel()
                {
                    Assunto = "Recuperação de senha",
                    Destinatario = user.Email,
                    Status = "0",
                    Texto = Utils.CrieEmail.CriaEmailRecupereSenha(result.Guid)
                };

                await _emailService.Add(_mapper.Map<Email>(mail));

                return new ResponseBase<bool>()
                {
                    Message = "Created",
                    Success = true,
                    Object = result != null,
                    Quantity = 1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
                return new ResponseBase<bool>()
                {
                    Message = ex.Message,
                    Success = false,
                    Object = false
                };
            }
        }

        [AllowAnonymous]
        [HttpPost("reset-pass")]
        public async Task<ResponseBase<bool>> ResetPass(string guid, string pass)
        {
            try
            {
                var result = await _service.GetByGuid(guid);

                if(result == null)
                {
                    return new ResponseBase<bool>() 
                    { 
                        Message = "Guid not found",
                        Success = false,
                        Object = false
                    };
                }

                if(result.Validated != "0")
                {
                    return new ResponseBase<bool>()
                    {
                        Message = "Already verified!",
                        Success = false,
                        Object = false
                    };
                }

                result.Validated = "1";
                result = await _service.Update(result);

                var user = await _userService.GetByEmail(result.EmailUser);
                user.Pass = pass;
                user = await _userService.Update(user);

                var mail = new EmailViewModel()
                {
                    Assunto = "Recuperação de senha",
                    Destinatario = user.Email,
                    Status = "0",
                    Texto = Utils.CrieEmail.ConfirmaAlteracaoPass(user.Nome)
                };

                return new ResponseBase<bool>()
                {
                    Message = "Created",
                    Success = true,
                    Object = result != null,
                    Quantity = 1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue on {GetType().Name}.{MethodBase.GetCurrentMethod().Name}", ex);
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
