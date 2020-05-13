using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Realms;
using sanitary.app.Models;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;
using System;

namespace sanitary.app.Services
{
    public class PaymentService : IPaymentService
    {
        readonly HttpClient client;

        private bool AuthenticationHeaderIsSet { get; set; }

        public Realm Realm { get { return Realm.GetInstance(); } }

        public PaymentService()
        {
            client = new HttpClient
            {
                MaxResponseContentBufferSize = 209715200 // 200 MB
            };
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            SetAuthenticationHeader();
        }

        public async Task MakePurchase()
        {
            try
            {
                string productId = "test_access";

                bool connected = await CrossInAppBilling.Current.ConnectAsync();

                if (!connected)
                {
                    //Couldn't connect to billing, could be offline, alert user
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", "Не удалось подключиться к серверу оплаты.", "OK");
                    return;
                }

                //string purchaseToken = "inapp:" + Xamarin.Essentials.AppInfo.PackageName + ":full_access";

                //try to purchase item
                //InAppBillingPurchase purchase = await CrossInAppBilling.Current.PurchaseAsync(productId, ItemType.InAppPurchase, "apppayload");
                var purchase = await CrossInAppBilling.Current.PurchaseAsync(productId, ItemType.InAppPurchase, "apppayload");
                if (purchase == null)
                {
                    //Not purchased, alert the user
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", "Не удалось произвести оплату.", "OK");
                }
                else
                {
                    //Purchased, save this information
                    SendPurchaseToServer(purchase);
                }
            }
            catch (Exception ex)
            {
                //Something bad has occurred, alert user
                App.IsUserHaveFullAccess = true;
            }
            finally
            {
                //Disconnect, it is okay if we never connected
                await CrossInAppBilling.Current.DisconnectAsync();
            }
        }

        private async void SendPurchaseToServer(InAppBillingPurchase purchase)
        {
            string restMethod = "purchaseAccess";
            System.Uri uri = new System.Uri(string.Format(Constants.RestUrl, restMethod));

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Realm realm = Realm.GetInstance();
                IQueryable<User> users = realm.All<User>();
                User user;

                if (users.Count() > 0)
                {
                    user = users.Last();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                }
                else
                {
                    return;
                }

                JObject jmessage = new JObject
                {
                    { "id", purchase.Id },
                    { "payload", purchase.Payload },
                    { "purchaseToken", purchase.PurchaseToken },
                    { "state", purchase.State.ToString() }
                };
                string json = jmessage.ToString();
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = client.PostAsync(uri, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    App.IsUserHaveFullAccess = true;
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Успех", "Оплата проведена успешно.", "OK");
                    return;
                }
                else
                {
                    string errorInfo = await response.Content.ReadAsStringAsync();
                    string errorMessage = ParseErrorMessage(errorInfo);

                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () => { await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", "Произошла ошибка на сервере", "OK"); });
                    return;
                }
            }
        }

        #region Add/Update methods
        public async Task<bool> SendNewObjectAsync(Models.Object CreatedObject)
        {
            if (IsThereInternet() == false)
            {
                return false;
            }

            if (!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }

            string restMethod = "entity";
            System.Uri uri = new System.Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                string jmessage = JsonConvert.SerializeObject(CreatedObject, Formatting.Indented);

                StringContent content = new StringContent(jmessage, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = client.PostAsync(uri, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseMessage = await response.Content.ReadAsStringAsync();

                    return true;
                }
                else
                {
                    string errorInfo = await response.Content.ReadAsStringAsync();
                    string errorMessage = ParseErrorMessage(errorInfo);

                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () => { await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", errorMessage, "OK"); });

                    return false;
                }

            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("An exception ({0}) occurred.", ex.GetType().Name);
                System.Console.WriteLine("Message:\n   {0}\n", ex.Message);
                System.Console.WriteLine("Stack Trace:\n   {0}\n", ex.StackTrace);

                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", ex.GetType().Name + "\n" + ex.Message + "\n" + ex.StackTrace, "OK");
                return false;
            }
        }
        #endregion

        #region Utility private methods
        private bool IsThereInternet()
        {
            // TODO: if no internet, show alert
            return Plugin.Connectivity.CrossConnectivity.Current.IsConnected;
        }

        private string ParseErrorMessage(string errorInfo)
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

                if (errors["data"] != null)
                {
                    errorMessage = (string)(errors["data"].First);
                }
                else if (errors["name"] != null)
                {
                    errorMessage = (string)(errors["name"].First);
                }

            }

            return errorMessage;
        }

        private void SetAuthenticationHeader()
        {
            Realm realm = Realm.GetInstance();
            IQueryable<User> users = realm.All<User>();
            User user;

            if (users.Count() > 0)
            {
                user = users.Last();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                AuthenticationHeaderIsSet = true;
            }
            else
            {
                AuthenticationHeaderIsSet = false;
            }
        }
        #endregion
    }
}
