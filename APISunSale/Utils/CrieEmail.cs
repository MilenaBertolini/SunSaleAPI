using Domain.Entities;
using System.Text;
using System.Text.Unicode;
using static Data.Helper.EnumeratorsTypes;

namespace APISunSale.Utils
{
    public static class CrieEmail
    {
        public static string CriaEmailBoasVindas(Usuarios user, string guid)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("  <head>");
            sb.AppendLine("    <meta charset=\"utf - 8\">");
            sb.AppendLine("    <title>Cadastro realizado com sucesso!</title>");
            sb.AppendLine("  </head>");
            sb.AppendLine("  <body>");
            sb.AppendLine($"    <h1>Parabéns {user.Nome?.Split(' ')?[0]}!</h1>");
            sb.AppendLine("    <p>Você acaba de se cadastrar no nosso site Questoesaqui.</p>");
            sb.AppendLine($"    <p>Seus dados foram registrados e basta você acessar esse link para começar a usar nossos serviços. Acesse: <a href=\"https://www.questoesaqui.com/valida/{guid}\">https://www.questoesaqui.com/valida/{guid}</a></p>");
            sb.AppendLine("    <p>Agradecemos pela sua confiança e esperamos que você encontre as respostas para todas as suas perguntas aqui. Acesse: <a href=\"https://www.questoesaqui.com/login\">QuestoesAqui</a></p>");
            sb.AppendLine("    <br>");
            sb.AppendLine("    <p>Atenciosamente,</p>");
            sb.AppendLine("    <p>A equipe do Questoesaqui</p>");
            sb.AppendLine("  </body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        public static string CriaEmailBoasVindasCrudForms(UsuariosCrudForms user)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("  <head>");
            sb.AppendLine("    <meta charset=\"utf - 8\">");
            sb.AppendLine("    <title>Cadastro realizado com sucesso!</title>");
            sb.AppendLine("  </head>");
            sb.AppendLine("  <body>");
            sb.AppendLine($"    <h1>Parabéns {user.Nome?.Split(' ')?[0]}!</h1>");
            sb.AppendLine("    <p>Você acaba de se cadastrar no nosso site CrudForms.</p>");
            sb.AppendLine("    <p>Seus dados foram registrados e você já pode começar a usar nossos serviços.</p>");
            sb.AppendLine("    <p>Agradecemos pela sua confiança e esperamos que você encontre as respostas para todas as suas perguntas aqui. Acesse: <a href=\"https://www.crudforms.com/login\">CrudForms</a></p>");
            sb.AppendLine("    <br>");
            sb.AppendLine("    <p>Atenciosamente,</p>");
            sb.AppendLine("    <p>A equipe do Questoesaqui</p>");
            sb.AppendLine("  </body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        public static string CriaEmailRecupereSenha(string guid, TipoSistema tipo)
        {
            string url = tipo == TipoSistema.QuestoesAqui ? "https://www.questoesaqui.com" : "https://www.crudforms.com";
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"pt-br\">");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset=\"UTF-8\">");
            sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            sb.AppendLine($"    <title>Recuperação de senha - {(tipo == TipoSistema.QuestoesAqui ? "Questões Aqui" : "CrudForms")}</title>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body style=\"font-family: Arial, sans-serif; font-size: 16px; line-height: 1.5; color: #333;\">");
            sb.AppendLine("    <h1>Recuperação de senha</h1>");
            sb.AppendLine("    <p>Olá,</p>");
            sb.AppendLine("    <p>Recebemos uma solicitação para recuperar a senha da sua conta. Para definir uma nova senha, clique no botão abaixo:</p>");
            sb.AppendLine($"    <p style=\"text-align: center;\"><a href=\"{url}/resetpass/{guid}\" style=\"display: inline-block; padding: 10px 20px; background-color: #007bff; color: #fff; text-decoration: none; border-radius: 5px;\">Recuperar senha</a></p>");
            sb.AppendLine("    <p>Se você não solicitou a recuperação de senha, basta ignorar este e-mail.</p>");
            sb.AppendLine("    <p>Obrigado,</p>");
            sb.AppendLine("    <p>A equipe de suporte.</p>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        public static string ConfirmaAlteracaoPass(string userName, TipoSistema tipo)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("	<meta charset=\"UTF-8\">");
            sb.AppendLine("	<title>Confirmação de alteração de senha</title>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body style=\"font-family: Arial, sans-serif; font-size: 16px; line-height: 1.5; color: #333;\">");
            sb.AppendLine("	<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" align=\"center\" width=\"600\" style=\"border-collapse: collapse;\">");
            sb.AppendLine("		<tr>");
            sb.AppendLine("			<td style=\"padding: 30px;\">");
            sb.AppendLine("				<h1 style=\"font-size: 24px; margin-bottom: 30px;\">Confirmação de alteração de senha</h1>");
            sb.AppendLine($"				<p>Olá {userName},</p>");
            sb.AppendLine("				<p>Este e-mail é para confirmar que a senha de sua conta foi alterada com sucesso.</p>");
            sb.AppendLine("				<p>Se você não realizou esta alteração, por favor entre em contato conosco imediatamente.</p>");
            sb.AppendLine("				<p>Atenciosamente,</p>");
            sb.AppendLine($"				<p>SunSale System - {(tipo == TipoSistema.QuestoesAqui ? "Questoes Aqui" : "CrudForms")}</p>");
            sb.AppendLine("			</td>");
            sb.AppendLine("		</tr>");
            sb.AppendLine("		<tr>");
            sb.AppendLine("			<td style=\"background-color: #f5f5f5; padding: 20px; text-align: center;\">");
            sb.AppendLine("				<p style=\"margin: 0;\">&copy; 2023 SunSale System. Todos os direitos reservados.</p>");
            sb.AppendLine("			</td>");
            sb.AppendLine("		</tr>");
            sb.AppendLine("	</table>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        public static string CriaEmailUsuarioValidado(Usuarios user)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("  <head>");
            sb.AppendLine("    <meta charset=\"utf - 8\">");
            sb.AppendLine("    <title>Cadastro validado com sucesso!</title>");
            sb.AppendLine("  </head>");
            sb.AppendLine("  <body>");
            sb.AppendLine($"    <h1>Parabéns {user.Nome?.Split(' ')?[0]}!</h1>");
            sb.AppendLine("    <p>Você acaba de validar seu cadastro no nosso site Questoesaqui.</p>");
            sb.AppendLine($"    <p>Seus dados foram validados e você já pode começar a usar nossos serviços.</p>");
            sb.AppendLine("    <p>Agradecemos pela sua confiança e esperamos que você encontre as respostas para todas as suas perguntas aqui. Acesse: <a href=\"https://www.questoesaqui.com/login\">QuestoesAqui</a></p>");
            sb.AppendLine("    <br>");
            sb.AppendLine("    <p>Atenciosamente,</p>");
            sb.AppendLine("    <p>A equipe do Questoesaqui</p>");
            sb.AppendLine("  </body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }
}
