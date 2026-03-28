using FlowDesk.Domain;
using Task = FlowDesk.Domain.Task;
using TaskStatus = FlowDesk.Domain.TaskStatus;

namespace FlowDesk.Data.Repositories;

public interface ITaskRepository : IRepository<Task>
{
    System.Threading.Tasks.Task<IEnumerable<Task>> GetByProjectIdAsync(int projectId);
    System.Threading.Tasks.Task<IEnumerable<Task>> GetByStatusAsync(TaskStatus status);
    System.Threading.Tasks.Task<IEnumerable<Task>> GetByAssigneeAsync(int userId);
    System.Threading.Tasks.Task<IEnumerable<Task>> GetArchivedByProjectAsync(int projectId);
    System.Threading.Tasks.Task<Task?> GetByIdWithDetailsAsync(int id);
}
