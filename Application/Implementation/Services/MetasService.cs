using Main = Domain.Entities.Metas;
using IService = Application.Interface.Services.IMetasService;
using IServiceLogger = Application.Interface.Services.ILoggerService;
using IServiceEmail = Application.Interface.Services.IEmailService;
using IRepository = Application.Interface.Repositories.IMetasRepository;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using AutoMapper;
using Domain.Entities;
using Domain.ViewModel;

namespace Application.Implementation.Services
{
    public class MetasService : IService
    {
        private readonly IRepository _repository;
        private readonly IServiceLogger _serviceLogger;
        private readonly IServiceEmail _serviceEmail;
        private readonly IMapper _mapper;

        public MetasService(IRepository repository, IServiceLogger serviceLogger, IServiceEmail serviceEmail, IMapper mapper)
        {
            _repository = repository;
            _serviceLogger = serviceLogger;
            _serviceEmail = serviceEmail;
            _mapper = mapper;
        }

        public async Task<Main> Add(Main entity)
        {
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;
            entity.IsActive = "1";
            entity.Sent = "0";

            var retorno =  await _repository.Add(entity);
            await EnviaEmailInicial(retorno);

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

        public async Task<bool> ExecuteProcess()
        {
            try
            {
                var list = await GetAll();
                var result = list.Where(m => m.Sent == "0" && m.DataObjetivo.Day == DateTime.Now.Day && m.DataObjetivo.Month == DateTime.Now.Month && m.DataObjetivo.Year == DateTime.Now.Year)?.ToList();

                if (result == null) return true;

                foreach(var item in result)
                {
                    await EnviaEmailMetaFinal(item);
                    item.Sent = "1";
                    await Update(item);
                }

                return true;
            }
            catch(Exception ex)
            {
                await _serviceLogger.AddException(ex);
                return false;
            }
        }

        private async Task EnviaEmailMetaFinal(Main main)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<!DOCTYPE html>");
            builder.AppendLine("<html lang=\"pt-br\">");
            builder.AppendLine("");
            builder.AppendLine("<head>");
            builder.AppendLine("  <meta charset=\"UTF-8\">");
            builder.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            builder.AppendLine("  <title>Atualizações no Projeto MetasTrack</title>");
            builder.AppendLine("</head>");
            builder.AppendLine("");
            builder.AppendLine("<body style=\"font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f4f4f4;\">");
            builder.AppendLine("");
            builder.AppendLine("  <table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" width=\"800\" style=\"border-collapse: collapse; background-color: #ffffff; margin-top: 20px; margin-bottom: 20px;\">");
            builder.AppendLine("    <tr>");
            builder.AppendLine("      <td style=\"padding: 20px; text-align: center;\">");
            builder.AppendLine("        <img src=\"https://www.sunsalesystem.com.br/img/logo.png\" alt=\"SunSale System Logo\" style=\"display: block; margin: auto;\" width=\"20%\" height=\"20%\"> SunSale System - MetasTrack");
            builder.AppendLine("      </td>");
            builder.AppendLine("    </tr>");
            builder.AppendLine("    <tr>");
            builder.AppendLine("      <td style=\"padding: 20px; text-align: left;\">");
            builder.AppendLine($"		<h1 style=\"color: #333333;\">Meta para {main.DataObjetivo.ToString("dd/MM/yyyy")}:</h1>");
            builder.AppendLine("		<ul>");
            if (!main.Meta.Contains("|"))
            {
                builder.AppendLine("			<li>");
                builder.AppendLine($"				<h2 style=\"color: #333333;\">{main.Meta}</h2>");
                builder.AppendLine("			</li>");
            }
            else
            {
                var split = main.Meta.Split('|');
                foreach (var item in split)
                {
                    builder.AppendLine("			<li>");
                    builder.AppendLine($"				<h2 style=\"color: #333333;\">{item}</h2>");
                    builder.AppendLine("			</li>");
                }
            }
            builder.AppendLine("		</ul>");
            builder.AppendLine($"		<h3 style=\"color: #333333;\">Esperamos que tenha concluído!</h>");
            builder.AppendLine("        <br>");
            builder.AppendLine("        <p style=\"color: #555555;\">Atenciosamente,</p>");
            builder.AppendLine("        <p style=\"color: #555555;\">SunSale System</p>");
            builder.AppendLine("      </td>");
            builder.AppendLine("    </tr>");
            builder.AppendLine("  </table>");
            builder.AppendLine("");
            builder.AppendLine("</body>");
            builder.AppendLine("");
            builder.AppendLine("</html>");

            await _serviceEmail.Add(_mapper.Map<Email>(new EmailViewModel()
            { 
                Assunto = "Atualização de meta para " + main.DataObjetivo.ToString("dd/MM/yyyy"),
                Destinatario = main.Email,
                Texto = builder.ToString()
            }));
        }

