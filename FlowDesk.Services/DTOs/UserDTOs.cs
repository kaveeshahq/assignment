namespace FlowDesk.Services.DTOs;

public record CreateUserRequest
{
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public required string Password { get; set; }
}

public record UpdateUserRequest
{
    public string? FullName { get; set; }
}

public record UserResponse
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public string Role { get; set; } = "TeamMember";
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
