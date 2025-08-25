using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMeshOrchestrator.Models
{
    public class Application
    {
        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
        [BsonIgnoreIfDefault]
        public string id;

        [JsonProperty("_rev", NullValueHandling = NullValueHandling.Ignore)]
        [BsonIgnoreIfDefault]
        public string Revision;

        [JsonProperty("container")]
        public string container;
        [JsonProperty("status")]
        public string status;
    }
}
