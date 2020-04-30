using mvvmCalculator.Model;

namespace mvvmCalculator.ViewModel
{
    public class CalculatorViewModel : ViewModelBase
    {
        private readonly Calculator _calculator;
		private string _result;

		public CalculatorViewModel()
        {
            _calculator = new Calculator();
        }

        public double A
        {
            get => _calculator.A;
            set
            {
                _calculator.A = value;
            }
        }

		public double B
		{
			get => _calculator.B;
			set
			{
				_calculator.B = value;
			}
		}

		public string Result
		{
			get => _result;
			set
			{
				_result = value;
				OnPropertyChanged(nameof(Result));
			}
		}

		private void CalcAdd()
		{
			Result = _calculator.Add().ToString();
		}

		private void CalcSub()
		{
			Result = _calculator.Sub().ToString();
		}

		private void CalcMul()
		{
			Result = _calculator.Mul().ToString();
		}

		private void CalcDel()
		{
			Result = _calculator.Del().ToString();
		}

		private void CalcPow()
		{
			Result = _calculator.Power().ToString();
		}

		private void CalcSqrt()
		{
			Result = _calculator.Sqrt().ToString();
		}

		#region AddCommand
		private RelayCommand _addCommand;
		public RelayCommand AddCommand => _addCommand ?? (_addCommand = new RelayCommand(ExecuteAddCommand));
		public void ExecuteAddCommand(object parameter)
		{
			CalcAdd();
		}
		#endregion

		#region SubCommand
		private RelayCommand _subCommand;
		public RelayCommand SubCommand => _subCommand ?? (_subCommand = new RelayCommand(ExecuteSubCommand));
		public void ExecuteSubCommand(object parameter)
		{
			CalcSub();
		}
		#endregion

		#region MulCommand
		private RelayCommand _mulCommand;
		public RelayCommand MulCommand => _mulCommand ?? (_mulCommand = new RelayCommand(ExecuteMulCommand));
		public void ExecuteMulCommand(object parameter)
		{
			CalcMul();
		}
		#endregion

		#region DelCommand
		private RelayCommand _delCommand;
		public RelayCommand DelCommand => _delCommand ?? (_delCommand = new RelayCommand(ExecuteDelCommand, CanDelCommand));
		public void ExecuteDelCommand(object parameter)
		{
			CalcDel();
		}

		public bool CanDelCommand(object parameter) => B != 0;
		#endregion

		#region PowCommand
		private RelayCommand _powCommand;
		public RelayCommand PowCommand => _powCommand ?? (_powCommand = new RelayCommand(ExecutePowCommand));
		public void ExecutePowCommand(object parameter)
		{
			CalcPow();
		}
		#endregion

		#region SqrtCommand
		private RelayCommand _sqrtCommand;
		public RelayCommand SqrtCommand => _sqrtCommand ?? (_sqrtCommand = new RelayCommand(ExecuteSqrtCommand, CanSqrtCommand));
		public void ExecuteSqrtCommand(object parameter)
		{
			CalcSqrt();
		}

		public bool CanSqrtCommand(object parameter) => A > 0;
		#endregion
	}
}
