using System.ComponentModel.DataAnnotations;

namespace Homework_track_API.Entities;

public class Student
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public string? ProfileImagePath { get; set; }
    public ICollection<StudentCourse>? StudentCourses { get; set; }
        
}