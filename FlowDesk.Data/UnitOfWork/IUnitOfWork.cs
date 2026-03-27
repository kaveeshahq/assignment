namespace FlowDesk.Data.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    ITaskRepository Tasks { get; }
    IProjectRepository Projects { get; }
    IUserRepository Users { get; }

    Task<int> SaveChangesAsync();
    Task<bool> BeginTransactionAsync();
    Task<bool> CommitTransactionAsync();
    Task<bool> RollbackTransactionAsync();
}
