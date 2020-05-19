using System;
using System.Linq;
using libMailSender;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using System.Collections.Generic;
using Common;

namespace WpfMailSenderClient
{
    class SchedulerClass
    {
        Dictionary<DateTime, string> dicDates = new Dictionary<DateTime, string>();
        public Dictionary<DateTime, string> DatesEmailTexts
        {
            get { return dicDates; }
            set
            {
                dicDates = value;
                dicDates = dicDates.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            }
        }

        DispatcherTimer timer = new DispatcherTimer();
        EmailSenderServiceClass emailSender;
        ListCollectionView emails;
        
        public void SendEmails(EmailSenderServiceClass emailSender, ListCollectionView emails)
        {
            this.emailSender = emailSender; 
            this.emails = emails;
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }
       
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (dicDates.Count == 0)
            {
                timer.Stop();
                MessageBox msgBox = new MessageBox("Письма отправлены", "Почтовое уведомление", Brushes.DarkGreen);
                msgBox.ShowDialog();
            }
            else if (dicDates.Keys.First<DateTime>().ToShortTimeString() == DateTime.Now.ToShortTimeString())
            {
                emailSender.strBody = dicDates[dicDates.Keys.First<DateTime>()];
                if (string.IsNullOrEmpty(emailSender.strSubject))
                    emailSender.strSubject = $"Рассылка от {dicDates.Keys.First<DateTime>().ToShortTimeString()} ";
                foreach (Emails mail in emails)
                    emailSender.SendMail(mail.Value);
                dicDates.Remove(dicDates.Keys.First<DateTime>());
            }
        }
    }
}
