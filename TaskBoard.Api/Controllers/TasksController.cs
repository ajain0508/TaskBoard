using Microsoft.AspNetCore.Mvc;
using TaskBoard.Api.DTOs.Task;
using TaskBoard.Api.Services.Interfaces;


namespace TaskBoard.Api.Controllers;


[ApiController]
[Route("api/projects/{projectId}/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _service;

    public TasksController(ITaskService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks(
        int projectId,
        string? status,
        string? priority,
        string? sortBy,
        string? sortDir,
        int page = 1,
        int pageSize = 10)
    {

        var result = await _service.GetTasks(
            projectId,
            status,
            priority,
            sortBy,
            sortDir,
            page,
            pageSize
        );


        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {

        var result = await _service.GetById(id);


        if(result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        int projectId,
        CreateTaskDto dto)
    {

        var result =
            await _service.Create(projectId,dto);


        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateTaskDto dto)
    {

        var updated =
            await _service.Update(id,dto);

        if(!updated)
            return NotFound();

        return NoContent();
    }





    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {

        var deleted =
            await _service.Delete(id);


        if(!deleted)
            return NotFound();


        return NoContent();
    }

}