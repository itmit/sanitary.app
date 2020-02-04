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
    public partial class ObjectPlusPage : ContentPage
    {
        public ObjectPlusPage()
        {
            InitializeComponent();
        }

		private void ImageButton_OnClicked(object sender, EventArgs e)
		{
			SaveButton.IsEnabled = true;
		}
	}
}