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
			StackVisible.IsVisible = true;
		}

		private async void ImageButton_OnClicked1(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new DirectoryPage());
			StackObject.IsVisible = true;
		}
	}
}