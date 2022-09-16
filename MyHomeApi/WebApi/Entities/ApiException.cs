using System.Net;
using System.Runtime.Serialization;
using MyHomeApi.Infrastructure.Models;

namespace MyHomeApi.Entities
{
    [Serializable]
    public class ApiException : Exception
    {
        public HttpErrorMessage Error { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public HttpResponseMessage Response { get; set; }

        public ApiException(HttpStatusCode statusCode, HttpErrorMessage error, HttpResponseMessage response) : base(error.Message)
        {
            Error = error;
            StatusCode = statusCode;
            Response = response;
        }
    }
}