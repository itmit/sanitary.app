using Newtonsoft.Json;
using Realms;

namespace sanitary.app.Models
{
    public class Material
    {
        [PrimaryKey]
        public string id { get; set; }

        public string uuid { get; set; }

        [JsonProperty("name")]
        public string Title { get; set; }

        [JsonProperty("photo")]
        public string Image { get; set; }

        [JsonProperty("count")]
        public int Quantity { get; set; }

        [JsonProperty("amount")]
        public int Price { get; set; }

        public int TotalAmount { get { return Quantity * Price; } }

        public string Description { get; set; }
    }
}
