using FlowDesk.Services.DTOs;
using FlowDesk.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlowDesk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger<TasksController> _logger;
    private const int CurrentUserId = 1;

    public TasksController(ITaskService taskService, ILogger<TasksController> logger)
    {
        _taskService = taskService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> CreateTask(CreateTaskRequest request)
    {
        var task = await _taskService.CreateTaskAsync(request, CurrentUserId);
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> GetTask(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
            return NotFound();

        return Ok(task);
    }

    [HttpGet("project/{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetProjectTasks(int projectId)
    {
        var tasks = await _taskService.GetProjectTasksAsync(projectId);
        return Ok(tasks);
    }

    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetUserTasks(int userId)
    {
        var tasks = await _taskService.GetUserTasksAsync(userId);
        return Ok(tasks);
    }

    [HttpGet("status/{status}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetTasksByStatus(string status)
    {
        var tasks = await _taskService.GetTasksByStatusAsync(status);
        return Ok(tasks);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> UpdateTask(int id, UpdateTaskRequest request)
    {
        var task = await _taskService.UpdateTaskAsync(id, request, CurrentUserId);
        return Ok(task);
    }

    [HttpPatch("{id}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> UpdateTaskStatus(int id, [FromQuery] string status)
    {
        var task = await _taskService.UpdateTaskStatusAsync(id, status, CurrentUserId);
        return Ok(task);
    }

    [HttpPatch("{id}/assign/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> AssignTask(int id, int userId)
    {
        var task = await _taskService.AssignTaskAsync(id, userId, CurrentUserId);
        return Ok(task);
    }

    [HttpPatch("{id}/unassign")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> UnassignTask(int id)
    {
        var task = await _taskService.UnassignTaskAsync(id, CurrentUserId);
        return Ok(task);
    }

    [HttpPatch("{id}/archive")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> ArchiveTask(int id)
    {
        var task = await _taskService.ArchiveTaskAsync(id, CurrentUserId);
        return Ok(task);
    }

    [HttpGet("project/{projectId}/archived")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetArchivedTasks(int projectId)
    {
        var tasks = await _taskService.GetArchivedTasksAsync(projectId);
        return Ok(tasks);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTask(int id)
    {
        await _taskService.DeleteTaskAsync(id, CurrentUserId);
        return NoContent();
    }
}
