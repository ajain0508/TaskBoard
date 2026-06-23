using TaskBoard.Api.DTOs.Project;

namespace TaskBoard.Api.Services.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectResponseDto>> GetAll();

    Task<ProjectResponseDto?> GetById(int id);

    Task<ProjectResponseDto> Create(CreateProjectDto dto);

    Task<bool> Update(int id, UpdateProjectDto dto);

    Task<bool> Delete(int id);
}