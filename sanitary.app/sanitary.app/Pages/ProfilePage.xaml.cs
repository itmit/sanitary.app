using System;
using sanitary.app.ViewModels;
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
			BindingContext = new ProfileViewModel();
		}

		private async void EntryPass_OnCompleted(object sender, EventArgs e)
		{
			await Application.Current.MainPage.DisplayAlert("Внимание", "Пароль успешно изменен!", "ОК");
			EditPass.IsVisible = true;
			EntryPass.IsVisible = false;
		}

		private void EntryPass_OnUnfocused(object sender, FocusEventArgs e)
		{
			EditPass.IsVisible = true;
			EntryPass.IsVisible = false;
		}

		private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			EditPass.IsVisible = false;
			EntryPass.IsVisible = true;
		}
	}
}