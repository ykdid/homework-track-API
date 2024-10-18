using System.ComponentModel.DataAnnotations;
using Homework_track_API.Enums;

namespace Homework_track_API.Entities;

public class Homework
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int CourseId { get; set; }
    [Required]
    public string Title { get; set; }   
    public DateTime InitialDate { get; set; }
    [Required]
    public DateTime ExpireDate { get; set; }
    [MaxLength(10000)]
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public string? DocumentationPath { get; set; }
    public HomeworkStatus Status { get; set; } = HomeworkStatus.Active;

}