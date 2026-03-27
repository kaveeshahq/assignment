using FlowDesk.Domain;
using Microsoft.EntityFrameworkCore;
using Task = FlowDesk.Domain.Task;

namespace FlowDesk.Data.Repositories;

public class TaskRepository : Repository<Task>, ITaskRepository
{
    public TaskRepository(FlowDeskDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Task>> GetByProjectIdAsync(int projectId)
    {
        return await DbSet
            .Where(t => t.ProjectId == projectId && !t.IsArchived)
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Task>> GetByStatusAsync(TaskStatus status)
    {
        return await DbSet
            .Where(t => t.Status == status && !t.IsArchived)
            .ToListAsync();
    }

    public async Task<IEnumerable<Task>> GetByAssigneeAsync(int userId)
    {
        return await DbSet
            .Where(t => t.AssignedToUserId == userId && !t.IsArchived)
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Task>> GetArchivedByProjectAsync(int projectId)
    {
        return await DbSet
            .Where(t => t.ProjectId == projectId && t.IsArchived)
            .OrderByDescending(t => t.ArchivedAt)
            .ToListAsync();
    }

    public async Task<Task?> GetByIdWithDetailsAsync(int id)
    {
        return await DbSet
            .Include(t => t.Project)
            .Include(t => t.AssignedToUser)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}
