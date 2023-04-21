using AutoMapper;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using PessoaMainViewModel = Domain.ViewModel.PessoasForDevViewModel;
using EmpresaMainViewModel = Domain.ViewModel.EmpresaForDevViewModel;
using CartaoCreditoMainViewModel = Domain.ViewModel.CartaoCreditoDevToolsViewModel;
using VeiculosMainViewModel = Domain.ViewModel.VeiculosForDevViewModel;
using ServicePerson = Application.Interface.Services.IPessoasForDevService;
using ServiceEmpresa = Application.Interface.Services.IEmpresaForDevService;
using ServiceCartao = Application.Interface.Services.ICartaoCreditoDevToolsService;
using ServiceVeiculo = Application.Interface.Services.IVeiculosForDevService;

namespace APISunSale.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ForDevPublicController
    {
        private readonly ILogger<ForDevPublicController> _logger;
        private readonly ServicePerson _servicePerson;
        private readonly ServiceEmpresa _serviceEmpresa;
        private readonly ServiceCartao _serviceCartao;
        private readonly ServiceVeiculo _serviceVeiculo;
        private readonly IMapper _mapper;
        public ForDevPublicController(ILogger<ForDevPublicController> logger, ServicePerson servicePerson, IMapper mapper, ServiceEmpresa serviceEmpresa, ServiceCartao serviceCartao, ServiceVeiculo serviceVeiculo)
        {
            _logger = logger;
            _servicePerson = servicePerson;
            _mapper = mapper;
            _serviceEmpresa = serviceEmpresa;
            _serviceCartao = serviceCartao;
            _serviceVeiculo = serviceVeiculo;
        }

        [HttpGet("person/random")]
        public async Task<ResponseBase<List<PessoaMainViewModel>>> GetRandomPerson(int? qt)
        {
            try
            {
                var result = await _servicePerson.GetRandom(qt);
                var response = _mapper.Map<List<PessoaMainViewModel>>(result);
                return new ResponseBase<List<PessoaMainViewModel>>()
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
                return new ResponseBase<List<PessoaMainViewModel>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet("person/pagged")]
        public async Task<ResponseBase<List<PessoaMainViewModel>>> GetAllPersonPagged(int page, int quantity)
        {
            try
            {
                var result = await _servicePerson.GetAllPagged(page, quantity);
                var response = _mapper.Map<List<PessoaMainViewModel>>(result);
                return new ResponseBase<List<PessoaMainViewModel>>()
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
                return new ResponseBase<List<PessoaMainViewModel>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet("company/random")]
        public async Task<ResponseBase<List<EmpresaMainViewModel>>> GetRandomCompany(int? qt)
        {
            try
            {
                var result = await _serviceEmpresa.GetRandom(qt);
                var response = _mapper.Map<List<EmpresaMainViewModel>>(result);
                return new ResponseBase<List<EmpresaMainViewModel>>()
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
                return new ResponseBase<List<EmpresaMainViewModel>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet("company/pagged")]
        public async Task<ResponseBase<List<EmpresaMainViewModel>>> GetAllCompanyPagged(int page, int quantity)
        {
            try
            {
                var result = await _serviceEmpresa.GetAllPagged(page, quantity);
                var response = _mapper.Map<List<EmpresaMainViewModel>>(result);
                return new ResponseBase<List<EmpresaMainViewModel>>()
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
                return new ResponseBase<List<EmpresaMainViewModel>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet("card/random")]
        public async Task<ResponseBase<List<CartaoCreditoMainViewModel>>> GetRandomCard(int? qt)
        {
            try
            {
                var result = await _serviceCartao.GetRandom(qt);
                var response = _mapper.Map<List<CartaoCreditoMainViewModel>>(result);
                return new ResponseBase<List<CartaoCreditoMainViewModel>>()
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
                return new ResponseBase<List<CartaoCreditoMainViewModel>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet("card/pagged")]
        public async Task<ResponseBase<List<CartaoCreditoMainViewModel>>> GetAllCardsPagged(int page, int quantity)
        {
            try
            {
                var result = await _serviceCartao.GetAllPagged(page, quantity);
                var response = _mapper.Map<List<CartaoCreditoMainViewModel>>(result);
                return new ResponseBase<List<CartaoCreditoMainViewModel>>()
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
                return new ResponseBase<List<CartaoCreditoMainViewModel>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet("vehicle/random")]
        public async Task<ResponseBase<List<VeiculosMainViewModel>>> GetRandomVehicle(int? qt)
        {
            try
            {
                var result = await _serviceVeiculo.GetRandom(qt);
                var response = _mapper.Map<List<VeiculosMainViewModel>>(result);
                return new ResponseBase<List<VeiculosMainViewModel>>()
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
                return new ResponseBase<List<VeiculosMainViewModel>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        [HttpGet("vehicle/pagged")]
        public async Task<ResponseBase<List<VeiculosMainViewModel>>> GetAllVehiclePagged(int page, int quantity)
        {
            try
            {
                var result = await _serviceVeiculo.GetAllPagged(page, quantity);
                var response = _mapper.Map<List<VeiculosMainViewModel>>(result);
                return new ResponseBase<List<VeiculosMainViewModel>>()
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
                return new ResponseBase<List<VeiculosMainViewModel>>()
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
