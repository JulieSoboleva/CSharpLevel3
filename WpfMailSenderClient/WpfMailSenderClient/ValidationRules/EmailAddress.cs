using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace WpfMailSenderClient.ValidationRules
{
    public class EmailAddress : ValidationRule
    {
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (!(value is string address)) 
				return new ValidationResult(false, "Некорректные данные");

			if (!Regex.IsMatch(address, @"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;,.]{0,1}\s*)+$"))
				return new ValidationResult(false, "Введён некорректный адрес");

			return ValidationResult.ValidResult;
		}
	}
}
