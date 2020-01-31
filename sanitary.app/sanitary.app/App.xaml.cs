using System;
using sanitary.app.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sanitary.app
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new AuthorizationPage());
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
