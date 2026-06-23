using Microsoft.AspNetCore.Mvc;
using TaskBoard.Api.DTOs.Project;
using TaskBoard.Api.Services.Interfaces;


namespace TaskBoard.Api.Controllers;


[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{

    private readonly IProjectService _service;

    public ProjectsController(IProjectService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
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
        CreateProjectDto dto)
    {

        var result = await _service.Create(dto);

        return CreatedAtAction(
            nameof(GetById),
            new {id=result.Id},
            result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateProjectDto dto)
    {

        var updated = await _service.Update(id,dto);

        if(!updated)
            return NotFound();

        return NoContent();
    }





    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {

        var deleted = await _service.Delete(id);


        if(!deleted)
            return NotFound();


        return NoContent();
    }

}