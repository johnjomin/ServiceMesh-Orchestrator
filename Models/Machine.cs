using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMeshOrchestrator.Models
{
    public class Machine
    {
        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
        [BsonIgnoreIfDefault]
        public string id;

        [JsonProperty("_rev", NullValueHandling = NullValueHandling.Ignore)]
        [BsonIgnoreIfDefault]
        public string Revision;

        [JsonProperty("owners")]
        public List<User> owners;
        [JsonProperty("applications")]
        public List<Application> applications;
        [JsonProperty("address")]
        public string address;
        [JsonProperty("password")]
        public short port;
    }
}
