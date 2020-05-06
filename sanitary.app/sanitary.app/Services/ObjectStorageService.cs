using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Realms;
using sanitary.app.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace sanitary.app.Services
{
    public class ObjectStorageService : IObjectStorageService
    {
        readonly HttpClient client;

        private bool AuthenticationHeaderIsSet { get; set; }

        public Realm Realm { get { return Realm.GetInstance(); } }

        public ObjectStorageService()
        {
            client = new HttpClient
            {
                MaxResponseContentBufferSize = 209715200 // 200 MB
            };
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            SetAuthenticationHeader();
        }

        #region Get data methods
        public async Task<Object> GetObjectFullInfo(string ObjectUuid)
        {
            if (IsThereInternet() == false)
            {
                return null;
            }

            if (!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }

            string restMethod = "entity/" + ObjectUuid + "/edit";
            Object Object = new Object();

            System.Uri uri = new System.Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                var responseAwaiter = client.GetAsync(uri).ConfigureAwait(false);
                var response = responseAwaiter.GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject objectArr = JObject.Parse(content);
                    var test = objectArr["data"]["entity"].ToString();
                    Object = JsonConvert.DeserializeObject<Object>(objectArr["data"]["entity"].ToString());
                    Object.Nodes = JsonConvert.DeserializeObject<List<Node>>(objectArr["data"]["nodes"].ToString());
                }
                else
                {
                    string errorInfo = await response.Content.ReadAsStringAsync();
                    string errorMessage = ParseErrorMessage(errorInfo);

                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () => { await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", errorMessage, "OK"); });
                }
            }
            catch (System.Exception)
            {
            }

            return Object;
        }

        public async Task<List<Object>> GetUserObjectsAsync()
        {
            if (IsThereInternet() == false)
            {
                return null;
            }

            if (!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }

            string restMethod = "entity";
            List<Object> Objects = new List<Object>();

            System.Uri uri = new System.Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                var responseAwaiter = client.GetAsync(uri).ConfigureAwait(false);
                var response = responseAwaiter.GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject catalogArr = JObject.Parse(content);
                    Objects = JsonConvert.DeserializeObject<List<Object>>(catalogArr["data"].ToString());
                }
            }
            catch (System.Exception)
            {
            }

            return Objects;
        }

        public async Task<List<Node>> GetObjectNodesAsync(string ObjectUuid)
        {
            if (IsThereInternet() == false)
            {
                return null;
            }

            if (!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }

            string restMethod = "entity/" + ObjectUuid;
            List<Node> Nodes = new List<Node>();

            System.Uri uri = new System.Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                var responseAwaiter = client.GetAsync(uri).ConfigureAwait(false);
                var response = responseAwaiter.GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject catalogArr = JObject.Parse(content);
                    Nodes = JsonConvert.DeserializeObject<List<Node>>(catalogArr["data"].ToString());
                }
            }
            catch (System.Exception)
            {
            }

            return Nodes;
        }

        public async Task<List<Material>> GetObjectMaterialsAsync(string ObjectUuid)
        {
            if (IsThereInternet() == false)
            {
                return null;
            }

            if (!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }

            string restMethod = "entity/getEstimate/" + ObjectUuid;
            List<Material> Materials = new List<Material>();

            System.Uri uri = new System.Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                var responseAwaiter = client.GetAsync(uri).ConfigureAwait(false);
                var response = responseAwaiter.GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject catalogArr = JObject.Parse(content);
                    Materials = JsonConvert.DeserializeObject<List<Material>>(catalogArr["data"].ToString());
                }
            }
            catch (System.Exception)
            {
            }

            return Materials;
        }

        public async Task<List<Node>> GetAllUserNodesAsync()
        {
            if (IsThereInternet() == false)
            {
                return null;
            }

            if (!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }

            string restMethod = "node";
            List<Node> Nodes = new List<Node>();

            System.Uri uri = new System.Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                var responseAwaiter = client.GetAsync(uri).ConfigureAwait(false);
                var response = responseAwaiter.GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject catalogArr = JObject.Parse(content);
                    Nodes = JsonConvert.DeserializeObject<List<Node>>(catalogArr["data"].ToString());
                }
            }
            catch (System.Exception)
            {
            }

            return Nodes;
        }

        public async Task<string> GetEstimatePdfUrlAsync(string ObjectUuid)
        {
            if (IsThereInternet() == false)
            {
                return null;
            }

            if (!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }

            string restMethod = "entity/getEstimatePDF/" + ObjectUuid;
            string FilePath = "";

            System.Uri uri = new System.Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                var responseAwaiter = client.GetAsync(uri).ConfigureAwait(false);
                var response = responseAwaiter.GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject resultArr = JObject.Parse(content);
                    FilePath = resultArr["data"].First.ToString();
                }
                else
                {
                    string errorInfo = await response.Content.ReadAsStringAsync();
                    string errorMessage = ParseErrorMessage(errorInfo);

                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () => { await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", errorMessage, "OK"); });
                }
            }
            catch (System.Exception)
            {
            }

            return FilePath;
        }
        #endregion

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

        public async Task<bool> UpdateObjectAsync(Models.Object CreatedObject)
        {
            if (IsThereInternet() == false)
            {
                return false;
            }

            if (!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }

            string restMethod = "entity/" + CreatedObject.uuid;
            System.Uri uri = new System.Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                string jmessage = JsonConvert.SerializeObject(CreatedObject, Formatting.Indented);

                HttpMethod method = new HttpMethod("PATCH");

                HttpRequestMessage request = new HttpRequestMessage(method, uri)
                {
                    Content = new StringContent(jmessage, System.Text.Encoding.UTF8, "application/json")
                };

                HttpResponseMessage response = null;
                response = await client.SendAsync(request);

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
            catch (HttpRequestException e)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", e.InnerException.Message, "OK");
                return false;
            }
        }

        public async Task<bool> AddMaterialToNode(Material MaterialToAdd, string NodeId)
        {
            if (IsThereInternet() == false)
            {
                return false;
            }

            if (!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }

            string restMethod = "addItemToNode";
            System.Uri uri = new System.Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                JObject jmessage = new JObject
                {
                    { "uuid_item", MaterialToAdd.uuid },
                    { "uuid_node", NodeId },
                    { "count", MaterialToAdd.Quantity },
                    { "amount", MaterialToAdd.Price },
                    { "description", MaterialToAdd.Description }
                };
                string json = jmessage.ToString();
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

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
                    return false;
                }

            }
            catch (System.Exception ex)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", ex.Message, "OK");
            }

            return false;
        }

        public async void SendCopyNodeRequestAsync(string nodeUuid, string objectUuid)
        {
            if (IsThereInternet() == false)
            {
                return;
            }

            if (!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }

            string restMethod = "node/copy";
            System.Uri uri = new System.Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                JObject jmessage = new JObject
                {
                    { "uuid", nodeUuid },
                    { "uuid_to", objectUuid }
                };
                string json = jmessage.ToString();
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = client.PostAsync(uri, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    return;
                }
                else
                {
                    string errorInfo = await response.Content.ReadAsStringAsync();
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", errorInfo, "OK");
                    return;
                }

            }
            catch (System.Exception ex)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", ex.Message, "OK");
            }
        }
        #endregion

        #region Delete methods
        public async Task<bool> DeleteUserObjectsAsync(Object objectToDelete)
        {
            if (IsThereInternet() == false)
            {
                return false;
            }

            if (!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }

            string restMethod = "entity/" + objectToDelete.uuid;
            System.Uri uri = new System.Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                HttpResponseMessage response = null;
                response = client.DeleteAsync(uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    return true;
                }
                else
                {
                    string errorInfo = await response.Content.ReadAsStringAsync();
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", errorInfo, "OK");
                    return false;
                }

            }
            catch (System.Exception ex)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", ex.Message, "OK");
            }

            return false;
        }

        public async Task<bool> DeleteNodeAsync(string NodeUuid)
        {
            if (IsThereInternet() == false)
            {
                return false;
            }

            if (!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }

            string restMethod = "node/" + NodeUuid;
            System.Uri uri = new System.Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                HttpResponseMessage response = null;
                response = client.DeleteAsync(uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    return true;
                }
                else
                {
                    string errorInfo = await response.Content.ReadAsStringAsync();
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", errorInfo, "OK");
                    return false;
                }

            }
            catch (System.Exception ex)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", ex.Message, "OK");
            }

            return false;
        }

        public async Task<bool> DeleteMaterialFromNodeAsync(string materialUuid)
        {
            if (IsThereInternet() == false)
            {
                return false;
            }

            if (!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }

            string restMethod = "node/destroyItemFromNode/" + materialUuid;
            System.Uri uri = new System.Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                HttpResponseMessage response = null;
                response = client.DeleteAsync(uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseMessage = await response.Content.ReadAsStringAsync();
                    return true;
                }
                else
                {
                    string errorInfo = await response.Content.ReadAsStringAsync();
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", errorInfo, "OK");
                    return false;
                }

            }
            catch (System.Exception ex)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Не выполнено", ex.Message, "OK");
            }

            return false;
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
            var users = realm.All<User>();
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
