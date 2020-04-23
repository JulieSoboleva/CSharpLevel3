using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace WpfMailSenderClient
{
    public partial class MailBox : Window
    {
        static ServiceReference.MailSenderClient client;
        static string[] attachments;
        static Regex rMails, rSomeUseful;

        public MailBox()
        {
            InitializeComponent();
            client = new ServiceReference.MailSenderClient();
            rMails = new Regex(@"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;,.]{0,1}\s*)+$");
            rSomeUseful = new Regex(@"\S+");
        }

        private void btnAttachFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofdSelectAttachments = new OpenFileDialog();
            ofdSelectAttachments.Multiselect = true;
            ofdSelectAttachments.Filter = "Text files (*.txt)|*.txt|Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";
            if (ofdSelectAttachments.ShowDialog().Value)
                attachments = ofdSelectAttachments.FileNames;
        }

        private string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return textRange.Text;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            MessageBox msgBox;
            try
            {
                if (string.IsNullOrEmpty(tbMailFrom.Text) || !rMails.IsMatch(tbMailFrom.Text)) 
                    throw new Exception("Не задан адрес отправителя");
                if (string.IsNullOrEmpty(pwBox.Password)) 
                    throw new Exception("Не введён пароль");
                if (string.IsNullOrEmpty(tbMailTo.Text) || !rMails.IsMatch(tbMailTo.Text)) 
                    throw new Exception("Не указано ни одного адресата");
                if (string.IsNullOrEmpty(tbCaption.Text) || !rSomeUseful.IsMatch(tbCaption.Text))
                {
                    msgBox = new MessageBox("Отправить письмо без темы?", "Подтверждение действий", Brushes.DarkBlue, true);
                    if (!msgBox.ShowDialog().Value)
                        return;
                }
                string mailBody = StringFromRichTextBox(rtbBody);
                if (string.IsNullOrEmpty(mailBody) || !rSomeUseful.IsMatch(mailBody))
                {
                    msgBox = new MessageBox("Отправить пустое письмо?", "Подтверждение действий", Brushes.DarkBlue, true);
                    if (!msgBox.ShowDialog().Value)
                        return;
                }

                if (client.SendMail(tbMailFrom.Text, pwBox.Password, tbMailTo.Text, tbCaption.Text, mailBody, attachments))
                {
                    msgBox = new MessageBox("Письмо отправлено", "Почтовое уведомление", Brushes.DarkGreen);
                    msgBox.ShowDialog();
                }
            }
            catch (Exception exn)
            {
                msgBox = new MessageBox(exn.Message, "Сообщение об ошибке", Brushes.Red);
                msgBox.ShowDialog();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            client?.Close();
        }
    }
}
