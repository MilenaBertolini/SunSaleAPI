using Main = Domain.Entities.DadosEstagiario;
using IService = Application.Interface.Services.IEstagioService;
using System.Text;

namespace Application.Implementation.Services
{
    public class EstagioService : IService
    {
        public EstagioService()
        {
        }

        public string CriaDocumento(Main input)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<!DOCTYPE html PUBLIC \" -//W3C//DTD HTML 4.01//EN\" \"https://www.w3.org/TR/html4/strict.dtd\">");
            builder.AppendLine("<html lang=\"pt-BR\">");
            builder.AppendLine("");
            builder.AppendLine("<head>");
            builder.AppendLine("    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
            builder.AppendLine("    <style type=\"text/css\" nonce=\"\">");
            builder.AppendLine("        body,");
            builder.AppendLine("        td,");
            builder.AppendLine("        div,");
            builder.AppendLine("        p,");
            builder.AppendLine("        a,");
            builder.AppendLine("        input {");
            builder.AppendLine("            font-family: arial, sans-serif;");
            builder.AppendLine("        }");
            builder.AppendLine("    </style>");
            builder.AppendLine("    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">");
            builder.AppendLine($"    <title>Contrato - {input.Nome}</title>");
            builder.AppendLine("    <style type=\"text/css\" nonce=\"\">");
            builder.AppendLine("        body,");
            builder.AppendLine("        td {");
            builder.AppendLine("            font-size: 13px");
            builder.AppendLine("        }");
            builder.AppendLine("");
            builder.AppendLine("        a:link,");
            builder.AppendLine("        a:active {");
            builder.AppendLine("            color: #1155CC;");
            builder.AppendLine("            text-decoration: none");
            builder.AppendLine("        }");
            builder.AppendLine("");
            builder.AppendLine("        a:hover {");
            builder.AppendLine("            text-decoration: underline;");
            builder.AppendLine("            cursor: pointer");
            builder.AppendLine("        }");
            builder.AppendLine("");
            builder.AppendLine("        a:visited {");
            builder.AppendLine("            color: #6611CC");
            builder.AppendLine("        }");
            builder.AppendLine("");
            builder.AppendLine("        img {");
            builder.AppendLine("            border: 0px");
            builder.AppendLine("        }");
            builder.AppendLine("");
            builder.AppendLine("        pre {");
            builder.AppendLine("            white-space: pre;");
            builder.AppendLine("            white-space: -moz-pre-wrap;");
            builder.AppendLine("            white-space: -o-pre-wrap;");
            builder.AppendLine("            white-space: pre-wrap;");
            builder.AppendLine("            word-wrap: break-word;");
            builder.AppendLine("            max-width: 800px;");
            builder.AppendLine("            overflow: auto;");
            builder.AppendLine("        }");
            builder.AppendLine("");
            builder.AppendLine("        .logo {");
            builder.AppendLine("            left: -7px;");
            builder.AppendLine("            position: relative;");
            builder.AppendLine("        }");
            builder.AppendLine("    </style>");
            builder.AppendLine("    <style type=\"text/css\"></style>");
            builder.AppendLine("</head>");
            builder.AppendLine("");
            builder.AppendLine("<body>");
            builder.AppendLine("    <div class=\"bodycontainer\" align=\"justify\">");
            builder.AppendLine("        <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
            builder.AppendLine("            <tbody>");
            builder.AppendLine("                <tr height=\"14px\">");
            builder.AppendLine("                    <td width=\"143\"> ");
            builder.AppendLine("                            <b>SunSale System</b> ");
            builder.AppendLine("                    </td>");
            builder.AppendLine("                    <td align=\"right\">");
            builder.AppendLine("                        <font size=\"-1\" color=\"#777\">");
            builder.AppendLine("                            <b>SunSale System &lt;sunsalesystem@gmail.com&gt;</b>");
            builder.AppendLine("                        </font> ");
            builder.AppendLine("                    </td>");
            builder.AppendLine("                </tr>");
            builder.AppendLine("            </tbody>");
            builder.AppendLine("        </table>");
            builder.AppendLine("        <br>");
            builder.AppendLine("        <hr>");
            builder.AppendLine("        <div class=\"maincontent\">");
            builder.AppendLine("            <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
            builder.AppendLine("                <tbody>");
            builder.AppendLine("                    <tr>");
            builder.AppendLine("                        <td> ");
            builder.AppendLine("                            <font size=\"+1\">");
            builder.AppendLine("                                <b>TERMO DE ESTÁGIO EM HOME OFFICE</b>");
            builder.AppendLine("                            </font>");
            builder.AppendLine("                        </td>");
            builder.AppendLine("                        <td align=\"right\">");
            builder.AppendLine("                            <font size=\"-1\">");
            builder.AppendLine($"                                ");
            builder.AppendLine("                            </font>");
            builder.AppendLine("                        </td>");
            builder.AppendLine("                    </tr>");
            builder.AppendLine("                </tbody>");
            builder.AppendLine("            </table>");
            builder.AppendLine("            <br>");
            builder.AppendLine("            <br>");
            builder.AppendLine("            <br>");
            builder.AppendLine("            <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"message\">");
            builder.AppendLine("                <tbody>");
            builder.AppendLine("                    <tr>");
            builder.AppendLine("                        <td colspan=\"2\">");
            builder.AppendLine("                            <table width=\"100%\" cellpadding=\"12\" cellspacing=\"0\" border=\"0\">");
            builder.AppendLine("                                <tbody>");
            builder.AppendLine("                                    <tr>");
            builder.AppendLine("                                        <td>");
            builder.AppendLine("                                            <font size=\"-1\">");
            builder.AppendLine($"                                                <b>RODRIGO BORGES MACHADO</b>, inscrito no CNPJ sob o número <b>36.638.102/0001-04</b>, localizado na Rua Maria José Durães 150, Apt 105 - Bairro Alto Umuarama, CEP 38405-358, Estado de MG, doravante denominado <b>SUNSALE SYSTEM</b>, declara que o estagiário contratado <b>{input.Nome}</b>, inscrito no CPF <b>{input.Cpf}</b>, matriculada no curso <b>{(input.Tipo.ToUpper().Equals("INFORMATICA") ? "TÉCNICO EM SUPORTE E MANUTENÇÃO EM INFORMÁTICA" : "MEIO AMBIENTE")}</b>, trabalhará de forma remota (Home Office) durante todo o período do seu estágio.");
            builder.AppendLine("                                                <br>");
            builder.AppendLine("                                                <br>");
            builder.AppendLine("                                            </font>");
            builder.AppendLine("                                        </td>");
            builder.AppendLine("                                    </tr>");
            builder.AppendLine("                                    <tr>");
            builder.AppendLine("                                        <td>");
            builder.AppendLine("                                            <div style=\"overflow: hidden;\">");
            builder.AppendLine("                                                <font size=\"-1\">");
            builder.AppendLine("                                                    <div dir=\"ltr\">");
            builder.AppendLine("                                                        _______________________________________________________________________");
            builder.AppendLine("                                                        <br>");
            builder.AppendLine("                                                        <b>SUNSALE SYSTEM</b>");
            builder.AppendLine("                                                        <br>");
            builder.AppendLine("                                                    </div>");
            builder.AppendLine("                                                </font>");
            builder.AppendLine("                                            </div>");
            builder.AppendLine("                                        </td>");
            builder.AppendLine("                                    </tr>");
            builder.AppendLine("                                </tbody>");
            builder.AppendLine("                            </table>");
            builder.AppendLine("                        </td>");
            builder.AppendLine("                    </tr>");
            builder.AppendLine("                </tbody>");
            builder.AppendLine("            </table>");
            builder.AppendLine("            <br>");
            builder.AppendLine("            <br>");
            builder.AppendLine("            <br>");
            builder.AppendLine("            <hr> ");
            builder.AppendLine("        </div>");
            builder.AppendLine("");
            builder.AppendLine("    </div>");
            builder.AppendLine("    <script type=\"text/javascript\" nonce=\"\">");
            builder.AppendLine("        document.body.onload = function() {");
            builder.AppendLine("            document.body.offsetHeight;");
            builder.AppendLine("            window.print()");
            builder.AppendLine("        };");
            builder.AppendLine("    </script>");
            builder.AppendLine("</body>");
            builder.AppendLine("");
            builder.AppendLine("</html>");

            string retorno = $"data:application/json;base64, {Convert.ToBase64String(Encoding.UTF8.GetBytes(builder.ToString()))}";

            return retorno;
        }

        public void Dispose()
        {
        }
    }
}
