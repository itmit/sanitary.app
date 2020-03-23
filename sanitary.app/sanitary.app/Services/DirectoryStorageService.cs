using sanitary.app.Models;
using Xamarin.Forms;
using Realms;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json.Linq;

namespace sanitary.app.Services
{
    public class DirectoryStorageService : IDirectoryStorageService
    {
        HttpClient client;

        public Realm Realm { get { return Realm.GetInstance(); } }

        public List<Directory> Directories { get; private set; }
        public Position Position { get; private set; }
        public User CurrentUser { get; set; }
        private bool AuthenticationHeaderIsSet { get; set; }

        public DirectoryStorageService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 209715200; // 200 MB
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            SetAuthenticationHeader();
        }

        public Directory GetDirectory(string id)
        {
            return Realm.Find<Directory>(id);
        }

        /// <summary>
        /// Gets all dialogs
        /// </summary>
        /// <returns>All dialogs</returns>
        public async Task<List<Directory>> GetAllDirectoriesAsync()
        {
            string restMethod = "catalog";
            Directories = new List<Directory>();

            if(!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }

            Uri uri = new Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                var responseAwaiter = client.GetAsync(uri).ConfigureAwait(false);
                var response = responseAwaiter.GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject catalogArr = JObject.Parse(content);
                    Directories = JsonConvert.DeserializeObject<List<Directory>>(catalogArr["data"].ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            return Directories;
        }

        public async Task<List<Directory>> GetSubDirectoriesAsync(string directoryUuid)
        {
            if (!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }

            string restMethod = "catalog/category";
            Directories = new List<Directory>();

            Realm realm = Realm.GetInstance();
            User user = realm.All<User>().Last();

            Uri uri = new Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                JObject jmessage = new JObject();
                jmessage.Add("uuid", directoryUuid);
                string json = jmessage.ToString();
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var responseAwaiter = client.PostAsync(uri, content).ConfigureAwait(false);
                var response = responseAwaiter.GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JObject catalogArr = JObject.Parse(result);
                    Directories = JsonConvert.DeserializeObject<List<Directory>>(catalogArr["data"].ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            return Directories;
        }

        public async Task<List<Directory>> GetPositionsAsync(string directoryUuid)
        {
            if (!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }
            string restMethod = "catalog/category/item";
            Directories = new List<Directory>();

            Realm realm = Realm.GetInstance();
            User user = realm.All<User>().Last();

            Uri uri = new Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                JObject jmessage = new JObject();
                jmessage.Add("uuid", directoryUuid);
                string json = jmessage.ToString();
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var responseAwaiter = client.PostAsync(uri, content).ConfigureAwait(false);
                var response = responseAwaiter.GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JObject catalogArr = JObject.Parse(result);
                    Directories = JsonConvert.DeserializeObject<List<Directory>>(catalogArr["data"].ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            return Directories;
        }

        public async Task<Position> GetSinglePositionAsync(string positionUuid)
        {
            if (!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }
            string restMethod = "catalog/category/item/" + positionUuid;
            Position = new Position();

            Realm realm = Realm.GetInstance();
            User user = realm.All<User>().Last();

            Uri uri = new Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                var responseAwaiter = client.GetAsync(uri).ConfigureAwait(false);
                var response = responseAwaiter.GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    JObject catalogArr = JObject.Parse(result);
                    Position = JsonConvert.DeserializeObject<Position>(catalogArr["data"].ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            return Position;
        }

        /// <summary>
        /// Checks if the given directory exists
        /// </summary>
        /// <returns><c>true</c>, if directory was found, <c>false</c> otherwise</returns>
        /// <param name="alarm">The Directory we want to know already exists</param>
        public bool DoesDirectoryExist(Directory directory)
        {
            var containsDirectory = Realm.All<Directory>().Contains(directory);
            if (containsDirectory)
                return true;

            return false;
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
    }
}
