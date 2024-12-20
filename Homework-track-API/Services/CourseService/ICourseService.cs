using Homework_track_API.DTOs;
using Homework_track_API.Entities;
using Homework_track_API.Enums;

namespace Homework_track_API.Services.CourseService;

public interface ICourseService
{
    Task<List<Course>> GetAllCourses();
    Task<Course> GetCourseById(int id);
    Task<List<Course>> GetActiveCoursesByTeacherId(int id);
    Task<List<Course>> GetArchivedCoursesByTeacherId(int id);
    Task<List<Course>> GetDeletedCoursesByTeacherId(int id);
    Task<Course> CreateCourseByTeacherId(int id,CreateCourse courseDto);
    Task<Course?> GetCourseByCode(string code);
    Task<Course> UpdateCourseById(int id, Course course);
    Task<IEnumerable<Course?>> FindCoursesByTeacherId(int teacherId, string courseName);
    Task<IEnumerable<Course?>> FindCoursesByStudentId(int studentId, string courseName);
    Task<bool> ChangeCourseStatus(int courseId, CourseStatus newStatus);
}