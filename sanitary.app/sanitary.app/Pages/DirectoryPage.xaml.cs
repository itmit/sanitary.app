﻿using sanitary.app.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sanitary.app.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DirectoryPage : ContentPage
    {
        private PageModels.DirectoryPageModel ViewModel
        {
            get { return BindingContext as PageModels.DirectoryPageModel; }
            set { ViewModel = value; }
        }

        public DirectoryPage()
        {
            InitializeComponent();

            //AdmobControl admobControl = new AdmobControl()
            //{
            //    AdUnitId = Constants.BannerId
            //};
            //Label adLabel = new Label() { Text = "Ads will be displayed here!" };

            //Content = new StackLayout()
            //{
            //    Children = { adLabel, admobControl }
            //};
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();

            ViewModel = BindingContext as PageModels.DirectoryPageModel;
            ViewModel.CoreMethods.SwitchSelectedTab<PageModels.MainPageModel>();

            return true; //Do not navigate backwards by pressing the button
        }
    }
}