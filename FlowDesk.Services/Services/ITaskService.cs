using FlowDesk.Services.DTOs;

namespace FlowDesk.Services.Services;

public interface ITaskService
{
    Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request, int currentUserId);
    Task<TaskResponse?> GetTaskByIdAsync(int id);
    Task<IEnumerable<TaskResponse>> GetProjectTasksAsync(int projectId);
    Task<IEnumerable<TaskResponse>> GetUserTasksAsync(int userId);
    Task<IEnumerable<TaskResponse>> GetTasksByStatusAsync(string status);
    Task<TaskResponse> UpdateTaskAsync(int id, UpdateTaskRequest request, int currentUserId);
    Task<TaskResponse> UpdateTaskStatusAsync(int id, string newStatus, int currentUserId);
    Task<TaskResponse> AssignTaskAsync(int id, int userId, int currentUserId);
    Task<TaskResponse> UnassignTaskAsync(int id, int currentUserId);
    Task<TaskResponse> ArchiveTaskAsync(int id, int currentUserId);
    Task<IEnumerable<TaskResponse>> GetArchivedTasksAsync(int projectId);
    Task DeleteTaskAsync(int id, int currentUserId);
}
