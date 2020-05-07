using FreshMvvm;
using PropertyChanged;
using System;
using System.Linq;
using sanitary.app.Models;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Realms;
using System.Net.Http.Headers;

namespace sanitary.app.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class ProfilePageModel : FreshBasePageModel
    {
        readonly HttpClient client = new HttpClient();
        public User CurrentUser { get; private set; }
        public string UserEmail { get; private set; }
        public string PasswordEntry { get; set; }
        public string RepeatPasswordEntry { get; set; }

        public bool IsEditPassActive { get; set; } = false;

        public ICommand EntryCompletedCommand
        {
            get
            {
                return new Xamarin.Forms.Command(async () =>
                {
                    await CoreMethods.DisplayAlert("OK", "Desc", "OK");
                });
            }
        }

        public ProfilePageModel()
        {

        }

        public ICommand ActivateEditPassCommand
        {
            get
            {
                return new Xamarin.Forms.Command(() =>
                {
                    IsEditPassActive = true;
                });
            }
        }

        public ICommand UpdatePasswordCommand
        {
            get
            {
                return new Xamarin.Forms.Command(() =>
                {
                    UpdatePassword();
                    PasswordEntry = string.Empty;
                    RepeatPasswordEntry = string.Empty;
                    IsEditPassActive = false;
                });
            }
        }

        public ICommand ExitProfileCommand
        {
            get
            {
                return new Xamarin.Forms.Command(async () =>
                {
                    bool answer = await CoreMethods.DisplayAlert("Внимание", "Вы действительно хотите выйти из приложения?", "Да", "Нет");

                    if (answer == true)
                    {
                        ExitAppAsync();
                    }
                });
            }
        }


        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            if (IsThereInternet() == false)
            {
                return;
            }

            GetUserInfo();
        }

        private async void UpdatePassword()
        {
            string restMethod = "changePassword";
            Uri uri = new Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                SetAuthenticationHeader();

                JObject jmessage = new JObject
                {
                    { "password", PasswordEntry },
                    { "password_confirmation", RepeatPasswordEntry }
                };

                string json = jmessage.ToString();
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = client.PostAsync(uri, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string userInfo = await response.Content.ReadAsStringAsync();
                    await CoreMethods.DisplayAlert("Выполнено", "Пароль успешно изменен", "Ok");
                }
                else
                {
                    string errorInfo = await response.Content.ReadAsStringAsync();
                }

            }
            catch (System.Exception ex)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", ex.Message, "OK");
            }

            return;
        }

        private void SetAuthenticationHeader()
        {
            Realm realm = Realm.GetInstance();
            var users = realm.All<User>();
            User user;

            if (users.Count() > 0)
            {
                user = users.Last();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
            }
        }

        private void GetUserInfo()
        {
            Realms.Realm realm = Realms.Realm.GetInstance();
            CurrentUser = realm.All<User>().Last();

            UserEmail = CurrentUser.Email;
        }

        private async void ExitAppAsync()
        {
            Realm CurrentRealm = Realm.GetInstance();
            CurrentRealm.Write(() =>
            {
                CurrentRealm.RemoveAll<User>();
            });

            App.IsUserLoggedIn = false;

            await CoreMethods.PopPageModel(true, false, true);
            await CoreMethods.SwitchSelectedTab<MainPageModel>();
            CoreMethods.SwitchOutRootNavigation(NavigationContainerNames.AuthenticationContainer);
        }

        private bool IsThereInternet()
        {
            return Plugin.Connectivity.CrossConnectivity.Current.IsConnected;
        }
    }
}
