using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace sanitary.app.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardPositionPage : ContentPage
	{
		private int _number = 1;
		private int _price = 100;
        public CardPositionPage()
        {
            InitializeComponent();
			ValueLabel.Text = "1";
			PriceLabel.Text = "100";
		}

		private void ImageButton_OnClicked(object sender, EventArgs e)
		{
			GetSum();
		}

		private void ImageButton_OnClicked1(object sender, EventArgs e)
		{
			GetSubtract();
		}

		private void GetSum()
		{
			_number++;
			_price += 100;
			ValueLabel.Text = _number.ToString();
			PriceLabel.Text = _price.ToString();
		}

		private void GetSubtract()
		{
			_number--;
			_price -= 100;
			ValueLabel.Text = _number.ToString();
			PriceLabel.Text = _price.ToString();
		}

		private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
		{
			AddButton.IsEnabled = true;
		}

		private void AddButton_OnClicked(object sender, EventArgs e)
		{
			Navigation.PopToRootAsync();
		}
	}
}