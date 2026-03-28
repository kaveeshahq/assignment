namespace FlowDesk.Api.Models;

public record ErrorResponse
{
    public string Message { get; set; }
    public string Code { get; set; }
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public Dictionary<string, string[]>? Errors { get; set; }
}
