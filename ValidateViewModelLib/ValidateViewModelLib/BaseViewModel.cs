using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ValidateViewModelLib
{
	public abstract class BaseViewModel<T> : INotifyPropertyChanged, INotifyDataErrorInfo
		where T : BaseViewModel<T>
	{
		public event PropertyChangedEventHandler PropertyChanged;

#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
		protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
		{
			/// if the value doesn't change, don't invoke the
			/// data binding system
			if (Equals(field, value))
				return false;

			field = value;
			RaisePropertyChanged(propertyName);
			return true;
		}


		protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
		{
			PropertyChanged?.Invoke(this, args);
		}





		protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			/// update the property in the data binding system
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

			/// apply the rules to the property
			if (String.IsNullOrWhiteSpace(propertyName))
				ApplyRules();
			else
				ApplyRules(propertyName);

			/// raise the HasErrors property within the databinding system
			OnPropertyChanged(new PropertyChangedEventArgs(nameof(HasErrors)));
		}





		public bool HasErrors => (Errors.Count > 0);

		event EventHandler<DataErrorsChangedEventArgs> INotifyDataErrorInfo.ErrorsChanged
		{
			add { _errorsChanged += value; }
			remove { _errorsChanged -= value; }
		}


		public IEnumerable GetErrors(string propertyName)
		{
			IEnumerable ret;

			if (String.IsNullOrWhiteSpace(propertyName))
			{
				List<string> allErrors = new List<string>();
				foreach (var item in Errors)
					allErrors.AddRange(item.Value);
				ret = allErrors;
			}
			else
			{
				if (Errors.ContainsKey(propertyName))
					ret = Errors[propertyName];
				else
					ret = new List<string>();
			}

			return ret;
		}



		public static RuleCollection<T> Rules { get; private set; } =
			new RuleCollection<T>();

		private Dictionary<string, List<string>> _errors = null;
		public Dictionary<string, List<string>> Errors
		{
			get
			{
				if (_errors == null)
				{
					_errors = new Dictionary<string, List<string>>();
					ApplyRules();
				}

				return _errors;
			}
		}


		private event EventHandler<DataErrorsChangedEventArgs> _errorsChanged;


		public void ApplyRules(string propertyName)
		{
			List<string> propertyErrors = Rules.ApplyRules((T) this, propertyName);
			if (propertyErrors != null && propertyErrors.Count > 0)
			{
				if (Errors.ContainsKey(propertyName))
					Errors[propertyName].Clear();
				else
					Errors[propertyName] = new List<string>();

				Errors[propertyName].AddRange(propertyErrors);
			}
			else
			{
				Errors.Remove(propertyName);
			}


			OnErrorsChanged(propertyName);
		}


		public void ApplyRules()
		{
			foreach(var r in Rules)
			{
				ApplyRules(r.PropertyName);
			}
		}



		protected virtual void OnErrorsChanged([CallerMemberName] string propertyName = null)
		{
			_errorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}



		public IObservable<string> WhenErrorsChanged
		{
			get
			{
				return Observable.FromEventPattern<DataErrorsChangedEventArgs>(
					h => _errorsChanged += h,
					h => _errorsChanged -= h)
					.Select(x => x.EventArgs.PropertyName);
			}
		}

	}
}
