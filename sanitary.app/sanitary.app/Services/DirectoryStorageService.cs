using sanitary.app.Models;
using Realms;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace sanitary.app.Services
{
    public class DirectoryStorageService : IDirectoryStorageService
    {
        readonly HttpClient client;

        public Realm Realm { get { return Realm.GetInstance(); } }

        public List<Directory> Directories { get; private set; }
        public Position Position { get; private set; }
        public User CurrentUser { get; set; }
        private bool AuthenticationHeaderIsSet { get; set; }

        public DirectoryStorageService()
        {
            client = new HttpClient
            {
                MaxResponseContentBufferSize = 209715200 // 200 MB
            };
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            SetAuthenticationHeader();
        }

        public Directory GetDirectory(string id)
        {
            return Realm.Find<Directory>(id);
        }

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
            catch (Exception)
            {
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

            Uri uri = new Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                JObject jmessage = new JObject
                {
                    { "uuid", directoryUuid }
                };
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
            catch (Exception)
            {
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

            Uri uri = new Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                JObject jmessage = new JObject
                {
                    { "uuid", directoryUuid }
                };
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
            catch (Exception)
            {
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
            catch (Exception)
            {
            }

            return Position;
        }

        public async Task<List<Directory>> SearchDirectoriesAsync(string searchText)
        {
            string restMethod = "catalog/search";
            Directories = new List<Directory>();

            if (!AuthenticationHeaderIsSet)
            {
                SetAuthenticationHeader();
            }

            Uri uri = new Uri(string.Format(Constants.RestUrl, restMethod));

            try
            {
                JObject jmessage = new JObject
                {
                    { "name", searchText }
                };
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
            catch (Exception)
            {
            }

            return Directories;
        }

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
