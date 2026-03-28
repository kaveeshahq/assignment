using FlowDesk.Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace FlowDesk.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly FlowDeskDbContext _context;
    private IDbContextTransaction? _transaction;
    private ITaskRepository? _taskRepository;
    private IProjectRepository? _projectRepository;
    private IUserRepository? _userRepository;

    public UnitOfWork(FlowDeskDbContext context)
    {
        _context = context;
    }

    public ITaskRepository Tasks => _taskRepository ??= new TaskRepository(_context);
    public IProjectRepository Projects => _projectRepository ??= new ProjectRepository(_context);
    public IUserRepository Users => _userRepository ??= new UserRepository(_context);

    public async System.Threading.Tasks.Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async System.Threading.Tasks.Task<bool> BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
        return true;
    }

    public async System.Threading.Tasks.Task<bool> CommitTransactionAsync()
    {
        if (_transaction == null)
            return false;

        try
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
            return true;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async System.Threading.Tasks.Task<bool> RollbackTransactionAsync()
    {
        if (_transaction == null)
            return false;

        try
        {
            await _transaction.RollbackAsync();
            return true;
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }
        await _context.DisposeAsync();
    }
}
