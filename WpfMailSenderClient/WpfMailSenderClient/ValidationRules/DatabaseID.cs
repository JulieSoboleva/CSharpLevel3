using System.Globalization;
using System.Windows.Controls;

namespace WpfMailSenderClient.ValidationRules
{
    public class DatabaseID : ValidationRule
    {
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (value is string str)
			{
				if (!int.TryParse(str, out var i)) 
					return new ValidationResult(false, "Идентификатор должен быть числом");
				value = i;
			}

			if (!(value is int id)) 
				return new ValidationResult(false, "Некорректный ввод");

			if (id < 0) 
				return new ValidationResult(false, "Идентификатор должен быть больше нуля");
			
			return ValidationResult.ValidResult;
		}
	}
}
