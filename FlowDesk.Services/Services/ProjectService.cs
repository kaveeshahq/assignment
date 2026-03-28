using FlowDesk.Data.UnitOfWork;
using FlowDesk.Domain;
using FlowDesk.Domain.Exceptions;
using FlowDesk.Services.DTOs;
using Microsoft.Extensions.Logging;
using Task = FlowDesk.Domain.Task;

namespace FlowDesk.Services.Services;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProjectService> _logger;

    public ProjectService(IUnitOfWork unitOfWork, ILogger<ProjectService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async System.Threading.Tasks.Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request, int currentUserId)
    {
        _logger.LogInformation("Creating project '{Name}' for user {UserId}", request.Name, currentUserId);

        var user = await _unitOfWork.Users.GetByIdAsync(currentUserId);
        if (user == null)
            throw new UserNotFoundException(currentUserId);

        var project = new Project
        {
            Name = request.Name,
            Description = request.Description,
            CreatedByUserId = currentUserId,
            CreatedByUser = user
        };

        await _unitOfWork.Projects.AddAsync(project);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Project {ProjectId} created successfully", project.Id);
        return MapToResponse(project, 0);
    }

    public async System.Threading.Tasks.Task<ProjectResponse?> GetProjectByIdAsync(int id)
    {
        var project = await _unitOfWork.Projects.GetByIdWithTasksAsync(id);
        if (project == null)
            return null;

        var taskCount = project.Tasks.Count(t => !t.IsArchived);
        return MapToResponse(project, taskCount);
    }

    public async System.Threading.Tasks.Task<IEnumerable<ProjectResponse>> GetUserProjectsAsync(int userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new UserNotFoundException(userId);

        var projects = await _unitOfWork.Projects.GetByCreatedByUserAsync(userId);
        return projects.Select(p =>
        {
            var taskCount = p.Tasks?.Count(t => !t.IsArchived) ?? 0;
            return MapToResponse(p, taskCount);
        });
    }

    public async System.Threading.Tasks.Task<IEnumerable<ProjectResponse>> GetAllProjectsAsync()
    {
        var projects = await _unitOfWork.Projects.GetAllAsync();
        return projects.Select(p => MapToResponse(p, 0));
    }

    public async System.Threading.Tasks.Task<ProjectResponse> UpdateProjectAsync(int id, UpdateProjectRequest request, int currentUserId)
    {
        _logger.LogInformation("Updating project {ProjectId}", id);

        var project = await _unitOfWork.Projects.GetByIdAsync(id);
        if (project == null)
            throw new ProjectNotFoundException(id);

        if (!string.IsNullOrEmpty(request.Name))
            project.Name = request.Name;

        if (!string.IsNullOrEmpty(request.Description))
            project.Description = request.Description;

        project.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Projects.Update(project);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Project {ProjectId} updated successfully", project.Id);
        return MapToResponse(project, 0);
    }

    public async System.Threading.Tasks.Task DeleteProjectAsync(int id, int currentUserId)
    {
        _logger.LogInformation("Deleting project {ProjectId}", id);

        var project = await _unitOfWork.Projects.GetByIdAsync(id);
        if (project == null)
            throw new ProjectNotFoundException(id);

        _unitOfWork.Projects.Remove(project);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Project {ProjectId} deleted", id);
    }

    private static ProjectResponse MapToResponse(Project project, int taskCount)
    {
        return new ProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedByUserId = project.CreatedByUserId,
            CreatedByUserName = project.CreatedByUser?.FullName,
            TaskCount = taskCount,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt
        };
    }
}
