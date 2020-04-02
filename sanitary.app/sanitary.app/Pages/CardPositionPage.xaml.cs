using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sanitary.app.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardPositionPage : ContentPage
	{
		private int _number = 1;
        public CardPositionPage()
        {
            InitializeComponent();
			ValueLabel.Text = "1";
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
			ValueLabel.Text = _number.ToString();
		}

		private void GetSubtract()
		{
			_number--;
            if(_number < 1)
            {
                _number = 1;
            }
			ValueLabel.Text = _number.ToString();
		}
	}
}