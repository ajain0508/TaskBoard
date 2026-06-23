using TaskBoard.Api.DTOs.Task;

namespace TaskBoard.Api.Services.Interfaces;

public interface ITaskService
{
    Task<object> GetTasks(
        int projectId,
        string? status,
        string? priority,
        string? sortBy,
        string? sortDir,
        int page,
        int pageSize
    );


    Task<TaskResponseDto?> GetById(int id);


    Task<TaskResponseDto> Create(
        int projectId,
        CreateTaskDto dto
    );


    Task<bool> Update(
        int id,
        UpdateTaskDto dto
    );


    Task<bool> Delete(int id);
}