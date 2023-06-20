using Main = Domain.Entities.RespostasQuestoes;
using IService = Application.Interface.Services.IRespostasQuestoesService;
using IRepository = Application.Interface.Repositories.IRespostasQuestoesRepository;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using Application.Model;
using Domain.Entities;
using System.Text;

namespace Application.Implementation.Services
{
    public class RespostasQuestoesService : IService
    {
        private readonly IRepository _repository;
        private readonly IRepositoryCodes _repositoryCodes;

        public RespostasQuestoesService(IRepository repository, IRepositoryCodes repositoryCodes)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
        }

        public async Task<Main> Add(Main entity)
        {
            entity.Codigo = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);

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
        public async Task<Main> GetRespostaCorreta(int questao)
        {
            return await _repository.GetRespostaCorreta(questao);
        }

        public string CriaDocumentoDetalhado(IEnumerable<Questoes> questoes, Usuarios user, List<Prova> provas)
        {
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
            builder.AppendLine($"    <title>Histórico - {user.Nome}</title>");
            builder.AppendLine($"    <style type=\"text/css\" nonce=\"\">");
            builder.AppendLine($"        body,");
            builder.AppendLine("        td {");
            builder.AppendLine($"            font-size: 13px;");
            builder.AppendLine($"            line-height: 1.8;");
            builder.AppendLine("        }");
            builder.AppendLine($"");
            builder.AppendLine($"        a:link,");
            builder.AppendLine("        a:active {");
            builder.AppendLine($"            color: #1155CC;");
            builder.AppendLine($"            text-decoration: none");
            builder.AppendLine("        }");
            builder.AppendLine($"");
            builder.AppendLine("        a:hover {");
            builder.AppendLine($"            text-decoration: underline;");
            builder.AppendLine($"            cursor: pointer");
            builder.AppendLine("        }");
            builder.AppendLine($"");
            builder.AppendLine("        a:visited {");
            builder.AppendLine($"            color: #6611CC");
            builder.AppendLine("        }");
            builder.AppendLine($"");
            builder.AppendLine("        img {");
            builder.AppendLine($"            border: 0px");
            builder.AppendLine("        }");
            builder.AppendLine($"");
            builder.AppendLine("        pre {");
            builder.AppendLine($"            white-space: pre;");
            builder.AppendLine($"            white-space: -moz-pre-wrap;");
            builder.AppendLine($"            white-space: -o-pre-wrap;");
            builder.AppendLine($"            white-space: pre-wrap;");
            builder.AppendLine($"            word-wrap: break-word;");
            builder.AppendLine($"            max-width: 800px;");
            builder.AppendLine($"            overflow: auto;");
            builder.AppendLine("        }");
            builder.AppendLine($"");
            builder.AppendLine("        .logo {");
            builder.AppendLine($"            left: -7px;");
            builder.AppendLine($"            position: relative;");
            builder.AppendLine("        }");
            builder.AppendLine($"");
            builder.AppendLine("        h3{");
            builder.AppendLine($"            padding: 5px;");
            builder.AppendLine("        }");
            builder.AppendLine($"");
            builder.AppendLine("        .centerDiv{");
            builder.AppendLine($"            width: 100%;");
            builder.AppendLine($"            display: flex;");
            builder.AppendLine($"            justify-content: center;");
            builder.AppendLine("        }");
            builder.AppendLine($"");
            builder.AppendLine("        .resultado{");
            builder.AppendLine($"            text-align: center;");
            builder.AppendLine($"            width: 80%;");
            builder.AppendLine($"            padding: 10px;");
            builder.AppendLine("        }");
            builder.AppendLine($"");
            builder.AppendLine("        .resultado table{");
            builder.AppendLine($"");
            builder.AppendLine("        }");
            builder.AppendLine($"    </style>");
            builder.AppendLine($"    <style type=\"text/css\"></style>");
            builder.AppendLine($"</head>");
            builder.AppendLine($"");
            builder.AppendLine($"<body>");
            builder.AppendLine($"    <div class=\"bodycontainer\" align=\"justify\">");
            builder.AppendLine($"        <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
            builder.AppendLine($"            <tbody>");
            builder.AppendLine($"                <tr height=\"14px\">");
            builder.AppendLine($"                    <td width=\"143\"> ");
            builder.AppendLine($"                            <b>SunSale System</b> ");
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
            builder.AppendLine($"                                <b>Histórico de questões</b>");
            builder.AppendLine($"                            </font>");
            builder.AppendLine($"                            <h3>");
            builder.AppendLine($"                                Usuário: {user.Nome}");
            builder.AppendLine($"                                <br/>");
            builder.AppendLine($"                                Quantidade de questões respondidas: {questoes.Count()}");
            builder.AppendLine($"                                <br/>");
            builder.AppendLine($"                                Quantidade de questões acertadas: {questoes.Where(q => q.RespostasQuestoes.Where(r => r.Certa.Equals("1")).Count() > 0).Count()}");
            builder.AppendLine($"                                <br/>");
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
            builder.AppendLine($"                <table border=\"0\" class=\"resultado\">");
            builder.AppendLine($"                    <thead>");
            builder.AppendLine($"                        <tr>");
            builder.AppendLine($"                            <th width=\"17%\">");
            builder.AppendLine($"                                Prova");
            builder.AppendLine($"                            </th>");
            builder.AppendLine($"                            <th width=\"17%\">");
            builder.AppendLine($"                                Número questão");
            builder.AppendLine($"                            </th>");
            builder.AppendLine($"                            <th width=\"49%\">");
            builder.AppendLine($"                                Matéria");
            builder.AppendLine($"                            </th>");
            builder.AppendLine($"                            <th width=\"33%\">");
            builder.AppendLine($"                                Resultado");
            builder.AppendLine($"                            </th>");
            builder.AppendLine($"                        </tr>");
            builder.AppendLine($"                    </thead>");
            builder.AppendLine($"                    <tbody>");
            questoes.ToList().ForEach(questao =>
            {
                var questaoDto = questoes.Where(q => q.NumeroQuestao.Equals(questao.NumeroQuestao)).FirstOrDefault();
                builder.AppendLine($"                        <tr>");
                builder.AppendLine($"                            <td> ");
                builder.AppendLine($"                                <h3>");
                builder.AppendLine($"                                    {provas.Where(p => p.Codigo.Equals(questao.CodigoProva)).FirstOrDefault()?.NomeProva}");
                builder.AppendLine($"                                </h3>");
                builder.AppendLine($"                            </td>");
                builder.AppendLine($"                            <td> ");
                builder.AppendLine($"                                <h3>");
                builder.AppendLine($"                                    {questao.NumeroQuestao}");
                builder.AppendLine($"                                </h3>");
                builder.AppendLine($"                            </td>");
                builder.AppendLine($"                            <td> ");
                builder.AppendLine($"                                <h3>");
                builder.AppendLine($"                                    {questaoDto?.Materia}");
                builder.AppendLine($"                                </h3>");
                builder.AppendLine($"                            </td>");
                builder.AppendLine($"                            <td> ");
                builder.AppendLine($"                                <h3>");
                builder.AppendLine($"                                    {(questao.RespostasQuestoes.ToList().Exists(r => r.Certa.Equals("1")) ? "Certa🥳" : "Errada😒")}");
                builder.AppendLine($"                                </h3>");
                builder.AppendLine($"                            </td>");
                builder.AppendLine($"                        </tr>");
            });

            builder.AppendLine($"                    </tbody>");
            builder.AppendLine($"                </table>");
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
