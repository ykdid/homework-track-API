using System.ComponentModel.DataAnnotations;

namespace Homework_track_API.Entities;

public class Submission
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int StudentId { get; set; }
    [Required]
    public int HomeworkId { get; set; }
    public string? SubmissionFilePath { get; set; }
    public int? Mark { get; set; }
    
}