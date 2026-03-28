using FlowDesk.Data.Repositories;

namespace FlowDesk.Data.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    ITaskRepository Tasks { get; }
    IProjectRepository Projects { get; }
    IUserRepository Users { get; }

    System.Threading.Tasks.Task<int> SaveChangesAsync();
    System.Threading.Tasks.Task<bool> BeginTransactionAsync();
    System.Threading.Tasks.Task<bool> CommitTransactionAsync();
    System.Threading.Tasks.Task<bool> RollbackTransactionAsync();
}
