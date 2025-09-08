using System.Net;
using System.Text.Json.Serialization;
using WebApplicationFilter.Api.src.Common.Constant;

namespace WebApplicationFilter.Api.src.Models
{
    public class Response<T> : IResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("data")]
        public T Data { get; set; } = default;

        [JsonPropertyName("success")]
        public bool Success { get; set; } = true;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("message")]
        public string Message { get; set; } = Literals.Success;

        [JsonPropertyName("description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Description { get; set; } = string.Empty;
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("errorMessages")]
        public List<string> ErrorMessages { get; set; } = [];
    }

    public class ServiceResponse<T> : Response<T>
    {
    }

    public class PagedResponse<T> : Response<T>
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("totalRecords")]
        public int TotalRecords { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            Data = data;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public PagedResponse() { }
    }

    public class ErrorResponse
    {
        public ErrorResponse() { }
        public static void SetErrorResponse<T>(Response<T> response, string description, HttpStatusCode statusCode, bool success = false)
        {
            response.Description = description;
            response.StatusCode = statusCode;
            response.Success = success;
        }


        public static void SetErrorResponse<T>(T response, string description, HttpStatusCode statusCode, bool success = false) where T : IResponse
        {
            response.Description = description;
            response.StatusCode = statusCode;
            response.Success = success;
        }

    }
    public interface IResponse
    {
        bool Success { get; set; }
        string Description { get; set; }
        string Message { get; set; }
        HttpStatusCode StatusCode { get; set; }
    }

   public class ProcessResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }
        public bool Error => !Success;

        public static ProcessResult<T> SuccessResult(T data, string message = null) => new()
        {
            Success = true,
            Message = message,
            StatusCode = HttpStatusCode.OK,
            Data = data
        };

        public static ProcessResult<T> Failure(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new()
        {
            Success = false,
            Message = message,
            StatusCode = statusCode,
            Data = default
        };
    }

}
