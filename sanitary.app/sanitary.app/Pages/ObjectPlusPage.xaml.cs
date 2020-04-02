
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sanitary.app.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ObjectPlusPage : ContentPage
    {
        private PageModels.ObjectPlusPageModel ViewModel
        {
            get { return BindingContext as PageModels.ObjectPlusPageModel; }
            set { ViewModel = value; }
        }

        public ObjectPlusPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ObjectNameEntry.Focus();
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();

            ViewModel = BindingContext as PageModels.ObjectPlusPageModel;
            ViewModel.CoreMethods.SwitchSelectedTab<PageModels.MainPageModel>();

            return true; //Do not navigate backwards by pressing the button
        }
    }
}