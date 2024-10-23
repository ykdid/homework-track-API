using Homework_track_API.Entities;

namespace Homework_track_API.Services.CourseService;

public interface ICourseService
{
    Task<List<Course>> GetAllCourses();
    Task<Course> GetCourseById(int id);
    Task<List<Course>> GetCoursesByTeacherId(int id);
    Task<Course> CreateCourseByTeacherId(int id,Course course);
    Task<Course?> GetCourseByCode(string code);
    Task<Course> UpdateCourseById(int id, Course course);
    Task<bool> SoftDeleteCourseById(int id);
    Task<bool> ArchiveCourseById(int id);
}