namespace FlowDesk.Services.DTOs;

public record CreateProjectRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}

public record UpdateProjectRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public record ProjectResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int CreatedByUserId { get; set; }
    public string? CreatedByUserName { get; set; }
    public int TaskCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
