using Homework_track_API.Enums;

namespace Homework_track_API.Entities;

public class Homework
{
    public int Id { get; set; }
    public int TeacherId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? HomeworkImagePath { get; set; }
    public HomeworkStatus Status { get; set; }
    
}