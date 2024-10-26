using Homework_track_API.Entities;

namespace Homework_track_API.Repositories.StudentRepository;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    Task<Student> GetStudentByIdAsync(int id);
    Task<bool> DeleteStudentByIdAsync(int id);
    Task<Student> CreateStudentAsync(Student student);
    Task<Student> UpdateStudentAsync(Student student);
    Task<Student?> GetStudentByEmailAsync(string email); 
}