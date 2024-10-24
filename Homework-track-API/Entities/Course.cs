using System.ComponentModel.DataAnnotations;
using Homework_track_API.Enums;

namespace Homework_track_API.Entities;

public class Course
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int TeacherId { get; set; }
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    [Required]
    public string Code { get; set; }

    public CourseStatus Status { get; set; } = CourseStatus.Active;
    public ICollection<StudentCourse>? StudentCourses { get; set; } 
    public ICollection<Homework>? Homeworks { get; set; } 
}