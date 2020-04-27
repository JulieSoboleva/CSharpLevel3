using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using libMailSender;

namespace WpfMailSenderClient
{
    public partial class MailBox : Window
    {
        static bool Modified { get; set; }
        static string[] attachments;
        static Regex rSomeUseful;
        static OpenFileDialog ofdSelectAttachments = new OpenFileDialog();
        static SaveFileDialog sfdLetter = new SaveFileDialog();
        static OpenFileDialog ofdLetter = new OpenFileDialog();

        public MailBox()
        {
            InitializeComponent();
            mcbSenders.CbSourse = VariablesClass.Senders;
            rSomeUseful = new Regex(@"\S+");
            ofdSelectAttachments.Multiselect = true;
            ofdSelectAttachments.Filter = "Text files (*.txt)|*.txt|Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";
            sfdLetter.Filter = "RTF-файл (*.rtf)|*.rtf";
            ofdLetter.DefaultExt = "*.rtf";
            ofdLetter.Filter = "RTF-файл (*.rtf)|*.rtf";
        }

        private void btnAttachFiles_Click(object sender, RoutedEventArgs e)
        {
            if (ofdSelectAttachments.ShowDialog().Value)
                attachments = ofdSelectAttachments.FileNames;
            ModComboBox.StrArray = attachments;
        }

        private string StringFromRichTextBox(RichTextBox rtb)
        {
            return new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd).Text;
        }

        private void Exit_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void New_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            rtbBody.Document.Blocks.Clear();
            Title = "Новое письмо";
            sfdLetter.FileName = "";
            Modified = false;
        }

        private void Open_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (TextSaved())
            {
                ofdLetter.FileName = "";
                if (ofdLetter.ShowDialog() != true)
                    return;
                
                TextRange tr = new TextRange(rtbBody.Document.ContentStart, rtbBody.Document.ContentEnd);
                string path = ofdLetter.FileName;
                using (FileStream fs = File.Open(path, FileMode.Open))
                {
                    tr.Load(fs, DataFormats.Rtf);
                }
                Title = "Редактор писем - " + path;
                sfdLetter.FileName = path;
                Modified = false;
            }
        }

        private void SaveToFile(string path)
        {
            TextRange tr = new TextRange(rtbBody.Document.ContentStart, rtbBody.Document.ContentEnd);
            using (FileStream fs = File.Create(path))
            {
                tr.Save(fs, DataFormats.Rtf);
            }
            Modified = false;
        }

        private void Save_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            string path = sfdLetter.FileName;
            if (string.IsNullOrEmpty(path))
                SaveAs_Executed(null, null);
            else
                SaveToFile(path);
        }

        private void SaveAs_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var oldPath = sfdLetter.FileName;
            sfdLetter.FileName = Path.GetFileName(oldPath);

            if (sfdLetter.ShowDialog() != true)
            {
                sfdLetter.FileName = oldPath;
                return;
            }
            string path = sfdLetter.FileName;
            SaveToFile(path);
            Title = "Редактор писем - " + path;
        }

        private bool TextSaved()
        {
            if (Modified)
            {
                MessageBox msgBox = new MessageBox("Сохранить изменения в файле?", "Подтверждение действий", Brushes.DarkBlue, true);
                if (msgBox.ShowDialog().Value)
                {
                    Save_Executed(null, null);
                    return !Modified;
                }
                else
                    return false;
            }
            return true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !TextSaved();
        }

        private void btnClock_Click(object sender, RoutedEventArgs e)
        {
            tcMain.SelectedItem = tabPlanner;
        }

        private void btnSendLater_Click(object sender, RoutedEventArgs e)
        {
            MessageBox msgBox;
            try
            {
                SchedulerClass sc = new SchedulerClass();
                TimeSpan tsSendTime = sc.GetSendTime(tpTimePicker.Text);
                if (tsSendTime == new TimeSpan())
                    throw new Exception("Некорректный формат даты");
                    
                DateTime dtSendDateTime = (cldSchedulDateTimes.SelectedDate ?? DateTime.Today).Add(tsSendTime);
                    throw new Exception("Дата и время отправки писем не могут быть раньше, чем настоящее время");

                //string mailBody = StringFromRichTextBox(rtbBody);
                //EmailSenderServiceClass emailSender = new EmailSenderServiceClass(cbSenderSelect.Text, cbSenderSelect.SelectedValue.ToString(),
                //    tbCaption.Text, mailBody, attachments);
                //sc.SendEmails(dtSendDateTime, emailSender, (IQueryable<Recipient>)dgEmails.ItemsSource);
            }
            catch (Exception exn)
            {
                msgBox = new MessageBox(exn.Message, "Сообщение об ошибке", Brushes.Red);
                msgBox.ShowDialog();
            }
        }

        private void btnSendNow_Click(object sender, RoutedEventArgs e)
        {
            MessageBox msgBox;
            try
            {
               // KeyValuePair<string, string> item = (KeyValuePair<string, string>)mcbSenders.sele //cbSenderSelect.SelectionBoxItem;
               
                if (string.IsNullOrEmpty(tbCaption.Text) || !rSomeUseful.IsMatch(tbCaption.Text))
                {
                    msgBox = new MessageBox("Отправить письмо без темы?", "Подтверждение действий", Brushes.DarkBlue, true);
                    if (!msgBox.ShowDialog().Value)
                    {
                        tcMain.SelectedItem = tabLetterEditor;
                        return;
                    }
                }
                string mailBody = StringFromRichTextBox(rtbBody);
                if (string.IsNullOrEmpty(mailBody) || !rSomeUseful.IsMatch(mailBody))
                {
                    msgBox = new MessageBox("Отправить пустое письмо?", "Подтверждение действий", Brushes.DarkBlue, true);
                    if (!msgBox.ShowDialog().Value)
                    {
                        tcMain.SelectedItem = tabLetterEditor;
                        return;
                    }
                }

                
                EmailSenderServiceClass mailSender = new EmailSenderServiceClass(mcbSenders.Text, mcbSenders.Value,
                    tbCaption.Text, mailBody, attachments);

                foreach (Recipient mail in (IQueryable<Recipient>)dgEmails.ItemsSource)
                    mailSender.SendMail(mail.Email);

                msgBox = new MessageBox("Письмо отправлено", "Почтовое уведомление", Brushes.DarkGreen);
                msgBox.ShowDialog();
            }
            catch (Exception exn)
            {
                msgBox = new MessageBox(exn.Message, "Сообщение об ошибке", Brushes.Red);
                msgBox.ShowDialog();
            }
        }

        private void TabSwitcherControl_Previous(object sender, RoutedEventArgs e)
        {
            if (tcMain.SelectedIndex == 0)
            {
                tscTabSwitcher.IsHideBtnPrevious = true;
                return;
            }
            tcMain.SelectedIndex--;
            tscTabSwitcher.IsHideBtnPrevious = false;
            tscTabSwitcher.IsHideBtnNext = false;
        }

        private void TabSwitcherControl_Next(object sender, RoutedEventArgs e)
        {
            if (tcMain.SelectedIndex == tcMain.Items.Count - 1)
            {
                tscTabSwitcher.IsHideBtnNext = true;
                return;
            }
            tcMain.SelectedIndex++;
            tscTabSwitcher.IsHideBtnPrevious = false;
            tscTabSwitcher.IsHideBtnNext = false;
        }

        private void rtbBody_TextChanged(object sender, TextChangedEventArgs e)
        {
            Modified = true;
        }
    }
}
