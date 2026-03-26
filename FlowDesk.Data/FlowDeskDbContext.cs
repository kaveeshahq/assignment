using FlowDesk.Domain;
using Microsoft.EntityFrameworkCore;
using Task = FlowDesk.Domain.Task;

namespace FlowDesk.Data;

public class FlowDeskDbContext : DbContext
{
    public FlowDeskDbContext(DbContextOptions<FlowDeskDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Task> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.Property(u => u.FullName).IsRequired().HasMaxLength(255);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Role).IsRequired();
            entity.Property(u => u.IsActive).IsRequired();

            entity.HasIndex(u => u.Email).IsUnique();

            entity.HasMany(u => u.AssignedTasks)
                .WithOne(t => t.AssignedToUser)
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(u => u.CreatedProjects)
                .WithOne(p => p.CreatedByUser)
                .HasForeignKey(p => p.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(255);
            entity.Property(p => p.Description).HasMaxLength(2000);

            entity.HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).IsRequired().HasMaxLength(255);
            entity.Property(t => t.Description).HasMaxLength(2000);
            entity.Property(t => t.Priority).IsRequired();
            entity.Property(t => t.Status).IsRequired();
            entity.Property(t => t.IsArchived).IsRequired();

            entity.HasIndex(t => new { t.ProjectId, t.Status });
            entity.HasIndex(t => new { t.ProjectId, t.AssignedToUserId });
        });
    }
}
