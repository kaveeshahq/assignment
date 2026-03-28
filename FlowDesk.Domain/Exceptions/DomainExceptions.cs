namespace FlowDesk.Domain.Exceptions;

public abstract class DomainException : Exception
{
    public string Code { get; }

    protected DomainException(string message, string code) : base(message)
    {
        Code = code;
    }
}

public class InvalidTaskException : DomainException
{
    public InvalidTaskException(string message) : base(message, "INVALID_TASK") { }
}

public class TaskNotFoundException : DomainException
{
    public TaskNotFoundException(int taskId) : base($"Task with ID {taskId} not found", "TASK_NOT_FOUND") { }
}

public class ProjectNotFoundException : DomainException
{
    public ProjectNotFoundException(int projectId) : base($"Project with ID {projectId} not found", "PROJECT_NOT_FOUND") { }
}

public class UserNotFoundException : DomainException
{
    public UserNotFoundException(int userId) : base($"User with ID {userId} not found", "USER_NOT_FOUND") { }
}

public class UserNotFoundByEmailException : DomainException
{
    public UserNotFoundByEmailException(string email) : base($"User with email {email} not found", "USER_NOT_FOUND_BY_EMAIL") { }
}

public class InvalidStatusTransitionException : DomainException
{
    public InvalidStatusTransitionException(string from, string to) : base($"Cannot transition from {from} to {to}", "INVALID_STATUS_TRANSITION") { }
}

public class DueDateInPastException : DomainException
{
    public DueDateInPastException() : base("Due date cannot be in the past", "DUE_DATE_IN_PAST") { }
}

public class InvalidPriorityException : DomainException
{
    public InvalidPriorityException() : base("Priority must be between 1 and 5", "INVALID_PRIORITY") { }
}

public class AccessDeniedException : DomainException
{
    public AccessDeniedException(string message) : base(message, "ACCESS_DENIED") { }
}

public class DuplicateEmailException : DomainException
{
    public DuplicateEmailException(string email) : base($"Email {email} is already in use", "DUPLICATE_EMAIL") { }
}
