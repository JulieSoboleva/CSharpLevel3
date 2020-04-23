using System;
using System.Net;
using System.Net.Mail;

namespace MailSenderService
{
    public class MailSender : IMailSender
    {
        public bool SendMail(string from, string password, string mailto, 
            string caption, string message, string[] attachFiles)
        {
            try
            {
                using (MailMessage mail = new MailMessage(from, mailto))
                {
                    mail.Subject = caption;
                    mail.Body = message;
                    mail.IsBodyHtml = false;
                    foreach (string attaFile in attachFiles)
                        mail.Attachments.Add(new Attachment(attaFile));

                    using (SmtpClient client = new SmtpClient())
                    {
                        client.Host = "smtp." + from.Split('@')[1];
                        switch (client.Host)
                        {
                            case "smtp.yandex.ru":
                            case "smtp.mail.ru":
                                client.Port = 25;
                                break;
                            case "smtp.gmail.com":
                                client.Port = 587;
                                break;
                            default:
                                throw new Exception("Порт не определён. ");
                        }
                        client.EnableSsl = true;
                        client.Credentials = new NetworkCredential(from, password);
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.Send(mail);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Не удалось отправить письмо: " + e.Message);
            }
        }
    }
}
