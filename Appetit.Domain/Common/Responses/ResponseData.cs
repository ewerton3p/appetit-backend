namespace Appetit.Domain.Common.Responses
{
    public class ResponseData<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; } = true;
        public Pagination? Pagination { get; set; }
        public string? Details { get; set; }

    }
}
