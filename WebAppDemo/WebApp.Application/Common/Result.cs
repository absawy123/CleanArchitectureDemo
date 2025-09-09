namespace WebApp.Application.Common
{
    public class Result
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static Result Success(string message = null!) =>
            new Result { Succeeded = true, Message = message };

        public static Result Failure(string message, List<string> errors = null!) =>
            new Result { Succeeded = false, Message = message, Errors = errors ?? new() };
    }


    public class Result<T> : Result
    {
        public T Data { get; set; }

        public static Result<T> Success(T data, string message = null) =>
            new Result<T> { Succeeded = true, Data = data, Message = message };

        public new static Result<T> Failure(string message, List<string> errors = null) =>
            new Result<T> { Succeeded = false, Message = message, Errors = errors ?? new() };
    }


}
