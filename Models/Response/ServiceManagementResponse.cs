using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMeshOrchestrator.Models.Response
{
    public class ServiceManagementResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether creation was successful.
        /// </summary>
        public bool IsAcknowledged { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether creation was successful.
        /// </summary>
        public long MatchedCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether creation was successful.
        /// </summary>
        public long ModifiedCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether creation was successful.
        /// </summary>
        public BsonValue UpsertedId { get; set; }
    }
}
