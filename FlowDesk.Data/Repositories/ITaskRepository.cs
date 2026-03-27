using FlowDesk.Domain;

namespace FlowDesk.Data.Repositories;

public interface ITaskRepository : IRepository<Task>
{
    Task<IEnumerable<Task>> GetByProjectIdAsync(int projectId);
    Task<IEnumerable<Task>> GetByStatusAsync(TaskStatus status);
    Task<IEnumerable<Task>> GetByAssigneeAsync(int userId);
    Task<IEnumerable<Task>> GetArchivedByProjectAsync(int projectId);
    Task<Task?> GetByIdWithDetailsAsync(int id);
}
