using Homework_track_API.Repositories.StudentRepository;
using Homework_track_API.Entities;
using Homework_track_API.Services.EncryptionService;

namespace Homework_track_API.Services.StudentService;

public class StudentService(IStudentRepository studentRepository , IEncryptionService encryptionService):IStudentService
{
    private readonly IStudentRepository _studentRepository = studentRepository;
    private readonly IEncryptionService _encryptionService = encryptionService;
    
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

        student.Email = _encryptionService.Decrypt(student.Email);
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

    public async Task<Student> UpdateStudent(int id,Student student)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid student ID.");
        }
        
        if (student == null)
        {
            throw new ArgumentNullException(nameof(student), "Updated student object cannot be null.");
        }
        
        var existingStudent = await _studentRepository.GetStudentByIdAsync(id);

        if (existingStudent == null)
        {
            throw new KeyNotFoundException($"Student with ID {id} not found.");
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
            existingStudent.Email = _encryptionService.Encrypt(student.Email.Trim());
        }
        
        if (!string.IsNullOrWhiteSpace(student.ProfileImagePath))
        {
            existingStudent.ProfileImagePath = student.ProfileImagePath;
        }
        
        await _studentRepository.UpdateStudentAsync(existingStudent);

        return existingStudent;
    }

    public async Task<bool> ChangePasswordById(int id, string currentPassword, string newPassword)
    {
        var student = await _studentRepository.GetStudentByIdAsync(id);

        if (student ==  null || !_encryptionService.VerifyHash(currentPassword,student.Password))
        {
            return false;
        }

        student.Password = _encryptionService.Hash(newPassword);
        await _studentRepository.UpdateStudentAsync(student);
        return true;
    }
    
    
}