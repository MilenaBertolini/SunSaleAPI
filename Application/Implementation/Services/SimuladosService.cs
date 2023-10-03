using Main = Domain.Entities.Simulados;
using IService = Application.Interface.Services.ISimuladoService;
using IRepository = Application.Interface.Repositories.ISimuladosRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using System.Text;
using Domain.Entities;
using Application.Model;
using AutoMapper;

namespace Application.Implementation.Services
{
    public class SimuladosService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;
        private readonly IMapper _mapper;

        public SimuladosService(IRepository repository, IRepositoryCodes repositoryCodes, IMapper mapper)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
            _mapper = mapper;
        }

        public async Task<Main> Add(Main entity)
        {
            entity.Codigo = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
            entity.Created = DateTime.Now;

            if (entity.Codigo == -1) throw new Exception("Impossible to create a new Id");

            return await _repository.Add(entity);
        }

        public Task<bool> DeleteById(int id)
        {
            return _repository.Delete(id);
        }

        public async Task<IEnumerable<Main>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<IEnumerable<Main>> GetAllPagged(int page, int quantity, int user)
        {
            return await _repository.GetAllPagged(page, quantity, user);
        }

        public async Task<Main> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public Task<Main> Update(Main entity)
        {
            return _repository.Update(entity);
        }

        public Task<Main> GetByProvaUser(int provaCodigo, int user)
        {
            return _repository.GetByProvaUser(provaCodigo, user);
        }

        public string CriaDocumentoDetalhado(IEnumerable<Questoes> questoes, Main simulado, Usuarios user, List<Simulado> questoesResolvidas)
        {
            TimeSpan time = TimeSpan.FromSeconds(simulado.Tempo);
            var materias = questoes.Where(questao => questoesResolvidas.Exists(q => q.certa.Equals("1") && questao.NumeroQuestao.Equals(q.NumeroQuestao))).GroupBy(q => q.Materia).Select(g => new { Value = g.Key, Count = g.Count() }).OrderByDescending(g => g.Count);

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
            builder.AppendLine($"    <title>Boletinho - {user.Nome}</title>");
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
            builder.AppendLine($"                                <b>RESULTADO SIMULADO</b>");
            builder.AppendLine($"                            </font>");
            builder.AppendLine($"                            <h3>");
            builder.AppendLine($"                                Usuário: {user.Nome}");
            builder.AppendLine($"                                <br/>");
            builder.AppendLine($"                                Prova: {simulado.Prova.NomeProva}");
            builder.AppendLine($"                                <br/>");
            builder.AppendLine($"                                Quantidade de questões respondidas: {simulado.QuantidadeQuestoes}");
            builder.AppendLine($"                                <br/>");
            builder.AppendLine($"                                Quantidade de questões acertadas: {simulado.QuantidadeCertas}");
            builder.AppendLine($"                                <br/>");
            builder.AppendLine($"                                Tempo de duração da prova: {time.Hours}:{time.Minutes}:{time.Seconds}");
            builder.AppendLine($"                            </h3>");
            builder.AppendLine($"                        </td>");
            builder.AppendLine($"                        <td align=\"right\">");
            builder.AppendLine($"                            <font size=\"-1\">");
            builder.AppendLine($"                            </font>");
            builder.AppendLine($"                        </td>");
            builder.AppendLine($"                    </tr>");
            builder.AppendLine($"                </tbody>");
            builder.AppendLine($"            </table>");
            builder.AppendLine($"            <br>");
            builder.AppendLine($"            <h3>");
            builder.AppendLine($"                Questões:");
            builder.AppendLine($"            </h3>");
            builder.AppendLine($"            <div class=\"centerDiv\">");
            questoes?.ToList().ForEach(questao =>
            {
                string textoQuestao = questao.CampoQuestao;
                int i = 0;
                questao.AnexosQuestoes?.ToList().ForEach(anexo =>
                {
                    textoQuestao = textoQuestao.Replace($"<img src=\"#\" alt=\"Anexo\" id=\"divAnexo0\"/>", $"<img src=\"{_mapper.Map<string>(anexo.Anexo)}\" alt=\"Anexo\" id=\"divAnexo${i}\"/>");
                    i++;
                });

                builder.AppendLine($"           <div class=\"questao\">");
                builder.AppendLine($"               <h4>");
                builder.AppendLine($"                   <b>{(questoesResolvidas.Where(q => q.NumeroQuestao.Equals(questao.NumeroQuestao)).FirstOrDefault().certa.Equals("1") ? "Você acertou esta questão 🥳" : "Você errou esta questão 😒")}</b><br><br>{textoQuestao}");
                builder.AppendLine($"               </h4>");
                builder.AppendLine($"               </br>");
                builder.AppendLine($"               </br>");

                List<string> list = new List<string>() { "a", "b", "c", "d", "e", "f", "g" };
                i = 0;
                questao.RespostasQuestoes?.ToList().ForEach(resposta =>
                {
                    builder.AppendLine($"                   <div class=\"resposta\">");
                    builder.AppendLine($"                       <h4>");
                    builder.AppendLine($"                           ({list[i]}) [ {(resposta.Certa.Equals("1") ? "X" : " ")} ] ");

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

        public void Dispose()
        {
            this._repository.Dispose();
        }
    }
}
