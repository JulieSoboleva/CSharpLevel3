using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WpfMailSenderClient
{
    public class Senders : ObservableCollection<Pair>
    {
       public Pair this[string Login]
        {
            get
            {
                foreach (Pair p in this)
                    if (p.Login == Login)
                        return p;
                return null;
            }
            set
            {
                foreach (Pair p in this)
                    if (p.Login == Login)
                        throw new ArgumentException("Такой email уже есть в списке");
                Add(new Pair(Login, ""));
            }
        }

        public string[] ToArray()
        {
            string[] result = new string[this.Count];
            for (int i = 0; i < this.Count; i++)
                result[i] = this[i].Login;
            return result;
        }
    }

    public class Pair : INotifyPropertyChanged, IEquatable<Pair>
    {
        private string login;
        public string Login
        {
            get { return login; }
            set
            {
                if (login != value)
                {
                    login = value;
                    NotifyPropertyChanged("Login");
                }
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    NotifyPropertyChanged("Password");
                }
            }
        }

        public Pair(string _login, string _password) 
        {
            Login = _login;
            Password = _password;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public bool Equals(Pair other)
        {
            return Login == other.Login;
        }
    }
}
