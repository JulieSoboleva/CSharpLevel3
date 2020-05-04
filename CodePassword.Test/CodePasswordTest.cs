using System;
using System.Diagnostics;
using System.Threading;
using CodePassword;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodePassword.Test
{
    [TestClass]
    public class CodePasswordTest : IDisposable
    {
        [TestInitialize]
        public void Init()
        {
            Debug.WriteLine($"TestInitialize {DateTime.Now}");
        }

        [TestMethod]
        public void Encrypt_abc_bcd()
        {
            string input_str = "abc";
            int key = 1;
            string expected_result = "bcd";

            string actual_result = PasswordClass.Encrypt(input_str, key);

            Assert.AreEqual(expected_result, actual_result, "Ошибка кодирования строки");
        }

        [TestCleanup]
        public void Cleanup()
        {
            Debug.WriteLine($"TestCleanup {DateTime.Now}");
        }

        [TestMethod, Timeout(40), Description("Это метод тестирует...")]
        public void Encrypt_empty_empty()
        {
            string input = "";
            int key = 1;
            string expected = "";
            Thread.Sleep(20);
            string actual = PasswordClass.Encrypt(input, key);

            Assert.AreEqual(expected, actual);
        }

        public void Dispose()
        {
            Debug.WriteLine($"Dispose {DateTime.Now}");
        }
    }
}
