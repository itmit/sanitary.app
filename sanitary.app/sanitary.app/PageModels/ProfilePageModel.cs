using FreshMvvm;
using PropertyChanged;
using System;
using System.Linq;
using sanitary.app.Models;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Realms;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace sanitary.app.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class ProfilePageModel : FreshBasePageModel
    {
        HttpClient client = new HttpClient();
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

        protected async override void ViewIsAppearing(object sender, EventArgs e)
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

                JObject jmessage = new JObject();
                jmessage.Add("password", PasswordEntry);
                jmessage.Add("password_confirmation", RepeatPasswordEntry);

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

        private bool IsThereInternet()
        {
            return Plugin.Connectivity.CrossConnectivity.Current.IsConnected;
        }
    }
}
