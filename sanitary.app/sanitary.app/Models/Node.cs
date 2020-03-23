using Newtonsoft.Json;
using Realms;
using System.Collections.Generic;

namespace sanitary.app.Models
{
    public class Node
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string uuid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("items")]
        public List<Material> Materials { get; set; }

        [JsonProperty("entity_name")]
        public string ObjectName { get; set; }

        public string FullName { get { return Name + ", " + ObjectName; } }
    }
}
