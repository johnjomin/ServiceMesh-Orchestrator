using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServiceMeshOrchestrator.Clients;
using ServiceMeshOrchestrator.Models.Exceptions;
using ServiceMeshOrchestrator.Models.Modeller;

namespace ServiceMeshOrchestrator.Services
{
    public class MongoDbClient: IMongoDbClient
    {
        private static readonly string RevisionKey = "_rev";
        private const string InitialRevision = "1";
        private static readonly string IdKey = "_id";
        
        

        private static readonly JsonWriterSettings MongoDbToJsonWriterSetting = new JsonWriterSettings()
        {
            OutputMode = JsonOutputMode.Strict,
        };
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            Converters = { new StringEnumConverter() },
            NullValueHandling = NullValueHandling.Ignore,
        };

        private readonly JsonSerializer _jsonSerializer = JsonSerializer.Create(JsonSerializerSettings);

        private IMongoDatabase Connect()
        {

            MongoClient client = new MongoClient("mongodb+srv://admin:admin@personal-cluster.0ekfl.mongodb.net/?retryWrites=true&w=majority");
            IMongoDatabase _database = client.GetDatabase("polystream");

            return _database;
        }
        /// <summary>
        /// Gets requested documents.
        /// </summary>
        /// <param name="database">Name of collection.</param>
        /// <param name="query">Mongo filter.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Documents result.</returns>
        /// <exception cref="HttpRequestException">Thrown if status isn't successful.</exception>
        public async Task<CouchDbGetDocumentsResult> GetDocumentsAsync(string database, string query, CancellationToken cancellationToken = default)
        {
            var _database = Connect();

            IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument>(database);

            bool success = BsonDocument.TryParse(query, out BsonDocument filter);
            if (!success)
            {
                throw new SimpleHttpResponseException(HttpStatusCode.InternalServerError, "Unable to parse query to grab multiple documents.");
            }

            IAsyncCursor<BsonDocument> findResultAsync = await collection.FindAsync(query);

            List<CouchDbRow> documents = new List<CouchDbRow>();
            CouchDbGetDocumentsResult results = new CouchDbGetDocumentsResult()
            {
                Offset = 0,
                TotalRows = 0,
                Rows = new List<CouchDbRow>(),
            };

            await findResultAsync.ForEachAsync(result =>
            {
                documents.Add(new CouchDbRow
                {
                    Id = result.GetValue(IdKey).AsString,
                    Key = result.GetValue(IdKey).AsString,
                    Doc = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(
                        result.ToJson(MongoDbToJsonWriterSetting)),
                });

                results.TotalRows++;
            });
            results.Rows = documents;

            Console.WriteLine($"Retrieved {documents.Count} from the collection {database} with the query {query}");

            return results;
        }

        /// <summary>
        /// Get requested document.
        /// </summary>
        /// <param name="database">Collection where document is located.</param>
        /// <param name="documentId">Id of document to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Document result.</returns>
        public async Task<CouchDbGetDocumentResult> GetDocumentAsync(string database, string documentId, CancellationToken cancellationToken = default)
        {
            var _database = Connect();

            IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument>(database);

            try
            {
                BsonDocument filter = new BsonDocument(IdKey, documentId);
                IAsyncCursor<BsonDocument> findResultAsync = await collection.FindAsync(filter);
                BsonDocument findResult = findResultAsync.FirstOrDefault();
                if (findResult.ElementCount > 0)
                {
                    JsonWriterSettings jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
                    string json = findResult.ToJson(jsonWriterSettings);

                    byte[] byteArray = Encoding.UTF8.GetBytes(json);
                    MemoryStream stream = new MemoryStream(byteArray);
                    using (StreamReader streamReader = new StreamReader(stream))
                    using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                    {
                        return _jsonSerializer.Deserialize<CouchDbGetDocumentResult>(jsonTextReader);
                    }
                }

                throw new SimpleHttpResponseException(HttpStatusCode.NotFound, $"{documentId} not found");
            }
            catch (NullReferenceException)
            {
                throw new SimpleHttpResponseException(HttpStatusCode.NotFound, $"{documentId} not found");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw new SimpleHttpResponseException(HttpStatusCode.InternalServerError, e.ToString());
            }
        }

        /// <summary>
        /// Post requested documents.
        /// </summary>
        /// <param name="database">Collection name.</param>
        /// <param name="documentId">Id of document to be created.</param>
        /// <param name="content">Content body.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating whether action was successful.</returns>
        public async Task<CouchDbCreateDocumentResult> CreateDocumentAsync(string database, string documentId, object content, CancellationToken cancellationToken = default)
        {
            var _database = Connect();

            IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument>(database);

            string contentData = Newtonsoft.Json.JsonConvert.SerializeObject(content);
            BsonDocument document = BsonSerializer.Deserialize<BsonDocument>(contentData);

            if (documentId == string.Empty)
            {
                documentId = ObjectId.GenerateNewId().ToString();
            }

            if (!document.Contains(IdKey))
            {
                document.Add(new BsonElement(IdKey, new BsonString(documentId)));
            }

            if (document.GetValue(IdKey).IsBsonNull)
            {
                document.Set(IdKey, new BsonString(documentId));
            }

            if (document.Contains(RevisionKey))
            {
                throw new SimpleHttpResponseException(HttpStatusCode.BadRequest, "Cannot assign to '_rev'");
            }

            document.Add(new BsonElement(RevisionKey, new BsonString(InitialRevision)));

            try
            {
                await collection.InsertOneAsync(document);

                CouchDbCreateDocumentResult result = new CouchDbCreateDocumentResult
                {
                    IsSuccessful = true,
                    DocumentId = documentId,
                    RevisionId = InitialRevision,
                };

                return result;
            }
            catch (MongoWriteException)
            {
                throw new SimpleHttpResponseException(HttpStatusCode.Conflict, $"{documentId} already exists!");
            }
            catch (MongoException e)
            {
                Console.WriteLine(e.ToString());
                throw new SimpleHttpResponseException(HttpStatusCode.InternalServerError, e.ToString());
            }
        }

        /// <summary>
        /// Delete the specified document.
        /// </summary>
        /// <param name="database">Database where document is located.</param>
        /// <param name="documentId">Id of document to delete.</param>
        /// <param name="revisionId">Latest revision of document.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A response indicating whether action was successful.</returns>
        public async Task<CouchDbDeleteDocumentResult> DeleteDocumentAsync(string database, string documentId, string revisionId, CancellationToken cancellationToken = default)
        {
            var _database = Connect();

            IMongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument>(database);

            if (!int.TryParse(revisionId, out int revision))
            {
                throw new SimpleHttpResponseException(HttpStatusCode.NotFound, $"ID:{documentId}, Rev:{revisionId} invalid revision");
            }

            BsonDocument filter = new BsonDocument(IdKey, documentId)
            {
                new BsonElement(RevisionKey, new BsonString(revisionId)),
            };

            DeleteResult deleteResult = await collection.DeleteOneAsync(filter);
            if (deleteResult.DeletedCount == 1)
            {
                CouchDbDeleteDocumentResult result = new CouchDbDeleteDocumentResult
                {
                    IsSuccessful = true,
                    DocumentId = documentId,
                    RevisionId = revisionId,
                };
                return result;
            }

            throw new SimpleHttpResponseException(HttpStatusCode.NotFound, $"ID:{documentId}, Rev:{revision} Did not match");
        }
    }
}
