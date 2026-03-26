using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FlowDesk.Data;

public class FlowDeskDbContextFactory : IDesignTimeDbContextFactory<FlowDeskDbContext>
{
    public FlowDeskDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FlowDeskDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FlowDeskDb;Trusted_Connection=true;");

        return new FlowDeskDbContext(optionsBuilder.Options);
    }
}
