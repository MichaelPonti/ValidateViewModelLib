using System;
using System.Collections.Generic;
using System.Text;

namespace ValidateViewModelLib
{
	/// <summary>
	/// implementation of the Rule class where a delegate is supplied
	/// for checking whether the rule has been validated or not.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class DelegateRule<T> : Rule<T>
	{
		private Func<T, bool> _f = null;

		public DelegateRule(string propertyName, string error, Func<T, bool> f)
			: base(propertyName, error)
		{
			_f = f ?? throw new ArgumentNullException(nameof(f));
		}


		public override bool ApplyRule(T obj)
		{
			return _f(obj);
		}
	}
}
