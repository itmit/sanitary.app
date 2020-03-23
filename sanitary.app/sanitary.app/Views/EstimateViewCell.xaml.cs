using sanitary.app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sanitary.app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EstimateViewCell : ViewCell
    {
        public EstimateViewCell()
        {
            InitializeComponent();
			BindingContext = new DownloadEstimatesViewModel();
		}
    }
}