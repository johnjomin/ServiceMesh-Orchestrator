using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMeshOrchestrator.Models.BodyModels
{
    public class MachineBody
    {
        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; }

        [JsonProperty("owners")]
        public List<UserBody> owners { get; set; }

        [JsonProperty("applications")]
        public List<ApplicationBody> applications { get; set; }

        [JsonProperty("address")]
        public string address { get; set; }

        [JsonProperty("password")]
        public short port { get; set; }
    }
}
