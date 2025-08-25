using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMeshOrchestrator.Models.Modeller
{
    public class CouchDbGetDocumentResult
    {
        public CouchDbGetDocumentResult() { }
        //
        // Summary:
        //     Gets or sets document id.
        [JsonProperty("_id")]
        [MaxLength(256)]
        public string DocumentId { get; set; }
        //
        // Summary:
        //     Gets or sets revision id.
        [JsonProperty("_rev")]
        public string RevisionId { get; set; }
        //
        // Summary:
        //     Gets or sets properties.
        [JsonExtensionData]
        public IDictionary<string, JToken> Properties { get; set; }
    }
}
