using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sanitary.app.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPositionsPage : ContentPage
    {
        public ListPositionsPage()
        {
            InitializeComponent();
        }

		private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
		{
			Navigation.PushAsync(new CardPositionPage());
		}
	}
}