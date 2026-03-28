using FlowDesk.Data.UnitOfWork;
using FlowDesk.Domain;
using FlowDesk.Domain.Exceptions;
using FlowDesk.Services.DTOs;
using Microsoft.Extensions.Logging;
using Task = FlowDesk.Domain.Task;

namespace FlowDesk.Services.Services;

public class TaskService : ITaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TaskService> _logger;

    public TaskService(IUnitOfWork unitOfWork, ILogger<TaskService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request, int currentUserId)
    {
        _logger.LogInformation("Creating task '{Title}' in project {ProjectId}", request.Title, request.ProjectId);

        var project = await _unitOfWork.Projects.GetByIdAsync(request.ProjectId);
        if (project == null)
            throw new ProjectNotFoundException(request.ProjectId);

        var task = new Task
        {
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            DueDate = request.DueDate,
            ProjectId = request.ProjectId,
            AssignedToUserId = request.AssignedToUserId,
            Status = TaskStatus.Todo
        };

        if (request.AssignedToUserId.HasValue)
        {
            var assignedUser = await _unitOfWork.Users.GetByIdAsync(request.AssignedToUserId.Value);
            if (assignedUser == null)
                throw new UserNotFoundException(request.AssignedToUserId.Value);
        }

        await _unitOfWork.Tasks.AddAsync(task);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Task {TaskId} created successfully", task.Id);
        return MapToResponse(task);
    }

    public async Task<TaskResponse?> GetTaskByIdAsync(int id)
    {
        var task = await _unitOfWork.Tasks.GetByIdWithDetailsAsync(id);
        if (task == null)
            return null;

        return MapToResponse(task);
    }

    public async Task<IEnumerable<TaskResponse>> GetProjectTasksAsync(int projectId)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(projectId);
        if (project == null)
            throw new ProjectNotFoundException(projectId);

        var tasks = await _unitOfWork.Tasks.GetByProjectIdAsync(projectId);
        return tasks.Select(MapToResponse);
    }

    public async Task<IEnumerable<TaskResponse>> GetUserTasksAsync(int userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new UserNotFoundException(userId);

        var tasks = await _unitOfWork.Tasks.GetByAssigneeAsync(userId);
        return tasks.Select(MapToResponse);
    }

    public async Task<IEnumerable<TaskResponse>> GetTasksByStatusAsync(string status)
    {
        if (!Enum.TryParse<TaskStatus>(status, true, out var taskStatus))
            throw new InvalidTaskException($"Invalid status: {status}");

        var tasks = await _unitOfWork.Tasks.GetByStatusAsync(taskStatus);
        return tasks.Select(MapToResponse);
    }

    public async Task<TaskResponse> UpdateTaskAsync(int id, UpdateTaskRequest request, int currentUserId)
    {
        _logger.LogInformation("Updating task {TaskId}", id);

        var task = await _unitOfWork.Tasks.GetByIdWithDetailsAsync(id);
        if (task == null)
            throw new TaskNotFoundException(id);

        if (!string.IsNullOrEmpty(request.Title))
            task.Title = request.Title;

        if (!string.IsNullOrEmpty(request.Description))
            task.Description = request.Description;

        if (request.Priority.HasValue)
        {
            if (request.Priority.Value < 1 || request.Priority.Value > 5)
                throw new InvalidPriorityException();
            task.Priority = request.Priority.Value;
        }

        if (request.DueDate.HasValue)
        {
            if (request.DueDate.Value <= DateTime.UtcNow)
                throw new DueDateInPastException();
            task.DueDate = request.DueDate.Value;
        }

        if (request.AssignedToUserId.HasValue && request.AssignedToUserId.Value != task.AssignedToUserId)
        {
            var assignedUser = await _unitOfWork.Users.GetByIdAsync(request.AssignedToUserId.Value);
            if (assignedUser == null)
                throw new UserNotFoundException(request.AssignedToUserId.Value);
            task.AssignedToUserId = request.AssignedToUserId.Value;
        }

        task.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Tasks.Update(task);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Task {TaskId} updated successfully", task.Id);
        return MapToResponse(task);
    }

    public async Task<TaskResponse> UpdateTaskStatusAsync(int id, string newStatus, int currentUserId)
    {
        _logger.LogInformation("Updating task {TaskId} status to {Status}", id, newStatus);

        var task = await _unitOfWork.Tasks.GetByIdWithDetailsAsync(id);
        if (task == null)
            throw new TaskNotFoundException(id);

        if (!Enum.TryParse<TaskStatus>(newStatus, true, out var targetStatus))
            throw new InvalidTaskException($"Invalid status: {newStatus}");

        if (!IsValidStatusTransition(task.Status, targetStatus))
            throw new InvalidStatusTransitionException(task.Status.ToString(), newStatus);

        task.Status = targetStatus;
        task.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Tasks.Update(task);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Task {TaskId} status updated to {Status}", task.Id, newStatus);
        return MapToResponse(task);
    }

    public async Task<TaskResponse> AssignTaskAsync(int id, int userId, int currentUserId)
    {
        _logger.LogInformation("Assigning task {TaskId} to user {UserId}", id, userId);

        var task = await _unitOfWork.Tasks.GetByIdWithDetailsAsync(id);
        if (task == null)
            throw new TaskNotFoundException(id);

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new UserNotFoundException(userId);

        if (!user.IsActive)
            throw new InvalidTaskException("Cannot assign task to inactive user");

        task.AssignedToUserId = userId;
        task.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Tasks.Update(task);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Task {TaskId} assigned to user {UserId}", task.Id, userId);
        return MapToResponse(task);
    }

    public async Task<TaskResponse> UnassignTaskAsync(int id, int currentUserId)
    {
        _logger.LogInformation("Unassigning task {TaskId}", id);

        var task = await _unitOfWork.Tasks.GetByIdWithDetailsAsync(id);
        if (task == null)
            throw new TaskNotFoundException(id);

        task.AssignedToUserId = null;
        task.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Tasks.Update(task);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Task {TaskId} unassigned", task.Id);
        return MapToResponse(task);
    }

    public async Task<TaskResponse> ArchiveTaskAsync(int id, int currentUserId)
    {
        _logger.LogInformation("Archiving task {TaskId}", id);

        var task = await _unitOfWork.Tasks.GetByIdWithDetailsAsync(id);
        if (task == null)
            throw new TaskNotFoundException(id);

        task.IsArchived = true;
        task.ArchivedAt = DateTime.UtcNow;
        task.Status = TaskStatus.Archived;
        task.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Tasks.Update(task);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Task {TaskId} archived", task.Id);
        return MapToResponse(task);
    }

    public async Task<IEnumerable<TaskResponse>> GetArchivedTasksAsync(int projectId)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(projectId);
        if (project == null)
            throw new ProjectNotFoundException(projectId);

        var tasks = await _unitOfWork.Tasks.GetArchivedByProjectAsync(projectId);
        return tasks.Select(MapToResponse);
    }

    public async Task DeleteTaskAsync(int id, int currentUserId)
    {
        _logger.LogInformation("Deleting task {TaskId}", id);

        var task = await _unitOfWork.Tasks.GetByIdAsync(id);
        if (task == null)
            throw new TaskNotFoundException(id);

        _unitOfWork.Tasks.Remove(task);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Task {TaskId} deleted", task.Id);
    }

    private static bool IsValidStatusTransition(TaskStatus from, TaskStatus to)
    {
        return (from, to) switch
        {
            (TaskStatus.Todo, TaskStatus.InProgress) => true,
            (TaskStatus.Todo, TaskStatus.Archived) => true,
            (TaskStatus.InProgress, TaskStatus.Done) => true,
            (TaskStatus.InProgress, TaskStatus.Todo) => true,
            (TaskStatus.InProgress, TaskStatus.Archived) => true,
            (TaskStatus.Done, TaskStatus.Archived) => true,
            (TaskStatus.Archived, _) => false,
            (_, TaskStatus.Archived) => true,
            _ => false
        };
    }

    private static TaskResponse MapToResponse(Task task)
    {
        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Priority = task.Priority,
            Status = task.Status.ToString(),
            DueDate = task.DueDate,
            ProjectId = task.ProjectId,
            AssignedToUserId = task.AssignedToUserId,
            AssignedToUserName = task.AssignedToUser?.FullName,
            IsArchived = task.IsArchived,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}
