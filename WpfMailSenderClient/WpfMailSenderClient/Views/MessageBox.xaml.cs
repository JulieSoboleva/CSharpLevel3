using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfMailSenderClient
{
    public partial class MessageBox : Window
    {
        public MessageBox(string message, string caption, Brush msgBrush, bool hasCancel=false)
        {
            InitializeComponent();
            Title = caption;
            
            TextBlock tMessage = new TextBlock();
            tMessage.Text = message;
            tMessage.Foreground = msgBrush;
            tMessage.FontSize = 16.0;
            tMessage.TextWrapping = TextWrapping.Wrap;
            tMessage.HorizontalAlignment = HorizontalAlignment.Center;
            tMessage.VerticalAlignment = VerticalAlignment.Top;
            tMessage.Margin = new Thickness(0, 20, 0, 0);
            msgPanel.Children.Add(tMessage);
            Button btnOK = new Button();
            btnOK.Content = "OK";
            btnOK.FontSize = 14.0;
            btnOK.VerticalAlignment = VerticalAlignment.Bottom;
            btnOK.Height = 30.0;
            btnOK.Width = 100.0;
            if (!hasCancel)
            {
                btnOK.HorizontalAlignment = HorizontalAlignment.Center;
                btnOK.Margin = new Thickness(0, 20, 0, 20);
            }
            else
            {
                btnOK.HorizontalAlignment = HorizontalAlignment.Left;
                btnOK.Margin = new Thickness(20, 20, 0, 20);
            }
            btnOK.Click += delegate { DialogResult = true; Close(); };
            msgPanel.Children.Add(btnOK);

            if (!hasCancel) return;

            btnOK = new Button();
            btnOK.Content = "Cancel";
            btnOK.FontSize = 14.0;
            btnOK.VerticalAlignment = VerticalAlignment.Bottom;
            btnOK.Height = 30.0;
            btnOK.Width = 100.0;
            btnOK.HorizontalAlignment = HorizontalAlignment.Right;
            btnOK.Margin = new Thickness(0, 20, 20, 20);
            btnOK.Click += delegate { DialogResult = false; Close(); };
            msgPanel.Children.Add(btnOK);
        }
    }
}
