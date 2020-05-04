using System.Linq;

namespace CodePassword
{
    public static class PasswordClass
    {
        public static string Encrypt(string str, int key = 78) => 
            new string(str.Select(c => (char)(c + key)).ToArray());

        public static string Deencrypt(string str, int key = 78) =>
            new string(str.Select(c => (char)(c - key)).ToArray());
    }
}
