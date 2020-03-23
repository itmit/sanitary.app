using System;
using sanitary.app.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sanitary.app.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegistrationPage : ContentPage
	{
		public RegistrationPage()
		{
			InitializeComponent();
		}

		private void Button_OnClicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new MenuTabbedView());
		}

		private void Button_OnClicked1(object sender, EventArgs e)
		{
			Navigation.PopAsync();
		}

		private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			Navigation.PushAsync(new PrivatePolicyPage());
		}

		private void CheckBox_OnCheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			RegButton.IsEnabled = true;
		}
	}
}