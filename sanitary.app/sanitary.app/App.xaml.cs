using FreshMvvm;
using sanitary.app.Models;
using sanitary.app.PageModels;
using Realms;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using sanitary.app.Services;

namespace sanitary.app
{
    public partial class App : Xamarin.Forms.Application
    {
        public static bool IsUserLoggedIn { get; set; }
        public static bool IsUserHaveFullAccess { get; set; }

        public App()
		{
			InitializeComponent();

			On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);

            SetUpIoC();

            Page loginPage = FreshPageModelResolver.ResolvePageModel<AuthorizationPageModel>();
            FreshNavigationContainer loginContainer = new FreshNavigationContainer(loginPage, NavigationContainerNames.AuthenticationContainer);

            ExtendedTabbedPage tabbedNavigation = SetUpTabbedNavigation();

            Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);

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

        private void TabbedNavigation_CurrentPageChanged(object sender, System.EventArgs e)
        {
            var navigationContainer = (FreshTabbedNavigationContainer)sender;
            navigationContainer.CurrentPage.Navigation.PopToRootAsync();
        }

        void SetUpIoC()
        {
            FreshIOC.Container.Register<IDirectoryStorageService, DirectoryStorageService>();
            FreshIOC.Container.Register<IObjectStorageService, ObjectStorageService>();
        }

        private ExtendedTabbedPage SetUpTabbedNavigation()
        {
            ExtendedTabbedPage tabbedNavigation = new ExtendedTabbedPage(NavigationContainerNames.MainContainer);
            tabbedNavigation.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            tabbedNavigation.AddTab<MainPageModel>("Главная", "ic_home_4x.png", null);
            tabbedNavigation.AddTab<DirectoryPageModel>("Каталог", "ic_directory_4x.png", null);
            tabbedNavigation.AddTab<EstimatesPageModel>("Сметы", "ic_document_4x.png", null);
            tabbedNavigation.AddTab<ObjectPageModel>("Объекты", "ic_object_4x.png", null);
            tabbedNavigation.AddTab<ObjectPlusPageModel>("+Объект", "ic_object_plus_4x.png", null);

            tabbedNavigation.SelectedTabColor = Color.FromHex("#FFFFFF");
            tabbedNavigation.UnselectedTabColor = Color.FromHex("#77A9D3");
            tabbedNavigation.BarBackgroundColor = Color.FromHex("#686bb1");

            tabbedNavigation.CurrentPageChanged += TabbedNavigation_CurrentPageChanged;

            return tabbedNavigation;
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
