using Newtonsoft.Json;
using System.Collections.Generic;

namespace sanitary.app.Models
{
    public class Object
    {
        public string uuid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("data")]
        public List<Node> Nodes { get; set; }
    }
}
