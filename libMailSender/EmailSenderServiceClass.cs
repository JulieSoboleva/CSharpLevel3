using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace libMailSender
{
    public class EmailSenderServiceClass
    {
        #region vars
        private string strLogin;        // email, c которого будет рассылаться почта
        private string strPassword;     // пароль к email, с которого будет рассылаться почта
        private string strSmtp;         // smtp-server
        private int smtpPort;           // порт для smtp-server
        private string strBody;         // текст письма для отправки
        private string strSubject;      // тема письма для отправки
        private string[] attachFiles;   // прикреплённые файлы
        private Regex rMails = new Regex(@"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;,.]{0,1}\s*)+$");
        #endregion

        public EmailSenderServiceClass(string sLogin, string sPassword, 
            string caption, string message, string[] attachments)
        {
            if (string.IsNullOrEmpty(sLogin) || !rMails.IsMatch(sLogin))
                throw new Exception("Не задан адрес отправителя");
            strLogin = sLogin;

            if (string.IsNullOrEmpty(sPassword))
                throw new Exception("Не введён пароль");
            strPassword = sPassword;

            strSmtp = "smtp." + sLogin.Split('@')[1];
            switch (strSmtp)
            {
                case "smtp.yandex.ru":
                case "smtp.mail.ru":
                    smtpPort = 25;
                    break;
                case "smtp.gmail.com":
                    smtpPort = 587;
                    break;
                default:
                    throw new Exception("Порт не определён. ");
            }

            strSubject = caption;
            strBody = message;
            attachFiles = attachments;
        }

        /// <summary>
        /// Отправка email одному адресату
        /// </summary>
        /// <param name="mailto">email получателя</param>
        public void SendMail(string mailto)
        {
            using (MailMessage mm = new MailMessage(strLogin, mailto))
            {
                mm.Subject = strSubject;
                mm.Body = strBody;
                mm.IsBodyHtml = false;
                foreach (string attaFile in attachFiles)
                    mm.Attachments.Add(new Attachment(attaFile));

                using (SmtpClient sc = new SmtpClient(strSmtp, smtpPort))
                {
                    sc.EnableSsl = true;
                    sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                    sc.UseDefaultCredentials = false;
                    sc.Credentials = new NetworkCredential(strLogin, strPassword);
                    try
                    {
                        sc.Send(mm);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Невозможно отправить письмо\n" + ex.Message);
                    }
                }
            }
        }
    }
}

