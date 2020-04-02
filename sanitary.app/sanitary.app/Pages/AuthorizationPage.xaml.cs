using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sanitary.app.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AuthorizationPage : ContentPage
	{
		public AuthorizationPage()
		{
			InitializeComponent();
		}

		private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			Navigation.PushAsync(new RegistrationPage());
		}
	}
}