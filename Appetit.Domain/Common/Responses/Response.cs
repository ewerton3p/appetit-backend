namespace Appetit.Domain.Common.Responses
{
    public class Response
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; } = true;
        public string? Details { get; set; }
    }
}
