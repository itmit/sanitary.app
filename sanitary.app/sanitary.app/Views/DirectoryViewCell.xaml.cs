using sanitary.app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sanitary.app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DirectoryViewCell : ViewCell
    {
        public DirectoryViewCell()
        {
            InitializeComponent();
			BindingContext = new DirectoryViewModel();
		}
    }
}