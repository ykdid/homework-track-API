using System.ComponentModel.DataAnnotations;

namespace Homework_track_API.DTOs;

public class CreateCourse
{
    [Required]
    public int TeacherId { get; set; }
    
    [Required]
    [MinLength(3, ErrorMessage = "Course name must be at least 3 characters long.")]
    public string Name { get; set; }
    
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
}
