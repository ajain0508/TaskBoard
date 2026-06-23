using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Api.Data;
using TaskBoard.Api.Models;


namespace TaskBoard.Api.Controllers;


[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{

    private readonly AppDbContext _context;
    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {

        var result = new
        {
            totalProjects =
                await _context.Projects.CountAsync(),


            totalTasks =
                await _context.Tasks.CountAsync(),


            completedTasks =
                await _context.Tasks
                .CountAsync(x=>x.Status == Status.Done),


            overdueTasks =
                await _context.Tasks
                .CountAsync(x =>
                    x.DueDate < DateTime.UtcNow &&
                    x.Status != Status.Done)
        };


        return Ok(result);
    }

}