using Homework_track_API.Entities;

namespace Homework_track_API.Services.TeacherService;

public interface ITeacherService
{
    Task<IEnumerable<Teacher>> GetAllTeachers();
    Task<Teacher> CreateTeacher(Teacher teacher);
    Task<bool> DeleteTeacherById(int id);
    Task<Teacher> GetTeacherById(int id);
    Task<Teacher> UpdateTeacher(int id,Teacher teacher);
}