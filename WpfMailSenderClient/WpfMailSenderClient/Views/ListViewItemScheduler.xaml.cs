using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfMailSenderClient.Views
{
    public partial class ListViewItemScheduler : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<ListViewItemScheduler> _mailsScheduler;
        public event PropertyChangedEventHandler PropertyChanged;
        public string MailText { get; set; }

        private DateTime _sendDateTime;
        public DateTime SendDateTime
        {
            get { return _sendDateTime; }
            set
            {
                _sendDateTime = value;
                OnPropertyChanged(nameof(SendDateTime));
            }
        }

        public ListViewItemScheduler(ObservableCollection<ListViewItemScheduler> mailsScheduler)
        {
            InitializeComponent();
            _mailsScheduler = mailsScheduler;
            _sendDateTime = DateTime.Now;
        }

        public event RoutedEventHandler btnEditMailTextClick;
        private void BtnEditMailText_Click(object sender, RoutedEventArgs e)
            => btnEditMailTextClick?.Invoke(null, null);
            
        private void BtnDelMail_Click(object sender, RoutedEventArgs e)
        {
            _mailsScheduler.Remove(this);
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
