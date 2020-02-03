using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sanitary.app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sanitary.app.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PositionViewCell : ViewCell
	{
		public PositionViewCell()
		{
			InitializeComponent();
			BindingContext = new ListPositionsViewModel();
		}
	}
}