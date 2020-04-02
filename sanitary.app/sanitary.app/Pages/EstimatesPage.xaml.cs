using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sanitary.app.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EstimatesPage : ContentPage
    {
        private PageModels.EstimatesPageModel ViewModel
        {
            get { return BindingContext as PageModels.EstimatesPageModel; }
            set { ViewModel = value; }
        }

        public EstimatesPage()
        {
            InitializeComponent();
		}

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();

            ViewModel = BindingContext as PageModels.EstimatesPageModel;
            ViewModel.CoreMethods.SwitchSelectedTab<PageModels.MainPageModel>();

            return true; //Do not navigate backwards by pressing the button
        }
    }
}