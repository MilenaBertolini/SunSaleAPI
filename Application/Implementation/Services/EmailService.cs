using Main = Domain.Entities.Email;
using IService = Application.Interface.Services.IEmailService;
using IRepository = Application.Interface.Repositories.IEmailRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.EntityFrameworkCore.Metadata;
using Domain.Entities;
using Microsoft.Extensions.Options;
using AutoMapper;
using Application.Interface.Services;
using System.Reflection;

namespace Application.Implementation.Services
{
    public class EmailService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;
        private readonly EmailSettings _emailSettings;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;

        public EmailService(IRepository repository, IRepositoryCodes repositoryCodes, IOptions<EmailSettings> emailSettings, IMapper mapper, ILoggerService loggerService)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
            _emailSettings = emailSettings.Value;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        public async Task<Main> Add(Main entity)
        {
            entity.Codigo = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
            entity.DataEnvio = DateTime.Now;

            if(await EnviaEmail(entity))
            {
                entity.Status = "1";
                entity.Observacao = "Enviado pelo sendGrid";
            }
            else
            {
                entity.Status = "2";
            }

            if (entity.Codigo == -1) throw new Exception("Impossible to create a new Id");

            return await _repository.Add(entity);
        }

        private async Task<bool> EnviaEmail(Main entity)
        {
            bool retorno = true;

            try
            {
                var client = new SendGridClient(_emailSettings.SendGridApiKey);
                var from = new EmailAddress(_emailSettings.EmailCredential);
                var subject = entity.Assunto;
                var to = new EmailAddress(entity.Destinatario);
                var plainTextContent = _mapper.Map<string>(entity.Texto);
                var htmlContent = plainTextContent;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);

                retorno = response.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                await _loggerService.AddException(ex);
                retorno = false;
            }

            return retorno;
        }

        public Task<bool> DeleteById(int id)
        {
            return _repository.Delete(id);
        }

        public async Task<IEnumerable<Main>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity)
        {
            return await _repository.GetAllPagged(page, quantity);
        }

        public async Task<Main> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public Task<Main> Update(Main entity)
        {
            return _repository.Update(entity);
        }

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
