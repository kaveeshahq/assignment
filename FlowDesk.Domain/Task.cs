namespace FlowDesk.Domain;

public class Task : BaseEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int Priority { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Todo;
    public DateTime? DueDate { get; set; }
    public int ProjectId { get; set; }
    public int? AssignedToUserId { get; set; }
    public bool IsArchived { get; set; } = false;
    public DateTime? ArchivedAt { get; set; }

    public required Project Project { get; set; }
    public User? AssignedToUser { get; set; }
}
