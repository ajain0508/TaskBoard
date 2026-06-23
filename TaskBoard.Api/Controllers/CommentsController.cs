using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Api.Data;
using TaskBoard.Api.DTOs.Comment;
using TaskBoard.Api.Models;


namespace TaskBoard.Api.Controllers;


[ApiController]
[Route("api/tasks/{taskId}/comments")]
public class CommentsController : ControllerBase
{

    private readonly AppDbContext _context;


    public CommentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int taskId)
    {

        var comments =
            await _context.Comments
            .Where(x => x.TaskId == taskId)
            .Select(x => new CommentResponseDto
            {
                Id = x.Id,
                Author = x.Author,
                Body = x.Body,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();


        return Ok(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        int taskId,
        CreateCommentDto dto)
    {


        var comment = new Comment
        {
            TaskId = taskId,
            Author = dto.Author,
            Body = dto.Body
        };

        _context.Comments.Add(comment);

        await _context.SaveChangesAsync();

        return Ok(comment);
    }

}