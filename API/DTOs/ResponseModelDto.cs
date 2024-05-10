using System.Text.Json.Serialization;

namespace API.DTOs
{
    public struct NoContent;

    public record ResponseModelDto<T>
    {
        public T? Data { get; init; }

        [JsonIgnore]
        public bool IsSuccess { get; init; }
        public List<string>? FailMessage { get; init; }

        public static ResponseModelDto<T> Success(T data)
        {
            return new ResponseModelDto<T>
            {
                Data = data,
                IsSuccess = true
            };
        }

        public static ResponseModelDto<T> Success()
        {
            return new ResponseModelDto<T>
            {
                IsSuccess = true
            };
        }

        public static ResponseModelDto<T> Fail(List<string> messages)
        {
            return new ResponseModelDto<T>
            {
                IsSuccess = false,
                FailMessage = messages
            };
        }

        public static ResponseModelDto<T> Fail(string message)
        {
            return new ResponseModelDto<T>
            {
                IsSuccess = false,
                FailMessage = [message]
            };
        }



    }
}