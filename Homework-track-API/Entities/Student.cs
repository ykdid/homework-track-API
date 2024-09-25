namespace Homework_track_API.Entities;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string StudentNumber { get; set; }
    public string? ProfileImagePath { get; set; }
        
}