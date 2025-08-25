//



using System;
using System.Net;

namespace ServiceMeshOrchestrator.Models.Exceptions
{
    //
    // Summary:
    //     Exception that returns meaningful content when an HttpRequest is not successful.
    public class SimpleHttpResponseException : Exception
    {
        //
        // Summary:
        //     Initializes a new instance of the GV.SCS.Shared.Exceptions.SimpleHttpResponseException
        //     class.
        //
        // Parameters:
        //   statusCode:
        //     Status code.
        //
        //   content:
        //     Content.
        public SimpleHttpResponseException(HttpStatusCode statusCode, string content) { }

        //
        // Summary:
        //     Gets the status code.
        public HttpStatusCode StatusCode { get; }
    }
}