namespace TaskBoard.Api.Models;

public class TaskItem
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public Project? Project { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Priority Priority { get; set; }

    public Status Status { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }


    public ICollection<Comment> Comments { get; set; }
        = new List<Comment>();
}

public enum Priority
{
    Low,
    Medium,
    High,
    Critical
}


public enum Status
{
    Todo,
    InProgress,
    Review,
    Done
}