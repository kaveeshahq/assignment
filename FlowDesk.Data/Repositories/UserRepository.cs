using FlowDesk.Domain;
using Microsoft.EntityFrameworkCore;

namespace FlowDesk.Data.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(FlowDeskDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await DbSet.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await DbSet.AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        return await DbSet
            .Where(u => u.IsActive)
            .OrderBy(u => u.FullName)
            .ToListAsync();
    }
}
