using FlowDesk.Services.DTOs;

namespace FlowDesk.Services.Services;

public interface IProjectService
{
    Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request, int currentUserId);
    Task<ProjectResponse?> GetProjectByIdAsync(int id);
    Task<IEnumerable<ProjectResponse>> GetUserProjectsAsync(int userId);
    Task<IEnumerable<ProjectResponse>> GetAllProjectsAsync();
    Task<ProjectResponse> UpdateProjectAsync(int id, UpdateProjectRequest request, int currentUserId);
    Task DeleteProjectAsync(int id, int currentUserId);
}
