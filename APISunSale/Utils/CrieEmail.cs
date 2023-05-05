using Domain.Entities;
using System.Text;
using System.Text.Unicode;

namespace APISunSale.Utils
{
    public static class CrieEmail
    {
        public static string CriaEmailBoasVindas(Usuarios user)
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
            sb.AppendLine("    <p>Seus dados foram registrados e você já pode começar a usar nossos serviços.</p>");
            sb.AppendLine("    <p>Agradecemos pela sua confiança e esperamos que você encontre as respostas para todas as suas perguntas aqui.</p>");
            sb.AppendLine("    <br>");
            sb.AppendLine("    <p>Atenciosamente,</p>");
            sb.AppendLine("    <p>A equipe do Questoesaqui</p>");
            sb.AppendLine("  </body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }
}
