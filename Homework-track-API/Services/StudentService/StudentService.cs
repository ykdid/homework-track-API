using Homework_track_API.Repositories.StudentRepository;
using Homework_track_API.Entities;

namespace Homework_track_API.Services.StudentService;

public class StudentService(IStudentRepository studentRepository):IStudentService
{
    private readonly IStudentRepository _studentRepository = studentRepository;
    
    public async Task<IEnumerable<Student>> GetAllStudents()
    {
        return await _studentRepository.GetAllStudentsAsync();
    }

    public async Task<Student> GetStudentById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid student ID.");
        }

        var student =  await _studentRepository.GetStudentByIdAsync(id);

        if (student == null)
        {
            throw new KeyNotFoundException($"Student with ID {id} not found.");
        }

        return student;
    }

    public async Task<bool> DeleteStudentById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid student ID.");
        }

        var student = await _studentRepository.GetStudentByIdAsync(id);

        if (student == null)
        {
            throw new KeyNotFoundException($"Student with ID {id} not found.");
        }

        await _studentRepository.DeleteStudentByIdAsync(id);
        
        return true;
    }

    public async Task<Student> CreateStudent(Student student)
    {
        if (student ==  null)
        {
            throw new ArgumentNullException(nameof(student));
        }
        
        student.Name = student.Name.Trim();
        student.Surname = student.Surname.Trim();
        
        if (string.IsNullOrEmpty(student.Name) && string.IsNullOrEmpty(student.Surname))
        {
            throw new ArgumentException("Student name and surname cannot be empty.");
        }
        
        if (student.Name.Contains(" ") || student.Surname.Contains(" "))
        {
            throw new ArgumentException("Name and surname should not contain spaces.");
        }
        
        await _studentRepository.CreateStudentAsync(student);
        return student;
    }

    public async Task<Student> UpdateStudent(Student student)
    {
        if (student == null)
        {
            throw new ArgumentNullException(nameof(student), "Updated student object cannot be null.");
        }
        
        var existingStudent = await _studentRepository.GetStudentByIdAsync(student.Id);

        if (existingStudent == null)
        {
            throw new KeyNotFoundException($"Student with ID {student.Id} not found.");
        }
        
        if (!string.IsNullOrWhiteSpace(student.Name))
        {
            existingStudent.Name = student.Name.Trim();
        }
        
        if (!string.IsNullOrWhiteSpace(student.Surname))
        {
            existingStudent.Surname = student.Surname.Trim();
        }
        
        if (!string.IsNullOrWhiteSpace(student.Email))
        {
            existingStudent.Email = student.Email.Trim();
        }
        
        if (!string.IsNullOrWhiteSpace(student.Password))
        {
            existingStudent.Password = student.Password; 
        }
        
        if (!string.IsNullOrWhiteSpace(student.ProfileImagePath))
        {
            existingStudent.ProfileImagePath = student.ProfileImagePath;
        }
        
        await _studentRepository.UpdateStudentAsync(existingStudent);

        return existingStudent;
    }
    
    
}