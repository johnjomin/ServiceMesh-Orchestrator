using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMeshOrchestrator.Models
{
    public class User
    {
        [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore)]
        [BsonIgnoreIfDefault]
        public string id;

        [JsonProperty("_rev", NullValueHandling = NullValueHandling.Ignore)]
        [BsonIgnoreIfDefault]
        public string Revision;

        [JsonProperty("name")]
        public string name;
        [JsonProperty("email")]
        public string email;
        [JsonProperty("password")]
        public string password;
    }
}
