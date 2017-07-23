//inclua as classes abaixo
using System.Net.Mail;
using System.Net;
using System.Text;

//crio objeto responsável pela mensagem de email
MailMessage objEmail = new MailMessage();

//rementente do email
objEmail.From = new MailAddress("email@seusite.com.br");

//email para resposta(quando o destinatário receber e clicar em responder, vai para:)
objEmail.ReplyTo = new MailAddress("email@seusite.com.br");

//destinatário(s) do email(s). Obs. pode ser mais de um, pra isso basta repetir a linha
//abaixo com outro endereço
objEmail.To.Add("destinatario@provedor.com.br");

//se quiser enviar uma cópia oculta pra alguém, utilize a linha abaixo:
objEmail.Bcc.Add("oculto@provedor.com.br");

//prioridade do email
objEmail.Priority = MailPriority.Normal;

//utilize true pra ativar html no conteúdo do email, ou false, para somente texto
objEmail.IsBodyHtml = true;

//Assunto do email
objEmail.Subject = "Assunto";

//corpo do email a ser enviado
objEmail.Body = "Conteúdo do email. Se ativar html, pode utilizar cores, fontes, etc.";

//codificação do assunto do email para que os caracteres acentuados serem reconhecidos.
objEmail.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");

//codificação do corpo do emailpara que os caracteres acentuados serem reconhecidos.
objEmail.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");

//cria o objeto responsável pelo envio do email
SmtpClient objSmtp = new SmtpClient();

//endereço do servidor SMTP(para mais detalhes leia abaixo do código)
objSmtp.Host = "smtp.seuservidor.com.br;"

//para envio de email autenticado, coloque login e senha de seu servidor de email
//para detalhes leia abaixo do código
objSmtp.Credentials = new NetworkCredential("login", "senha");

//envia o email
objSmtp.Send(objEmail);