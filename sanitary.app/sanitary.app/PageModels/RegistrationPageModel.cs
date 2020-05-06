using FreshMvvm;
using System.Windows.Input;
using Realms;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using sanitary.app.Models;
using System.Threading.Tasks;

namespace sanitary.app.PageModels
{
    public class RegistrationPageModel : FreshBasePageModel
    {
        HttpClient client;

        public string MessageLabel { get; set; } = string.Empty;
        public Realm Realm { get { return Realm.GetInstance(); } }

        public string FullName { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Adress { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RepeatPassword { get; set; } = string.Empty;

        public string UserToken { get; set; } = string.Empty;

        public RegistrationPageModel()
        {

        }

        public ICommand OnRegisterButtonClicked
        {
            get
            {
                return new FreshAwaitCommand(async (param, tcs) =>
                {
                    await OnRegisterClicked();
                    tcs.SetResult(true);
                });
            }
        }

        async Task OnRegisterClicked()
        {
            if (IsThereInternet() == false)
            {
                MessageLabel = "Интернет-соединение отсутствует. Подключитесь к работающей сети.";
                return;
            }

            User user = new User
            {
                Name = FullName,
                Email = Email,
                Password = Password
            };

            bool isValid = await AreCredentialsCorrectAsync(user);
            if (isValid)
            {
                App.IsUserLoggedIn = true;

                user.Token = UserToken;

                Realm.Write(() =>
                {
                    Realm.Add(user, true);
                });

                CoreMethods.SwitchOutRootNavigation(NavigationContainerNames.MainContainer);
            }

        }

        async Task<bool> AreCredentialsCorrectAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name) |
                string.IsNullOrWhiteSpace(user.Email) |
                string.IsNullOrWhiteSpace(user.Password) | user.Password.Length < 5)
            {
                MessageLabel = "НЕВЕРНО УКАЗАН НОМЕР ТЕЛЕФОНА ИЛИ ПАРОЛЬ";
                return false;
            }

            if (Password != RepeatPassword)
            {
                MessageLabel = "Пароли не совпадают";
                return false;
            }

            // TODO:
            // add Email, Organization validation

            bool result = await SendUserInfoAsync(user);

            return result;
        }

        private bool IsThereInternet()
        {
            return Plugin.Connectivity.CrossConnectivity.Current.IsConnected;
        }

        private async Task<bool> SendUserInfoAsync(User user)
        {
            client = new HttpClient
            {
                MaxResponseContentBufferSize = 209715200 // 200 MB
            };

            string restMethod = "register";
            System.Uri uri = new System.Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                JObject jmessage = new JObject
                {
                    { "email", user.Email },
                    { "name", user.Name },
                    { "password", user.Password },
                    { "password_confirmation", RepeatPassword }
                };

                string json = jmessage.ToString();
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = client.PostAsync(uri, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JObject resultArray = JObject.Parse(result);

                    UserToken = resultArray["data"]["access_token"].ToString();

                    App.IsUserHaveFullAccess = false;

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