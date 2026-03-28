namespace FlowDesk.Services.DTOs;

public record CreateTaskRequest
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public int ProjectId { get; set; }
    public int? AssignedToUserId { get; set; }
}

public record UpdateTaskRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public int? AssignedToUserId { get; set; }
}

public record TaskResponse
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int Priority { get; set; }
    public string Status { get; set; } = "Todo";
    public DateTime? DueDate { get; set; }
    public int ProjectId { get; set; }
    public int? AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; }
    public bool IsArchived { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
