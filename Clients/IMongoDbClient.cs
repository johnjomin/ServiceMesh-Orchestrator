using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ServiceMeshOrchestrator.Models.Modeller;

namespace ServiceMeshOrchestrator.Clients
{
    public interface IMongoDbClient
    {
        /// <summary>
        /// Gets requested documents.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="query">Query string.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Documents result.</returns>
        /// <exception cref="HttpRequestException">Thrown if status isn't successful.</exception>
        Task<CouchDbGetDocumentsResult> GetDocumentsAsync(string database, string query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get requested document.
        /// </summary>
        /// <param name="database">Database where document is located.</param>
        /// <param name="documentId">Id of document to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Document result.</returns>
        Task<CouchDbGetDocumentResult> GetDocumentAsync(string database, string documentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Post requested documents.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="documentId">Id of document to be created.</param>
        /// <param name="content">Content body.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating whether action was successful.</returns>
        Task<CouchDbCreateDocumentResult> CreateDocumentAsync(string database, string documentId, object content, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete the specified document.
        /// </summary>
        /// <param name="database">Database where document is located.</param>
        /// <param name="documentId">Id of document to delete.</param>
        /// <param name="revisionId">Latest revision of document.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating whether action was successful.</returns>
        Task<CouchDbDeleteDocumentResult> DeleteDocumentAsync(string database, string documentId, string revisionId, CancellationToken cancellationToken = default);
    }
}
