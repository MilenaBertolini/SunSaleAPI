using Main = Domain.Entities.Prova;
using IService = Application.Interface.Services.IProvaService;
using IRepository = Application.Interface.Repositories.IProvaRepository;
using IServiceAcao = Application.Interface.Services.IAcaoUsuarioService;
using IServiceQuestoes = Application.Interface.Services.IQuestoesService;
using IServiceTipoProvaAssociado = Application.Interface.Services.ITipoProvaAssociadoService;
using IRepositoryCodes = Application.Interface.Repositories.ICodigosTableRepository;
using Domain.Entities;
using Application.Model;
using System.Text;
using AutoMapper;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Implementation.Services
{
    public class ProvaService : IService
    {
        private readonly IRepository _repository;
        private readonly IServiceAcao _serviceAcao;
        private readonly IServiceQuestoes _serviceQuestoes;
        private readonly IRepositoryCodes _repositoryCodes;
        private readonly IServiceTipoProvaAssociado _serviceTipoProvaAssociado;
        private readonly IMapper _mapper;

        public ProvaService(IRepository repository, IRepositoryCodes repositoryCodes, IServiceAcao serviceAcao, IServiceTipoProvaAssociado serviceTipoProvaAssociado, IServiceQuestoes serviceQuestoes, IMapper mapper)
        {
            _repository = repository;
            _repositoryCodes = repositoryCodes;
            _serviceAcao = serviceAcao;
            _serviceTipoProvaAssociado = serviceTipoProvaAssociado;
            _serviceQuestoes = serviceQuestoes;
            _mapper = mapper;
        }

        public async Task<Main> Add(Main entity, int codigoUsuario)
        {
            entity.Codigo = await _repositoryCodes.GetNextCodigo(typeof(Main).Name);
            if (entity.Codigo == -1) throw new Exception("Impossible to create a new Id");

            entity.DataRegistro = DateTime.Now;
            entity.CreatedBy = codigoUsuario;
            entity.UpdatedBy = codigoUsuario;
            entity.UpdatedOn = DateTime.Now;

            var tipos = new List<TipoProvaAssociado>();
            foreach(var t in entity.TipoProvaAssociado.ToList())
            {
                t.CodigoProva = entity.Codigo;
                t.Codigo = await _repositoryCodes.GetNextCodigo(typeof(TipoProvaAssociado).Name);
                tipos.Add(t);
            };

            entity.TipoProvaAssociado.Clear();
            tipos.ForEach(t => entity.TipoProvaAssociado.Add(t));


            var result = await _repository.Add(entity);
            
            await _serviceAcao.Add(new AcaoUsuario()
            {
                Acao = $"Inserir Prova {result.Codigo} - {result.NomeProva}",
                CodigoUsuario = codigoUsuario
            });

            return result;
        }

        public Task<bool> DeleteById(int id)
        {
            return _repository.Delete(id);
        }

        public async Task<IEnumerable<Main>> GetSimulados()
        {
            return await _repository.GetSimulados();
        }

        public async Task<Tuple<IEnumerable<Main>, int>> GetAllPagged(int page, int quantity, string tipo, string prova, bool admin)
        {
            return await _repository.GetAllPagged(page, quantity, tipo, prova, admin);
        }

        public async Task<IEnumerable<Main>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Main> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<Main> Update(Main entity, int user)
        {
            entity.UpdatedBy = user;
            entity.UpdatedOn = DateTime.Now;

            var retorno = await _repository.Update(entity);

            var tipos = await _serviceTipoProvaAssociado.GetAllByProva(entity.Codigo);

            foreach(var t in tipos)
            {
                await _serviceTipoProvaAssociado.DeleteById(t.Codigo);
            }

            foreach(var t in entity.TipoProvaAssociado)
            {
                await _serviceTipoProvaAssociado.Add(t);
            }

            return retorno;
        }

        public async Task<string> CriaDocumentoProva(int codigo)
        {
            Prova prova = await GetById(codigo);
            var questoes = await _serviceQuestoes.GetQuestoesByProva(prova.Codigo);

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
            builder.AppendLine($"    <title>Prova - {prova.NomeProva}</title>");
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
											max-width: 50%;
											max-height: 50%;
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
            builder.AppendLine($"                                <b>{prova.NomeProva}</b>");
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
            questoes.ToList().ForEach(questao =>
            {
                string textoQuestao = questao.CampoQuestao;
                int i = 0;
                questao.AnexosQuestoes?.ToList().ForEach(anexo =>
                {
                    textoQuestao = textoQuestao.Replace($"<img src=\"#\" alt=\"Anexo\" id=\"divAnexo0\"/>", $"<img src=\"{_mapper.Map<string>(anexo.Anexo)}\" alt=\"Anexo\" id=\"divAnexo${i}\"/>");
                    i++;
                });

                builder.AppendLine($"           <div class=\"questao\">");
                builder.AppendLine($"               {textoQuestao}");
                builder.AppendLine($"               </br>");
                builder.AppendLine($"               </br>");

                List<string> list = new List<string>() { "a", "b", "c", "d", "e", "f", "g"};
                i = 0;
                questao.RespostasQuestoes.ToList().ForEach(resposta =>
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
