using Microsoft.EntityFrameworkCore;
using TaskBoard.Api.Models;

namespace TaskBoard.Api.Data;

public static class SeedData
{
    public static async Task Initialize(AppDbContext context)
    {
        if (await context.Projects.AnyAsync())
            return;


        var project1 = new Project
        {
            Name = "E-Commerce App",
            Description = "Online shopping application"
        };


        var project2 = new Project
        {
            Name = "CRM System",
            Description = "Customer management system"
        };


        context.Projects.AddRange(project1, project2);

        await context.SaveChangesAsync();

        var tasks = new List<TaskItem>
        {
            new TaskItem
            {
                ProjectId = project1.Id,
                Title="Create Login API",
                Description="Implement authentication",
                Priority=Priority.High,
                Status=Status.InProgress,
                DueDate=DateTime.UtcNow.AddDays(3)
            },

            new TaskItem
            {
                ProjectId = project1.Id,
                Title="Create Product Module",
                Priority=Priority.Medium,
                Status=Status.Todo,
                DueDate=DateTime.UtcNow.AddDays(7)
            },

            new TaskItem
            {
                ProjectId = project2.Id,
                Title="Customer CRUD",
                Priority=Priority.Critical,
                Status=Status.Review,
                DueDate=DateTime.UtcNow.AddDays(-2)
            }
        };


        context.Tasks.AddRange(tasks);

        await context.SaveChangesAsync();

        var comments = new List<Comment>
        {
            new Comment
            {
                TaskId=tasks[0].Id,
                Author="Admin",
                Body="API development started"
            },

            new Comment
            {
                TaskId=tasks[0].Id,
                Author="Developer",
                Body="Working on JWT"
            },

            new Comment
            {
                TaskId=tasks[2].Id,
                Author="Manager",
                Body="Need review"
            }
        };


        context.Comments.AddRange(comments);
        await context.SaveChangesAsync();
    }
}