using System.Net;

namespace SpravaProjektovUI.Infrastructure
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public string? Detail { get; }
        
        public Dictionary<string, object>? Extensions { get; }

        public ApiException(
            string message,
            HttpStatusCode statusCode,
            string? detail,
            Dictionary<string, object>? extensions)
            : base(message)
        {
            StatusCode = statusCode;
            Detail = detail;
            Extensions = extensions;
        }
    }
}
