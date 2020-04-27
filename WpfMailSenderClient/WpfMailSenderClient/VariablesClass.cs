using CodePassword;
using System.Collections.Generic;

namespace WpfMailSenderClient
{
    public static class VariablesClass
    {
        public static Dictionary<string, string> Senders { get; } = new Dictionary<string, string>
        {
            {"a.aleks@yandex.ru", PasswordClass.Deencrypt("13579")},
            {"bborisov@gmail.com", PasswordClass.Deencrypt("2468")}
        };
    }
}
