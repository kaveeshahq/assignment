using FlowDesk.Services.DTOs;

namespace FlowDesk.Services.Services;

public interface IProjectService
{
    System.Threading.Tasks.Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request, int currentUserId);
    System.Threading.Tasks.Task<ProjectResponse?> GetProjectByIdAsync(int id);
    System.Threading.Tasks.Task<IEnumerable<ProjectResponse>> GetUserProjectsAsync(int userId);
    System.Threading.Tasks.Task<IEnumerable<ProjectResponse>> GetAllProjectsAsync();
    System.Threading.Tasks.Task<ProjectResponse> UpdateProjectAsync(int id, UpdateProjectRequest request, int currentUserId);
    System.Threading.Tasks.Task DeleteProjectAsync(int id, int currentUserId);
}
