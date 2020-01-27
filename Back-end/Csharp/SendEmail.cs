SmtpClient client = new SmtpClient("mysmtpserver");
client.UseDefaultCredentials = false;
client.Credentials = new NetworkCredential("username", "password");
 
MailMessage mailMessage = new MailMessage();
mailMessage.From = new MailAddress("whoever@me.com");
mailMessage.To.Add("receiver@me.com");
mailMessage.Body = "body";
mailMessage.Subject = "subject";
client.Send(mailMessage);