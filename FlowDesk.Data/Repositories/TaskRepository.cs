using FlowDesk.Domain;
using Microsoft.EntityFrameworkCore;
using Task = FlowDesk.Domain.Task;
using TaskStatus = FlowDesk.Domain.TaskStatus;

namespace FlowDesk.Data.Repositories;

public class TaskRepository : Repository<Task>, ITaskRepository
{
    public TaskRepository(FlowDeskDbContext context) : base(context)
    {
    }

    public async System.Threading.Tasks.Task<IEnumerable<Task>> GetByProjectIdAsync(int projectId)
    {
        return await DbSet
            .Where(t => t.ProjectId == projectId && !t.IsArchived)
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<IEnumerable<Task>> GetByStatusAsync(TaskStatus status)
    {
        return await DbSet
            .Where(t => t.Status == status && !t.IsArchived)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<IEnumerable<Task>> GetByAssigneeAsync(int userId)
    {
        return await DbSet
            .Where(t => t.AssignedToUserId == userId && !t.IsArchived)
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<IEnumerable<Task>> GetArchivedByProjectAsync(int projectId)
    {
        return await DbSet
            .Where(t => t.ProjectId == projectId && t.IsArchived)
            .OrderByDescending(t => t.ArchivedAt)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<Task?> GetByIdWithDetailsAsync(int id)
    {
        return await DbSet
            .Include(t => t.Project)
            .Include(t => t.AssignedToUser)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}
