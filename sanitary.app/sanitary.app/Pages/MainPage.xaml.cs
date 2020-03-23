using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sanitary.app.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

		private void Button_OnClicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new ProfilePage());
		}

		private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			
		}
	}
}