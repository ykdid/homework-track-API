using System.ComponentModel.DataAnnotations;

namespace Homework_track_API.DTOs;

public class UserLoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}