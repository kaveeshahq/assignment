using FlowDesk.Domain;

namespace FlowDesk.Data.Repositories;

public interface IProjectRepository : IRepository<Project>
{
    Task<Project?> GetByIdWithTasksAsync(int id);
    Task<IEnumerable<Project>> GetByCreatedByUserAsync(int userId);
}
