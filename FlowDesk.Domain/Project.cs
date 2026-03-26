namespace FlowDesk.Domain;

public class Project : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int CreatedByUserId { get; set; }

    public required User CreatedByUser { get; set; }
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}
