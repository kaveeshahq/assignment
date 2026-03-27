using FlowDesk.Domain;
using Microsoft.EntityFrameworkCore;

namespace FlowDesk.Data.Repositories;

public class ProjectRepository : Repository<Project>, IProjectRepository
{
    public ProjectRepository(FlowDeskDbContext context) : base(context)
    {
    }

    public async Task<Project?> GetByIdWithTasksAsync(int id)
    {
        return await DbSet
            .Include(p => p.Tasks.Where(t => !t.IsArchived))
            .Include(p => p.CreatedByUser)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Project>> GetByCreatedByUserAsync(int userId)
    {
        return await DbSet
            .Where(p => p.CreatedByUserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }
}
