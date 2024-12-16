using Homework_track_API.Entities;

namespace Homework_track_API.Repositories.CourseRepository;

public interface ICourseRepository
{
    Task<List<Course>> GetAllCoursesAsync();
    Task<Course> GetCourseByIdAsync(int id);   
    Task<List<Course>> GetActiveCoursesByTeacherId(int id);
    Task<List<Course>> GetArchivedCoursesByTeacherId(int id);
    Task<List<Course>> GetDeletedCoursesByTeacherId(int id);
    Task<Course> CreateCourseAsync(Course course);
    Task<Course?> GetCourseByCodeAsync(string code);
    Task<Course> UpdateCourseAsync(Course course);
    Task<bool> SoftDeleteCourseByIdAsync(int id);
    Task<bool> ArchiveCourseByIdAsync(int id);
    Task<IEnumerable<Course?>> FindCoursesByTeacherIdAsync(int teacherId, string courseName);
    Task<IEnumerable<Course?>> FindCoursesByStudentIdAsync(int studentId, string courseName);

}