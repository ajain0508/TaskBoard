using Microsoft.EntityFrameworkCore;
using TaskBoard.Api.Data;
using TaskBoard.Api.DTOs.Task;
using TaskBoard.Api.Models;
using TaskBoard.Api.Services.Interfaces;

namespace TaskBoard.Api.Services.Implementations;

public class TaskService : ITaskService
{

    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<object> GetTasks(
        int projectId,
        string? status,
        string? priority,
        string? sortBy,
        string? sortDir,
        int page,
        int pageSize)
    {

        var query = _context.Tasks
            .Where(x => x.ProjectId == projectId)
            .AsQueryable();

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(x =>
                x.Status.ToString() == status);
        }

        if (!string.IsNullOrEmpty(priority))
        {
            query = query.Where(x =>
                x.Priority.ToString() == priority);
        }

        query = sortBy switch
        {
            "dueDate" =>
                sortDir == "asc"
                ? query.OrderBy(x => x.DueDate)
                : query.OrderByDescending(x => x.DueDate),


            "priority" =>
                sortDir == "asc"
                ? query.OrderBy(x => x.Priority)
                : query.OrderByDescending(x => x.Priority),


            _ =>
                query.OrderByDescending(x => x.CreatedAt)
        };

        var totalCount = await query.CountAsync();

        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new TaskResponseDto
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Title = x.Title,
                Description = x.Description,
                Priority = x.Priority,
                Status = x.Status,
                DueDate = x.DueDate,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();

        return new
        {
            data,
            page,
            pageSize,
            totalCount,
            totalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize)
        };

    }

    public async Task<TaskResponseDto?> GetById(int id)
    {

        var task = await _context.Tasks
            .FirstOrDefaultAsync(x => x.Id == id);



        if (task == null)
            return null;

        return new TaskResponseDto
        {
            Id = task.Id,
            ProjectId = task.ProjectId,
            Title = task.Title,
            Description = task.Description,
            Priority = task.Priority,
            Status = task.Status,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };

    }

    public async Task<TaskResponseDto> Create(
        int projectId,
        CreateTaskDto dto)
    {

        var task = new TaskItem
        {
            ProjectId = projectId,
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            Status = dto.Status,
            DueDate = dto.DueDate
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return new TaskResponseDto
        {
            Id = task.Id,
            ProjectId = task.ProjectId,
            Title = task.Title,
            Description = task.Description,
            Priority = task.Priority,
            Status = task.Status,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };

    }

    public async Task<bool> Update(
        int id,
        UpdateTaskDto dto)
    {

        var task = await _context.Tasks
            .FindAsync(id);

        if (task == null)
            return false;

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Priority = dto.Priority;
        task.Status = dto.Status;
        task.DueDate = dto.DueDate;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {

        var task = await _context.Tasks
            .FindAsync(id);

        if (task == null)
            return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return true;
    }
}