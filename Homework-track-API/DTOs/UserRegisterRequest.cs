using System.ComponentModel.DataAnnotations;
using Homework_track_API.Enums;

namespace Homework_track_API.DTOs;

public class UserRegisterRequest
{
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

    [Required]
    public UserRole Role { get; set; }
}