using Microsoft.EntityFrameworkCore;
using TaskBoard.Api.Models;

namespace TaskBoard.Api.Data;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {

    }
    public DbSet<Project> Projects { get; set; }

    public DbSet<TaskItem> Tasks { get; set; }

    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.Entity<Project>()
            .HasIndex(x => x.Name)
            .IsUnique();



        builder.Entity<Project>()
            .HasMany(x => x.Tasks)
            .WithOne(x => x.Project)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.Entity<TaskItem>()
            .HasMany(x => x.Comments)
            .WithOne(x => x.Task)
            .HasForeignKey(x => x.TaskId)
            .OnDelete(DeleteBehavior.Cascade);



        base.OnModelCreating(builder);
    }
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {

        var entries = ChangeTracker
            .Entries();


        foreach(var entry in entries)
        {

            if(entry.Entity is Project project)
            {
                if(entry.State == EntityState.Added)
                    project.CreatedAt = DateTime.UtcNow;
            }


            if(entry.Entity is TaskItem task)
            {

                if(entry.State == EntityState.Added)
                {
                    task.CreatedAt = DateTime.UtcNow;
                }


                if(entry.State == EntityState.Modified)
                {
                    task.UpdatedAt = DateTime.UtcNow;
                }

            }

            if(entry.Entity is Comment comment)
            {
                if(entry.State == EntityState.Added)
                    comment.CreatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);

    }

}