using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMeshOrchestrator.Models.Modeller
{
    public class WorkflowServiceCreateResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether creation was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the id of the newly created entity.
        /// </summary>
        public string EntityId { get; set; }
    }
}
