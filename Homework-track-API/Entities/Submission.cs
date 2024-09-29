using System.ComponentModel.DataAnnotations;

namespace Homework_track_API.Entities;

public class Submission
{
    [Key]
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int HomeworkId { get; set; }
    public string? SubmissionFilePath { get; set; }
    
}