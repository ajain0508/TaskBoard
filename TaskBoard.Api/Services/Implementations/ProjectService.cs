using Microsoft.EntityFrameworkCore;
using TaskBoard.Api.Data;
using TaskBoard.Api.DTOs.Project;
using TaskBoard.Api.Models;
using TaskBoard.Api.Services.Interfaces;

namespace TaskBoard.Api.Services.Implementations;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;


    public ProjectService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectResponseDto>> GetAll()
    {
        return await _context.Projects
            .Select(p => new ProjectResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ProjectResponseDto?> GetById(int id)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(x => x.Id == id);


        if (project == null)
            return null;


        return new ProjectResponseDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt
        };
    }

    public async Task<ProjectResponseDto> Create(
        CreateProjectDto dto)
    {

        bool exists = await _context.Projects
            .AnyAsync(x => x.Name == dto.Name);


        if (exists)
        {
            throw new Exception("Project name already exists");
        }

        var project = new Project
        {
            Name = dto.Name,
            Description = dto.Description
        };


        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return new ProjectResponseDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt
        };
    }

    public async Task<bool> Update(
        int id,
        UpdateProjectDto dto)
    {
        var project = await _context.Projects
            .FindAsync(id);

        if (project == null)
            return false;



        project.Name = dto.Name;
        project.Description = dto.Description;



        await _context.SaveChangesAsync();


        return true;
    }

    public async Task<bool> Delete(int id)
    {

        var project = await _context.Projects
            .FindAsync(id);


        if (project == null)
            return false;

        _context.Projects.Remove(project);

        await _context.SaveChangesAsync();
        return true;
    }
}