using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMeshOrchestrator.Models.Modeller
{
    //
    // Summary:
    //     CouchDB row.
    [ExcludeFromCodeCoverage]
    public class CouchDbRow
    {
        public CouchDbRow() { }
        //
        // Summary:
        //     Gets or sets id.
        [JsonProperty("id")]
        public string Id { get; set; }
        //
        // Summary:
        //     Gets or sets key.
        [JsonProperty("key")]
        public string Key { get; set; }
        //
        // Summary:
        //     Gets or sets doc.
        public JObject Doc { get; set; }
    }
}