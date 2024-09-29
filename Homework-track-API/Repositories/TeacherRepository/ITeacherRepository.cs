using Homework_track_API.Data;
using Homework_track_API.Entities;

namespace Homework_track_API.Repositories.TeacherRepository;

public interface ITeacherRepository
{
    Task<IEnumerable<Teacher>> GetAllTeachersAsync();
    Task<Teacher> CreateTeacherAsync(Teacher teacher);
    Task<bool> DeleteTeacherAsync(int id);
    Task<Teacher> GetTeacherByIdAsync(int id);
    Task<Teacher> UpdateTeacherByIdAsync(Teacher teacher);
}   