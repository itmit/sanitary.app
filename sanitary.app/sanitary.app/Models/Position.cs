using Newtonsoft.Json;
using Realms;

namespace sanitary.app.Models
{
    public class Position : RealmObject
    {
        public string id { get; set; }

        [PrimaryKey]
        public string uuid { get; set; }

        [JsonProperty("name")]
        public string Title { get; set; }

        [JsonProperty("photo")]
        public string Image { get; set; }

        public string ImagePath { get { return string.Format(Constants.ImageDomainUrl, Image); } }
    }
}
