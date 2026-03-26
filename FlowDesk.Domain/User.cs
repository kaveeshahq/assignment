namespace FlowDesk.Domain;

public class User : BaseEntity
{
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; } = UserRole.TeamMember;
    public bool IsActive { get; set; } = true;

    public ICollection<Task> AssignedTasks { get; set; } = new List<Task>();
    public ICollection<Project> CreatedProjects { get; set; } = new List<Project>();
}
