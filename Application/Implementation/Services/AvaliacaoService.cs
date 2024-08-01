using Main = Domain.Entities.Avaliacao;
using IService = Application.Interface.Services.IAvaliacaoService;
using IQuestoesAvaliacaoService = Application.Interface.Services.IQuestoesAvaliacaoService;
using IRepository = Application.Interface.Repositories.IAvaliacaoRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using Domain.Entities;
using System.Text;
using AutoMapper;

namespace Application.Implementation.Services
{
    public class AvaliacaoService : IService
    {
        private readonly IRepository _repository;
        private readonly IQuestoesAvaliacaoService _questoesAvaliacaoService;
        private readonly IRepositoryCodes _repositoryCodes;
        private readonly IMapper _mapper;
        public AvaliacaoService(IRepository repository, IRepositoryCodes repositoryCodes, IQuestoesAvaliacaoService questoesAvaliacaoService, IMapper mapper)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
            _questoesAvaliacaoService = questoesAvaliacaoService;
            _mapper = mapper;
        }

        public async Task<Main> Add(Main entity, int user)
        {
            entity.Id = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
            entity.CreatedBy = user;
            entity.UpdatedBy = user;
            entity.CreatedOn = DateTime.Now;
            entity.UpdatedOn = DateTime.Now;
            entity.IsActive = "1";

            if (entity.Id == -1) throw new Exception("Impossible to create a new Id");

            foreach (var item in entity.QuestoesAvaliacao)
            {
                item.IdAvaliacao = entity.Id;
                await _questoesAvaliacaoService.Add(item, user);
            }

            if(entity.IsPublic != "1")
            {
                entity.Key = Guid.NewGuid().ToString();
            }

            var retorno = await _repository.Add(entity);

            return retorno;
        }

        public async Task<bool> DeleteById(int id, int user)
        {
            var entity = await GetById(id);
            entity.UpdatedBy = user;
            entity.UpdatedOn = DateTime.Now;
            entity.IsActive = "1";

            return await _repository.Update(entity) != null;
        }

