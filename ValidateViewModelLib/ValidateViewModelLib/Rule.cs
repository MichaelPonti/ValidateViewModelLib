using System;
using System.Collections.Generic;
using System.Text;

namespace ValidateViewModelLib
{
	/// <summary>
	/// Base class for a rule. I am sure that most implementations of this
	/// can use the DelegateRule where a rule function is supplied.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class Rule<T>
	{
		private string _propertyName = null;
		private string _error = null;


		public Rule(string propertyName, string error)
		{
			if (String.IsNullOrWhiteSpace(propertyName))
				throw new ArgumentNullException(nameof(propertyName));
			if (String.IsNullOrWhiteSpace(error))
				throw new ArgumentNullException(nameof(error));

			_propertyName = propertyName;
			_error = error;
		}


		public string PropertyName { get { return _propertyName; } }
		public string Error { get { return _error; } }


		public abstract bool ApplyRule(T obj);
	}
}
