using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ValidateViewModelLib
{
	/// <summary>
	/// collection of rules associated with a view model.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class RuleCollection<T> : Collection<Rule<T>>
	{
		private Collection<Rule<T>> _rules = new Collection<Rule<T>>();


		public List<string> ApplyRules(T obj, string propertyName)
		{
			List<string> ret = new List<string>();
			foreach(Rule<T> rule in this)
			{
				/// check the rule if the property name matches or isn't specified
				if (String.IsNullOrWhiteSpace(propertyName) || rule.PropertyName.Equals(propertyName))
				{
					if (!rule.ApplyRule(obj))
					{
						ret.Add(rule.Error);
					}
				}
			}

			return ret;
		}


	}
}
