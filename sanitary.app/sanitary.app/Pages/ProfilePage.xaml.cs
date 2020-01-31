using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sanitary.app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
		public ProfilePage()
		{
			InitializeComponent();
		}

		private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			EditPass.IsVisible = false;
			EntryPass.IsVisible = true;
		}

		private void TapGestureRecognizer_OnTapped1(object sender, EventArgs e)
		{
			Navigation.PopAsync();
		}

		private async void EntryPass_OnCompleted(object sender, EventArgs e)
		{
			await Application.Current.MainPage.DisplayAlert("Внимание!", "Ваш пароль успешно изменен!", "ОК");
			EditPass.IsVisible = true;
			EntryPass.IsVisible = false;
		}

		private void EntryPass_OnUnfocused(object sender, FocusEventArgs e)
		{
			EditPass.IsVisible = true;
			EntryPass.IsVisible = false;
		}
	}
}