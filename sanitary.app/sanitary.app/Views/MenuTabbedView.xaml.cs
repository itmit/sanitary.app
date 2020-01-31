﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using TabbedPage = Xamarin.Forms.TabbedPage;
using static Xamarin.Forms.PlatformConfiguration.AndroidSpecific.TabbedPage;

namespace sanitary.app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuTabbedView : TabbedPage
    {
        public MenuTabbedView()
        {
            InitializeComponent();
			SetToolbarPlacement(this, ToolbarPlacement.Bottom);
		}
    }
}