using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateVmSample
{
	public class MainWindowViewModel : ValidateViewModelLib.BaseViewModel<MainWindowViewModel>
	{
		static MainWindowViewModel()
		{
			Rules.Add(new ValidateViewModelLib.DelegateRule<MainWindowViewModel>(nameof(Name), "name can't be blank", x => !String.IsNullOrWhiteSpace(x.Name)));
			Rules.Add(new ValidateViewModelLib.DelegateRule<MainWindowViewModel>("Name", "Name cna't contain 'x'", x => (x.Name != null && !x.Name.Contains("x"))));

			Rules.Add(new ValidateViewModelLib.DelegateRule<MainWindowViewModel>("DecimalField", "Decimal value can't be less than 4 or greater than 20", x => (x.DecimalField.HasValue && (x.DecimalField >= 4 && x.DecimalField <= 20))));
		}




		private string _name = null;
		public string Name
		{
			get { return _name; }
			set { SetProperty<string>(ref _name, value); }
		}


		private decimal? _decimalField = null;
		public decimal? DecimalField
		{
			get { return _decimalField; }
			set { SetProperty<decimal?>(ref _decimalField, value); }
		}
	}
}