        private async Task EnviaEmailInicial(Main main)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<!DOCTYPE html>");
            builder.AppendLine("<html lang=\"pt-br\">");
            builder.AppendLine("");
            builder.AppendLine("<head>");
            builder.AppendLine("  <meta charset=\"UTF-8\">");
            builder.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            builder.AppendLine("  <title>Atualizações no Projeto MetasTrack</title>");
            builder.AppendLine("</head>");
            builder.AppendLine("");
            builder.AppendLine("<body style=\"font-family: Arial, sans-serif; margin: 0; padding: 0; background-color: #f4f4f4;\">");
            builder.AppendLine("");
            builder.AppendLine("  <table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" width=\"800\" style=\"border-collapse: collapse; background-color: #ffffff; margin-top: 20px; margin-bottom: 20px;\">");
            builder.AppendLine("    <tr>");
            builder.AppendLine("      <td style=\"padding: 20px; text-align: center;\">");
            builder.AppendLine("        <img src=\"https://www.sunsalesystem.com.br/img/logo.png\" alt=\"SunSale System Logo\" style=\"display: block; margin: auto;\" width=\"20%\" height=\"20%\"> SunSale System - MetasTrack");
            builder.AppendLine("      </td>");
            builder.AppendLine("    </tr>");
            builder.AppendLine("    <tr>");
            builder.AppendLine("      <td style=\"padding: 20px; text-align: left;\">");
            builder.AppendLine($"		<h1 style=\"color: #333333;\">Meta recebida!</h1>");
            builder.AppendLine("		<ul>");
            if (!main.Meta.Contains("|"))
            {
                builder.AppendLine($"		<h2 style=\"color: #333333;\">Acaba de ser feito o cadastro para a meta em {main.DataObjetivo.ToString("dd/MM/yyyy")}:</h1>");
                builder.AppendLine("			<li>");
                builder.AppendLine($"				<h2 style=\"color: #333333;\">{main.Meta}</h2>");
                builder.AppendLine("			</li>");
            }
            else
            {
                builder.AppendLine($"		<h2 style=\"color: #333333;\">Acaba de ser feito o cadastro para as metas em {main.DataObjetivo.ToString("dd/MM/yyyy")}:</h1>");
                var split = main.Meta.Split('|');
                foreach (var item in split)
                {
                    builder.AppendLine("			<li>");
                    builder.AppendLine($"				<h2 style=\"color: #333333;\">{item}</h2>");
                    builder.AppendLine("			</li>");
                }
            }
            builder.AppendLine("		</ul>");
            builder.AppendLine($"		<h3 style=\"color: #333333;\">Esperamos que conclua!</h>");
            builder.AppendLine("        <br>");
            builder.AppendLine("        <p style=\"color: #555555;\">Atenciosamente,</p>");
            builder.AppendLine("        <p style=\"color: #555555;\">SunSale System</p>");
            builder.AppendLine("      </td>");
            builder.AppendLine("    </tr>");
            builder.AppendLine("  </table>");
            builder.AppendLine("");
            builder.AppendLine("</body>");
            builder.AppendLine("");
            builder.AppendLine("</html>");

            await _serviceEmail.Add(_mapper.Map<Email>(new EmailViewModel()
            {
                Assunto = "Inclusão de meta para " + main.DataObjetivo.ToString("dd/MM/yyyy"),
                Destinatario = main.Email,
                Texto = builder.ToString()
            }));
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