        public async Task<IEnumerable<Main>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity, string chave, int user, string subject, string bancas, string provas, string materias, string professores)
        {
            var list = await _repository.GetAllPagged(page, quantity, chave, user, subject, bancas, provas, materias, professores);

            return list;
        }

        public async Task<Main> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<Main> Update(Main entity, int user)
        {
            entity.UpdatedBy = user;
            entity.UpdatedOn = DateTime.Now;
            entity.IsActive = "1";

            foreach (var item in await _questoesAvaliacaoService.GetAllByAvaliacao(entity.Id))
            {
                await _questoesAvaliacaoService.DeleteById(item.Id);
            }
            
            foreach(var item in entity.QuestoesAvaliacao)
            {
                item.IdAvaliacao = entity.Id;
                await _questoesAvaliacaoService.Add(item, user);
            }

            return await _repository.Update(entity);
        }

        public async Task<IEnumerable<Main>> GetByUserId(int id)
        {
            return await _repository.GetByUserId(id);
        }

        public async Task<int> QuantidadeTotal()
        {
            return await _repository.QuantidadeTotal();
        }

        public async Task<IEnumerable<string>> GetAllProfessores()
        {
            return await _repository.GetAllProfessores();
        }

        public async Task<string> CriaDocumentoAvaliacao(int codigo)
        {
            Avaliacao avaliacao = await GetById(codigo);
            var questoes = await _questoesAvaliacaoService.GetAllByAvaliacao(avaliacao.Id);

            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"<!DOCTYPE html PUBLIC \" -//W3C//DTD HTML 4.01//EN\" \"https://www.w3.org/TR/html4/strict.dtd\">");
            builder.AppendLine($"<html lang=\"pt-BR\">");
            builder.AppendLine($"");
            builder.AppendLine($"<head>");
            builder.AppendLine($"    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
            builder.AppendLine($"    <style type=\"text/css\" nonce=\"\">");
            builder.AppendLine($"        body,");
            builder.AppendLine($"        td,");
            builder.AppendLine($"        div,");
            builder.AppendLine($"        p,");
            builder.AppendLine($"        a,");
            builder.AppendLine("        input {");
            builder.AppendLine($"            font-family: arial, sans-serif;");
            builder.AppendLine("        }");
            builder.AppendLine($"    </style>");
            builder.AppendLine($"    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">");
            builder.AppendLine($"    <title>Prova - {avaliacao.Nome}</title>");
            builder.AppendLine($"    <style type=\"text/css\" nonce=\"\">");
            builder.AppendLine(@"        body,
                                        td {
                                            font-size: 13px;
                                            line-height: 1.8;
										}
									
											a:link,
										a:active {
												color: #1155CC;
												text-decoration: none
										}
									
										a:hover {
												text-decoration: underline;
												cursor: pointer
										}
									
										a:visited {
												color: #6611CC
										}
									
										img {
												border: 0px
										}
									
										pre {
												white-space: pre;
												white-space: -moz-pre-wrap;
												white-space: -o-pre-wrap;
												white-space: pre-wrap;
												word-wrap: break-word;
												max-width: 800px;
												overflow: auto;
										}
									
										.logo {
												left: -7px;
												position: relative;
										}
									
										h3{
												padding: 5px;
										}

                                        .maincontent{
                                            margin: 32px
                                        }
									
										.centerDiv{
												width: 100%;
												display: flex;
												justify-content: center;
                                                flex-direction: column;
										}
									
										.resultado{
												text-align: center;
												width: 80%;
												padding: 10px;
										}
									
										.questao{
									        display: flex;
                                            flex-direction: column;
										}

                                        .questao img{
									        margin-top: 16px;
											margin-bottom: 16px;
											max-width: 100%;
											max-height: 100%;
											object-fit: cover;
											display: block;
										}
										</style>");
            builder.AppendLine("<style type=\"text/css\"></style>");
            builder.AppendLine($"</head>");
            builder.AppendLine($"");
            builder.AppendLine($"<body>");
            builder.AppendLine($"    <div class=\"bodycontainer\" align=\"justify\">");
            builder.AppendLine($"        <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
            builder.AppendLine($"            <tbody>");
            builder.AppendLine($"                <tr height=\"14px\">");
            builder.AppendLine($"                    <td width=\"143\"> ");
            builder.AppendLine($"                        <img src=\"https://www.sunsalesystem.com.br/img/logo.png\" style=\"width: 10%;\"><b>SunSale System</b> ");
            builder.AppendLine($"                    </td>");
            builder.AppendLine($"                    <td align=\"right\">");
            builder.AppendLine($"                        <font size=\"-1\" color=\"#777\">");
            builder.AppendLine($"                            <b>SunSale System &lt;sunsalesystem@gmail.com&gt;</b>");
            builder.AppendLine($"                        </font> ");
            builder.AppendLine($"                    </td>");
            builder.AppendLine($"                </tr>");
            builder.AppendLine($"            </tbody>");
            builder.AppendLine($"        </table>");
            builder.AppendLine($"        <br>");
            builder.AppendLine($"        <hr>");
            builder.AppendLine($"        <div class=\"maincontent\">");
            builder.AppendLine($"            <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
            builder.AppendLine($"                <tbody>");
            builder.AppendLine($"                    <tr>");
            builder.AppendLine($"                        <td> ");
            builder.AppendLine($"                            <font size=\"+1\">");
            builder.AppendLine($"                                Prova: <b>{avaliacao.Nome}</b><br>");
            builder.AppendLine($"                            </font>");
            builder.AppendLine($"                            <font size=\"-2\">");
            builder.AppendLine($"                                Professor: <b>{avaliacao.Usuario.Nome}</b>");
            builder.AppendLine($"                            </font>");
            builder.AppendLine($"                        </td>");
            builder.AppendLine($"                        <td align=\"right\">");
            builder.AppendLine($"                            <font size=\"-1\">");
            builder.AppendLine($"                            </font>");
            builder.AppendLine($"                        </td>");
            builder.AppendLine($"                    </tr>");
            builder.AppendLine($"                </tbody>");
            builder.AppendLine($"            </table>");
            builder.AppendLine($"            <br>");
            builder.AppendLine($"            <div class=\"centerDiv\">");
            questoes?.ToList().ForEach(questao =>
            {
                string textoQuestao = questao.Questao.CampoQuestao;
                int i = 0;
                questao.Questao.AnexosQuestoes?.ToList().ForEach(anexo =>
                {
                    textoQuestao = textoQuestao.Replace($"<img src=\"#\" alt=\"Anexo\" Id=\"divAnexo0\"/>", $"<img src=\"{_mapper.Map<string>(anexo.Anexo)}\" alt=\"Anexo\" Id=\"divAnexo${i}\"/>");
                    i++;
                });

                builder.AppendLine($"           <div class=\"questao\">");
                builder.AppendLine($"               <h4>");
                builder.AppendLine($"                   {textoQuestao}");
                builder.AppendLine($"               </h4>");
                builder.AppendLine($"               </br>");
                builder.AppendLine($"               </br>");

                List<string> list = new List<string>() { "a", "b", "c", "d", "e", "f", "g" };
                i = 0;
                questao.Questao.RespostasQuestoes?.ToList().ForEach(resposta =>
                {
                    builder.AppendLine($"                   <div class=\"resposta\">");
                    builder.AppendLine($"                       <h4>");
                    builder.AppendLine($"                           ({list[i]}) [    ] ");

                    if (resposta.AnexoResposta?.Count > 0)
                    {
                        resposta.AnexoResposta?.ToList().ForEach(anexo =>
                        {
                            builder.AppendLine($"                       <img src=\"{_mapper.Map<string>(anexo.Anexo)}\"/>");
                        });
                    }
                    else
                    {
                        builder.AppendLine($"               {resposta.TextoResposta}");
                    }
                    builder.AppendLine($"                       </h4>");
                    builder.AppendLine($"                   </div>");
                    i++;
                });

                builder.AppendLine($"           </div>");
                builder.AppendLine($"            <hr>");
                builder.AppendLine($"            <br>");
                builder.AppendLine($"            <br>");
            });
            builder.AppendLine($"            </div>");
            builder.AppendLine($"            <br>");
            builder.AppendLine($"            <br>");
            builder.AppendLine($"            <br>");
            builder.AppendLine($"            <hr> ");
            builder.AppendLine($"        </div>");
            builder.AppendLine($"");
            builder.AppendLine($"    </div>");
            builder.AppendLine($"    <script type=\"text/javascript\" nonce=\"\">");
            builder.AppendLine("        document.body.onload = function() {");
            builder.AppendLine($"            document.body.offsetHeight;");
            builder.AppendLine($"            window.print()");
            builder.AppendLine("        };");
            builder.AppendLine($"    </script>");
            builder.AppendLine($"</body>");
            builder.AppendLine($"");
            builder.AppendLine($"</html>");

            string retorno = $"data:application/json;base64, {Convert.ToBase64String(Encoding.UTF8.GetBytes(builder.ToString()))}";

            return retorno;
        }

        public async Task<string> CriaDocumentoGabaritoAvaliacao(int codigo)
        {
            Main avaliacao = await GetById(codigo);
            var questoes = await _questoesAvaliacaoService.GetAllByAvaliacao(avaliacao.Id);

            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"<!DOCTYPE html PUBLIC \" -//W3C//DTD HTML 4.01//EN\" \"https://www.w3.org/TR/html4/strict.dtd\">");
            builder.AppendLine($"<html lang=\"pt-BR\">");
            builder.AppendLine($"");
            builder.AppendLine($"<head>");
            builder.AppendLine($"    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
            builder.AppendLine($"    <style type=\"text/css\" nonce=\"\">");
            builder.AppendLine($"        body,");
            builder.AppendLine($"        td,");
            builder.AppendLine($"        div,");
            builder.AppendLine($"        p,");
            builder.AppendLine($"        a,");
            builder.AppendLine("        input {");
            builder.AppendLine($"            font-family: arial, sans-serif;");
            builder.AppendLine("        }");
            builder.AppendLine($"    </style>");
            builder.AppendLine($"    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">");
            builder.AppendLine($"    <title>Gabarito - {avaliacao.Nome}</title>");
            builder.AppendLine($"    <style type=\"text/css\" nonce=\"\">");
            builder.AppendLine(@"        body,
                                        td {
                                            font-size: 13px;
                                            line-height: 1.8;
										}
									
											a:link,
										a:active {
												color: #1155CC;
												text-decoration: none
										}
									
										a:hover {
												text-decoration: underline;
												cursor: pointer
										}
									
										a:visited {
												color: #6611CC
										}
									
										img {
												border: 0px
										}
									
										pre {
												white-space: pre;
												white-space: -moz-pre-wrap;
												white-space: -o-pre-wrap;
												white-space: pre-wrap;
												word-wrap: break-word;
												max-width: 800px;
												overflow: auto;
										}
									
										.logo {
												left: -7px;
												position: relative;
										}
									
										h3{
												padding: 5px;
										}

                                        .maincontent{
                                            margin: 32px
                                        }
									
										.centerDiv{
												width: 100%;
												display: flex;
												justify-content: center;
                                                flex-direction: column;
										}
									
										.resultado{
												text-align: center;
												width: 80%;
												padding: 10px;
										}
									
										.questao{
									        display: flex;
                                            flex-direction: column;
										}

                                        .questao img{
									        margin-top: 16px;
											margin-bottom: 16px;
											max-width: 100%;
											max-height: 100%;
											object-fit: cover;
											display: block;
										}
										</style>");
            builder.AppendLine("<style type=\"text/css\"></style>");
            builder.AppendLine($"</head>");
            builder.AppendLine($"");
            builder.AppendLine($"<body>");
            builder.AppendLine($"    <div class=\"bodycontainer\" align=\"justify\">");
            builder.AppendLine($"        <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
            builder.AppendLine($"            <tbody>");
            builder.AppendLine($"                <tr height=\"14px\">");
            builder.AppendLine($"                    <td width=\"143\"> ");
            builder.AppendLine($"                        <img src=\"https://www.sunsalesystem.com.br/img/logo.png\" style=\"width: 10%;\"><b>SunSale System</b> ");
            builder.AppendLine($"                    </td>");
            builder.AppendLine($"                    <td align=\"right\">");
            builder.AppendLine($"                        <font size=\"-1\" color=\"#777\">");
            builder.AppendLine($"                            <b>SunSale System &lt;sunsalesystem@gmail.com&gt;</b>");
            builder.AppendLine($"                        </font> ");
            builder.AppendLine($"                    </td>");
            builder.AppendLine($"                </tr>");
            builder.AppendLine($"            </tbody>");
            builder.AppendLine($"        </table>");
            builder.AppendLine($"        <br>");
            builder.AppendLine($"        <hr>");
            builder.AppendLine($"        <div class=\"maincontent\">");
            builder.AppendLine($"            <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
            builder.AppendLine($"                <tbody>");
            builder.AppendLine($"                    <tr>");
            builder.AppendLine($"                        <td> ");
            builder.AppendLine($"                            <font size=\"+1\">");
            builder.AppendLine($"                                Gabarito Avaliação: <b>{avaliacao.Nome}</b>");
            builder.AppendLine($"                            </font>");
            builder.AppendLine($"                        </td>");
            builder.AppendLine($"                        <td align=\"right\">");
            builder.AppendLine($"                            <font size=\"-2\">");
            builder.AppendLine($"                                Professor: <b>{avaliacao.Usuario.Nome}</b>");
            builder.AppendLine($"                            </font>");
            builder.AppendLine($"                        </td>");
            builder.AppendLine($"                    </tr>");
            builder.AppendLine($"                </tbody>");
            builder.AppendLine($"            </table>");
            builder.AppendLine($"            <br>");
            builder.AppendLine($"            <div class=\"centerDiv\">");
            questoes?.ToList().ForEach(questao =>
            {
                builder.AppendLine($"                   <div class=\"resposta\">");
                questao.Questao.RespostasQuestoes?.ToList().Where(q => q.Certa.Equals("1"))?.ToList()?.ForEach(resposta =>
                {

                    builder.AppendLine($"                       <h4>");
                    builder.AppendLine($"                           Questão {questao.Questao.NumeroQuestao} - Resposta certa: ");

                    if (resposta.AnexoResposta?.Count > 0)
                    {
                        resposta.AnexoResposta?.ToList().ForEach(anexo =>
                        {
                            builder.AppendLine($"                       <img src=\"{_mapper.Map<string>(anexo.Anexo)}\"/>");
                        });
                    }
                    else
                    {
                        builder.AppendLine($"               {resposta.TextoResposta}");
                    }
                    builder.AppendLine($"                       </h4>");
                });
                builder.AppendLine($"                   </div>");

                builder.AppendLine($"            <hr>");
            });
            builder.AppendLine($"            </div>");
            builder.AppendLine($"            <br>");
            builder.AppendLine($"            <hr> ");
            builder.AppendLine($"        </div>");
            builder.AppendLine($"");
            builder.AppendLine($"    </div>");
            builder.AppendLine($"    <script type=\"text/javascript\" nonce=\"\">");
            builder.AppendLine("        document.body.onload = function() {");
            builder.AppendLine($"            document.body.offsetHeight;");
            builder.AppendLine($"            window.print()");
            builder.AppendLine("        };");
            builder.AppendLine($"    </script>");
            builder.AppendLine($"</body>");
            builder.AppendLine($"");
            builder.AppendLine($"</html>");

            string retorno = $"data:application/json;base64, {Convert.ToBase64String(Encoding.UTF8.GetBytes(builder.ToString()))}";

            return retorno;
        }


        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
