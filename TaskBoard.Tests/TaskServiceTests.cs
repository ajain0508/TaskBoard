using Microsoft.EntityFrameworkCore;
using TaskBoard.Api.Data;
using TaskBoard.Api.DTOs.Task;
using TaskBoard.Api.Models;
using TaskBoard.Api.Services.Implementations;

namespace TaskBoard.Tests;

public class TaskServiceTests
{
    private AppDbContext GetContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(
                Guid.NewGuid().ToString()
            )
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateTask_ShouldCreateTask()
    {
        using var context = GetContext();
        var service = new TaskService(context);

        var project = new Project
        {
            Name="Test Project"
        };

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        var dto = new CreateTaskDto
        {
            Title="Create API",
            Description="Testing task",
            Priority=Priority.High,
            Status=Status.Todo
        };

        var result =
            await service.Create(project.Id,dto);

        Assert.NotNull(result);

        Assert.Equal(
            "Create API",
            result.Title
        );

    }

    [Fact]
    public async Task GetTaskById_ShouldReturnTask()
    {

        using var context = GetContext();
        var task = new TaskItem
        {
            Title="Login API",
            Status=Status.Todo,
            Priority=Priority.Medium
        };


        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        var service =
            new TaskService(context);

        var result =
            await service.GetById(task.Id);

        Assert.NotNull(result);

        Assert.Equal(
            "Login API",
            result.Title
        );

    }

    [Fact]
    public async Task UpdateTask_ShouldUpdateData()
    {

        using var context = GetContext();
        var task = new TaskItem
        {
            Title="Old Title",
            Status=Status.Todo,
            Priority=Priority.Low
        };

        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        var service =
            new TaskService(context);

        var dto = new UpdateTaskDto
        {
            Title="New Title",
            Status=Status.Done,
            Priority=Priority.High
        };

        var result = await service.Update(task.Id,dto);

        Assert.True(result);

        var updated =
            await context.Tasks.FindAsync(task.Id);

        Assert.Equal(
            "New Title",
            updated!.Title
        );

    }

    [Fact]
    public async Task DeleteTask_ShouldRemoveTask()
    {
        using var context = GetContext();
        var task = new TaskItem
        {
            Title="Delete Me",
            Status=Status.Todo,
            Priority=Priority.Low
        };

        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        var service = new TaskService(context);

        var deleted = await service.Delete(task.Id);
        Assert.True(deleted);

        var result = await context.Tasks.FindAsync(task.Id);

        Assert.Null(result);
    }
}