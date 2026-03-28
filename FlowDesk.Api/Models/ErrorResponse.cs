namespace FlowDesk.Api.Models;

public record ErrorResponse
{
    public required string Message { get; set; }
    public required string Code { get; set; }
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public Dictionary<string, string[]>? Errors { get; set; }
}
