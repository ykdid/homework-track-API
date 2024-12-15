using Homework_track_API.DTOs;
using Homework_track_API.Entities;

namespace Homework_track_API.Services.CourseService;

public interface ICourseService
{
    Task<List<Course>> GetAllCourses();
    Task<Course> GetCourseById(int id);
    Task<List<Course>> GetCoursesByTeacherId(int id);
    Task<Course> CreateCourseByTeacherId(int id,CreateCourse courseDto);
    Task<Course?> GetCourseByCode(string code);
    Task<Course> UpdateCourseById(int id, Course course);
    Task<bool> SoftDeleteCourseById(int id);
    Task<bool> ArchiveCourseById(int id);
    Task<IEnumerable<Course?>> FindCoursesByTeacherId(int teacherId, string courseName);
    Task<IEnumerable<Course?>> FindCoursesByStudentId(int studentId, string courseName);
}