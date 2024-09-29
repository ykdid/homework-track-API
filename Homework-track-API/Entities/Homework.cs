using System.ComponentModel.DataAnnotations;
using Homework_track_API.Enums;

namespace Homework_track_API.Entities;

public class Homework
{
    [Key]
    public int Id { get; set; }
    public int TeacherId { get; set; }
    public string Title { get; set; }
    public DateTime InitialDate { get; set; }
    public DateTime ExpireDate { get; set; }
    [MaxLength(10000)]
    public string? Description { get; set; }
    public string? HomeworkImagePath { get; set; }
    public string? HomeworkDocumentationPath { get; set; }
    public HomeworkStatus Status { get; set; } = HomeworkStatus.Active;

}