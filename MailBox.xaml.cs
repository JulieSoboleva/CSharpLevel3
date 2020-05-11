using System;
using System.IO;
using CodePassword;
using libMailSender;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Documents;
using WpfMailSenderClient.Views;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading;

namespace WpfMailSenderClient
{
    public partial class MailBox : Window
    {
        public static MailBox me = null;
        static bool Modified { get; set; }
        static List<string> attachments = new List<string>();
        static Regex rSomeUseful;
        static OpenFileDialog ofdSelectAttachments = new OpenFileDialog();
        static SaveFileDialog sfdLetter = new SaveFileDialog();
        static OpenFileDialog ofdLetter = new OpenFileDialog();
        static Senders senders = new Senders();

        public MailBox()
        {
            InitializeComponent();
            senders.Add(new Pair("a.aleks@yandex.ru", PasswordClass.Deencrypt("13579")));
            senders.Add(new Pair("bborisov@gmail.com", PasswordClass.Deencrypt("2468")));
            mcbSenders.StrArray = senders.ToArray();
            rSomeUseful = new Regex(@"\S+");
            ofdSelectAttachments.Multiselect = true;
            ofdSelectAttachments.Filter = "Text files (*.txt)|*.txt|Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";
            sfdLetter.Filter = "RTF-файл (*.rtf)|*.rtf";
            ofdLetter.DefaultExt = "*.rtf";
            ofdLetter.Filter = "RTF-файл (*.rtf)|*.rtf";
            me = this;
        }

        private void btnAttachFiles_Click(object sender, RoutedEventArgs e)
        {
            if (ofdSelectAttachments.ShowDialog().Value)
                attachments.AddRange(ofdSelectAttachments.FileNames);
            mcdAttachments.StrArray = attachments.ToArray();
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
                string mailBody = StringFromRichTextBox(rtbBody);
                EmailSenderServiceClass emailSender = new EmailSenderServiceClass(mcbSenders.Text, senders[mcbSenders.Text].Password, tbCaption.Text, mailBody, attachments.ToArray());
                foreach (ListViewItemScheduler scheduler in (DataContext as ViewModel.MailBoxViewModel).SendMails)
                    sc.DatesEmailTexts.Add(scheduler.SendDateTime, mailBody);
                sc.SendEmails(emailSender, (ListCollectionView)(DataContext as ViewModel.MailBoxViewModel).EmailsView);
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
                
                EmailSenderServiceClass mailSender = new EmailSenderServiceClass(mcbSenders.Text, senders[mcbSenders.Text].Password, tbCaption.Text, mailBody, attachments.ToArray());
                foreach (Recipient mail in (DataContext as ViewModel.MailBoxViewModel).EmailsView)
                {
                    Thread thread = new Thread(() => mailSender.SendMail(mail.Email));
                    thread.Start();
                }
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

        private void mcbSenders_btnAddClick(object sender, RoutedEventArgs e)
        {
            AddSender wndNewSender = new AddSender();
            if (wndNewSender.ShowDialog().Value)
            {
                senders.Add(new Pair(wndNewSender.Login, wndNewSender.Password));
                mcbSenders.StrArray = senders.ToArray();
                wndNewSender.Close();
            }
        }

        private void mcbSenders_btnEditClick(object sender, RoutedEventArgs e)
        {
            AddSender wndNewSender = new AddSender(mcbSenders.Text, senders[mcbSenders.Text].Password);
            if (wndNewSender.ShowDialog().Value)
            {
                senders[wndNewSender.Login] = new Pair(wndNewSender.Login, wndNewSender.Password);
                mcbSenders.StrArray = senders.ToArray();
                wndNewSender.Close();
            }
        }

        private void mcbSenders_btnDeleteClick(object sender, RoutedEventArgs e)
        {
            senders.Remove(senders[mcbSenders.Text]);
            mcbSenders.StrArray = senders.ToArray();
        }

        private void mcdAttachments_btnDeleteClick(object sender, RoutedEventArgs e)
        {
            attachments.RemoveAt(mcdAttachments.SelectedComboIndex);
            mcdAttachments.StrArray = attachments.ToArray();
        }
    }
}
