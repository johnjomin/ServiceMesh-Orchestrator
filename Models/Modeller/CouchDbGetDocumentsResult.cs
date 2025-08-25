using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMeshOrchestrator.Models.Modeller
{
    //
    // Summary:
    //     CouchDB get documents result.
    [ExcludeFromCodeCoverage]
    public class CouchDbGetDocumentsResult
    {
        public CouchDbGetDocumentsResult() { }

        //
        // Summary:
        //     Gets or sets offset.
        [JsonProperty("offset")]
        public int Offset { get; set; }
        //
        // Summary:
        //     Gets or sets total rows.
        [JsonProperty("total_rows")]
        public int TotalRows { get; set; }
        //
        // Summary:
        //     Gets or sets rows.
        [JsonProperty("rows")]
        public IEnumerable<CouchDbRow> Rows { get; set; }
    }
}
