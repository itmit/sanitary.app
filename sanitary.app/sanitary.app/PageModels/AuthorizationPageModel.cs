using FreshMvvm;
using PropertyChanged;
using System.Windows.Input;
using Realms;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using sanitary.app.Models;
using System.Threading.Tasks;

namespace sanitary.app.PageModels
{
    [AddINotifyPropertyChangedInterface]

    public class AuthorizationPageModel : FreshBasePageModel
    {
        HttpClient client;

        public Realm Realm { get { return Realm.GetInstance(); } }

        public string UsernameEntry { get; set; } = string.Empty;
        public string EmailEntry { get; set; } = string.Empty;
        public string PasswordEntry { get; set; } = string.Empty;

        public AuthorizationPageModel()
        {

        }

        public ICommand OnLoginButtonClicked
        {
            get
            {
                return new FreshAwaitCommand(async (param, tcs) =>
                {
                    await OnLoginClickedAsync();
                    tcs.SetResult(true);
                });
            }
        }

        public ICommand OpenRegisterationCommand
        {
            get
            {
                return new Xamarin.Forms.Command((param) =>
                {
                    OpenRegisterPage();
                });
            }
        }

        public ICommand OpenRestorePassCommand
        {
            get
            {
                return new Xamarin.Forms.Command((param) =>
                {
                    OpenRestorePassPage();
                });
            }
        }

        private async void OpenRegisterPage()
        {
            await CoreMethods.PushPageModel<RegistrationPageModel>();
        }

        private async void OpenRestorePassPage()
        {
            await CoreMethods.PushPageModel<RestorePassPageModel>();
        }

        private async Task OnLoginClickedAsync()
        {
            if (IsThereInternet() == false)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", "Интернет-соединение отсутствует. Подключитесь к работающей сети.", "OK");
                return;
            }

            bool isValid = await AreCredentialsCorrectAsync();
            if (isValid)
            {
                App.IsUserLoggedIn = true;
                CoreMethods.SwitchOutRootNavigation(NavigationContainerNames.MainContainer);
            }

        }

        private async Task<bool> AreCredentialsCorrectAsync()
        {
            if (string.IsNullOrWhiteSpace(EmailEntry) |
                string.IsNullOrWhiteSpace(PasswordEntry) | PasswordEntry.Length < 6)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", "Неверно указан email или пароль", "OK");
                return false;
            }

            bool result = await SendUserInfoAsync();

            return result;
        }

        private bool IsThereInternet()
        {
            return Plugin.Connectivity.CrossConnectivity.Current.IsConnected;
        }

        private async Task<bool> SendUserInfoAsync()
        {
            client = new HttpClient
            {
                MaxResponseContentBufferSize = 209715200 // 200 MB
            };

            string restMethod = "login";
            System.Uri uri = new System.Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                JObject jmessage = new JObject
                {
                    { "email", EmailEntry },
                    { "password", PasswordEntry }
                };

                string json = jmessage.ToString();
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = client.PostAsync(uri, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string userInfo = await response.Content.ReadAsStringAsync();
                    JObject userObj = JObject.Parse(userInfo);

                    if(userObj["data"]["client"]["is_full_access"].ToString() == "1")
                    {
                        App.IsUserHaveFullAccess = true;
                    }
                    else
                    {
                        App.IsUserHaveFullAccess = false;
                    }

                    User user = new User
                    {
                        Name = userObj["data"]["client"]["name"].ToString(),
                        Email = userObj["data"]["client"]["email"].ToString(),
                        Token = userObj["data"]["access_token"].ToString(),
                        IsUserHaveFullAccess = App.IsUserHaveFullAccess
                    };

                    Realm.Write(() =>
                    {
                        Realm.Add(user, true);
                    });

                    return true;
                }
                else
                {
                    string errorInfo = await response.Content.ReadAsStringAsync();
                    await ParseErrorMessageAsync(errorInfo);
                    
                    return false;
                }

            }
            catch (System.Exception ex)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", ex.Message, "OK");
            }

            return true;
        }

        private async Task ParseErrorMessageAsync(string errorInfo)
        {
            string errorMessage = "";
            JObject errorObj = JObject.Parse(errorInfo);

            if (errorObj.ContainsKey("error"))
            {
                errorMessage = (string)errorObj["error"];
            }
            else if (errorObj.ContainsKey("errors"))
            {
                JToken errors = errorObj["errors"];

                if (errors["email"] != null)
                {
                    errorMessage = (string)(errors["email"].First);
                }
                else if (errors["password"] != null)
                {
                    errorMessage = (string)(errors["password"].First);
                }

            }

            await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", errorMessage, "OK");
        }
    }
}
