using Microsoft.Practices.Unity;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ValidateVmSample
{
	class Bootstrapper : UnityBootstrapper
	{
		protected override DependencyObject CreateShell()
		{
			var w = Container.Resolve<MainWindow>();
			return w;
		}


		protected override void InitializeShell()
		{
			Application.Current.MainWindow.Show();
		}
	}
}
