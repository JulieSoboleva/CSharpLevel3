using System.Windows;

namespace WpfMailSenderClient.Views
{
    public partial class AddSender : Window
    {
        public string Login { get; private set; }
        public string Password { get; private set; }

        public AddSender()
        {
            InitializeComponent();
        }

        public AddSender(string login, string password)
        {
            InitializeComponent();
            tbLogin.Text = login;
            pwPassword.Password = password;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbLogin.Text) && string.IsNullOrEmpty(pwPassword.Password)) 
                Close();
            else
            {
                Login = tbLogin.Text;
                Password = pwPassword.Password;
                DialogResult = true;
                Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
