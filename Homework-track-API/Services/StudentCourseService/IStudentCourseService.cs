using Homework_track_API.Entities;

namespace Homework_track_API.Services.StudentCourseService;

public interface IStudentCourseService
{
    Task<StudentCourse> AddStudentToCourse(int studentId, int courseId);
    Task<bool> RemoveStudentFromCourse(int studentId, int courseId);
    Task<List<Student>> GetStudentsByCourseId(int courseId);
    Task<List<Course>> GetCoursesByStudentId(int studentId);
}