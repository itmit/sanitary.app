﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sanitary.app.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ObjectPage : ContentPage
    {
        private PageModels.ObjectPageModel ViewModel
        {
            get { return BindingContext as PageModels.ObjectPageModel; }
            set { ViewModel = value; }
        }

        public ObjectPage()
        {
            InitializeComponent();
		}

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();

            ViewModel = BindingContext as PageModels.ObjectPageModel;
            ViewModel.CoreMethods.SwitchSelectedTab<PageModels.MainPageModel>();

            return true; //Do not navigate backwards by pressing the button
        }
    }
}