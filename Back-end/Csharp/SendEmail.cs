using System.Net.Mail;
using System.Net;
using System.Text;

MailMessage objEmail = new MailMessage();

objEmail.From = new MailAddress("email@seusite.com.br");
objEmail.ReplyTo = new MailAddress("email@seusite.com.br");
objEmail.To.Add("destinatario@provedor.com.br");
objEmail.Bcc.Add("oculto@provedor.com.br");
objEmail.Priority = MailPriority.Normal;
objEmail.IsBodyHtml = true;
objEmail.Subject = "Assunto";
objEmail.Body = "Conteúdo do email. Se ativar html, pode utilizar cores, fontes, etc.";
objEmail.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
objEmail.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");

SmtpClient objSmtp = new SmtpClient();
objSmtp.Host = "smtp.seuservidor.com.br;"
objSmtp.Credentials = new NetworkCredential("login", "senha");
objSmtp.Send(objEmail);