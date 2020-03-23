using sanitary.app.ViewModels;
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
            BindingContext = new ListPositionsViewModel();
        }

		private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
		{
			Navigation.PushAsync(new CardPositionPage());
		}
	}
}