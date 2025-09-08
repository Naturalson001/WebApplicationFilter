using System.Net;

namespace WebApplicationFilter.Api.src.Middleware
{
     public class CustomException : Exception
    {
        private HttpStatusCode _httpStatusCode = HttpStatusCode.InternalServerError;
        public CustomException(string message, Exception inner) : base(message, inner)
        {

        }
        public CustomException(string message, HttpStatusCode httpStatusCode)
                : base(message)
        {
            SetStatusCode(httpStatusCode);
        }
        public CustomException(string message)
            : base($"[{message}]")
        {
        }

        private void SetStatusCode(HttpStatusCode code) =>
        _httpStatusCode = code;

        
        public HttpStatusCode GetStatusCode() => _httpStatusCode;
    }
}