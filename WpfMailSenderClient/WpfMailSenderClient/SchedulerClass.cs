using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;
using libMailSender;

namespace WpfMailSenderClient
{
    class SchedulerClass
    {
        DispatcherTimer timer = new DispatcherTimer();  // таймер 
        EmailSenderServiceClass emailSender;            // экземпляр класса, отвечающего за отправку писем
        DateTime dtSend;                                // дата и время отправки
        IQueryable<Recipient> emails;                   // коллекция email-ов адресатов
        
        /// <summary>
        /// Метод, который превращает строку из текстбокса tbTimePicker в TimeSpan
        /// </summary>
        /// <param name="strSendTime"></param>
        /// <returns></returns>
        public TimeSpan GetSendTime(string strSendTime)
        {
            TimeSpan tsSendTime = new TimeSpan();
            try
            {
                tsSendTime = TimeSpan.Parse(strSendTime);
            }
            catch { }
            return tsSendTime;
        }

        public void SendEmails(DateTime dtSend, EmailSenderServiceClass emailSender, IQueryable<Recipient> emails)
        {
            this.emailSender = emailSender; 
            this.dtSend = dtSend;
            this.emails = emails;
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (dtSend.ToShortTimeString() == DateTime.Now.ToShortTimeString())
            {
                foreach (Recipient mail in emails)
                    emailSender.SendMail(mail.Email);
                
                timer.Stop();
                MessageBox msgBox = new MessageBox("Письмо отправлено", "Почтовое уведомление", Brushes.DarkGreen);
                msgBox.ShowDialog();
            }
        }
    }
}
