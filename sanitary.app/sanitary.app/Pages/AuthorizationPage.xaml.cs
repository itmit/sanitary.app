﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sanitary.app.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AuthorizationPage : ContentPage
	{
		public AuthorizationPage()
		{
			InitializeComponent();
		}

		private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
		{
			Navigation.PushAsync(new RegistrationPage());
		}
	}
}