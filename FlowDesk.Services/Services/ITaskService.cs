using FlowDesk.Services.DTOs;

namespace FlowDesk.Services.Services;

public interface ITaskService
{
    System.Threading.Tasks.Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request, int currentUserId);
    System.Threading.Tasks.Task<TaskResponse?> GetTaskByIdAsync(int id);
    System.Threading.Tasks.Task<IEnumerable<TaskResponse>> GetProjectTasksAsync(int projectId);
    System.Threading.Tasks.Task<IEnumerable<TaskResponse>> GetUserTasksAsync(int userId);
    System.Threading.Tasks.Task<IEnumerable<TaskResponse>> GetTasksByStatusAsync(string status);
    System.Threading.Tasks.Task<TaskResponse> UpdateTaskAsync(int id, UpdateTaskRequest request, int currentUserId);
    System.Threading.Tasks.Task<TaskResponse> UpdateTaskStatusAsync(int id, string newStatus, int currentUserId);
    System.Threading.Tasks.Task<TaskResponse> AssignTaskAsync(int id, int userId, int currentUserId);
    System.Threading.Tasks.Task<TaskResponse> UnassignTaskAsync(int id, int currentUserId);
    System.Threading.Tasks.Task<TaskResponse> ArchiveTaskAsync(int id, int currentUserId);
    System.Threading.Tasks.Task<IEnumerable<TaskResponse>> GetArchivedTasksAsync(int projectId);
    System.Threading.Tasks.Task DeleteTaskAsync(int id, int currentUserId);
}
