using FreshMvvm;
using sanitary.app.Models;
using sanitary.app.PageModels;
using sanitary.app.Pages;
using Realms;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace sanitary.app
{
	public partial class App : Xamarin.Forms.Application
    {
        public static bool IsUserLoggedIn { get; set; }

        public App()
		{
			InitializeComponent();

			On<Xamarin.Forms.PlatformConfiguration.Android>()
				.UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);

            InitializeComponent();

            Page loginPage = FreshPageModelResolver.ResolvePageModel<AuthorizationPageModel>();
            FreshNavigationContainer loginContainer = new FreshNavigationContainer(loginPage, NavigationContainerNames.AuthenticationContainer);

            FreshTabbedNavigationContainer tabbedNavigation = new FreshTabbedNavigationContainer(NavigationContainerNames.MainContainer);
            tabbedNavigation.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            tabbedNavigation.AddTab<MainPageModel>("Главная", "ic_home_4x.png", null);
            tabbedNavigation.AddTab<DirectoryPageModel>("Каталог", "ic_directory_4x.png", null);
            tabbedNavigation.AddTab<EstimatesPageModel>("Сметы", "ic_document_4x.png", null);
            tabbedNavigation.AddTab<ObjectPageModel>("Объекты", "ic_object_4x.png", null);
            tabbedNavigation.AddTab<ObjectPlusPageModel>("+ Объект", "ic_object_plus_4x.png", null);

            tabbedNavigation.SelectedTabColor = Color.FromHex("#FFFFFF");
            tabbedNavigation.UnselectedTabColor = Color.FromHex("#77A9D3");
            tabbedNavigation.BarBackgroundColor = Color.FromHex("#0A192E");

            Realm realm = Realm.GetInstance();
            IQueryable<User> user = realm.All<User>();
            bool UserIsFound = user?.Count() > 0;

            if (!IsUserLoggedIn & !UserIsFound)
            {
                MainPage = loginContainer;
            }
            else
            {
                MainPage = tabbedNavigation;
            }
        }

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
