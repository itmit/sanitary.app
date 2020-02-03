using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using sanitary.app.Models;
using sanitary.app.Pages;
using Xamarin.Forms;

namespace sanitary.app.ViewModels
{
    public class ProfileViewModel
    {
		#region Fields
		private string _name;
		private string _email;
		private string _password;
		#endregion

		public ProfileViewModel()
		{
			var user = new User();
			Name = user.Name;
			Email = user.Email;
			ExitProfile = new Command(Exit);
		}

		#region Prop
		public string Name
		{
			get => _name;
			set => _name = value;
		}

		public string Email
		{
			get => _email;
			set => _email = value;
		}

		public string Password
		{
			get => _password;
			set => _password = value;
		}

		public ICommand ExitProfile
		{
			get;
		}
		#endregion

		private void Exit()
		{
			Application.Current.MainPage.Navigation.PopAsync();
		}
	}
}
