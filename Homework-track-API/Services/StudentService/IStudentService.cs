using Homework_track_API.Entities;

namespace Homework_track_API.Services.StudentService;

public interface IStudentService
{
    Task<IEnumerable<Student>> GetAllStudents();
    Task<Student> GetStudentById(int id);
    Task<bool> DeleteStudentById(int id);
    Task<Student> CreateStudent(Student student);
    Task<Student> UpdateStudent(Student student);
    Task<bool> ChangePasswordById(int id, string currentPassword, string newPassword);
}