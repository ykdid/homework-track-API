using Homework_track_API.Entities;

namespace Homework_track_API.Repositories.StudentCourseRepository;

public interface IStudentCourseRepository
{
    Task<StudentCourse> AddStudentToCourseAsync(int studentId, int courseId);
    Task<bool> RemoveStudentFromCourseAsync(int studentId, int courseId);
    Task<List<Student>> GetStudentsByCourseIdAsync(int courseId);
    Task<List<Course>> GetCoursesByStudentIdAsync(int studentId);
    Task<bool> IsStudentEnrolledInCourseAsync(int studentId, int courseId);
}