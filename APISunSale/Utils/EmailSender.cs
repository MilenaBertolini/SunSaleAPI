using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Domain.Entities;
using Domain.ViewModel;

namespace APISunSale.Utils
{
    public static class EmailSender
    {
        public static bool SendEmail(EmailViewModel email, string remetente, string smtpClient, int porta, string emailCredencial, string senha)
        {
            bool retorno = true;
            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(remetente);
                mail.To.Add(email.Destinatario);
                mail.Subject = email.Assunto;
                mail.Body = email.Texto;
                mail.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    smtp.Host = smtpClient;
                    smtp.EnableSsl = true; // GMail requer SSL
                    smtp.Port = porta;       // porta para SSL
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // modo de envio
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(emailCredencial, senha);

                    smtp.Send(mail);
                    smtp.Dispose();
                }
            }
            catch (SmtpException e)
            {
                retorno = false;
            }
            catch (Exception e)
            {
                retorno = false;
            }

            return retorno;
        }
    }
}
