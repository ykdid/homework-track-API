using Homework_track_API.Entities;
using Homework_track_API.Repositories.TeacherRepository;

namespace Homework_track_API.Services.TeacherService;

public class TeacherService:ITeacherService
{
    private readonly TeacherRepository _teacherRepository;

    public TeacherService(TeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task<IEnumerable<Teacher>> GetAllTeachers()
    {
        return await _teacherRepository.GetAllTeachersAsync();
    }

    public async Task<Teacher> GetTeacherById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid teacher ID.");
        }

        var teacher = await _teacherRepository.GetTeacherByIdAsync(id);

        if (teacher == null)
        {
            throw new KeyNotFoundException($"Teacher with ID {id} not found.");
        }

        return teacher;
    }

    public async Task<bool> DeleteTeacherById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid teacher ID.");
        }

        var teacher = await _teacherRepository.GetTeacherByIdAsync(id);

        if (teacher == null)
        {
            throw new KeyNotFoundException($"Student with ID {id} not found.");
           
        }

        await _teacherRepository.DeleteTeacherAsync(id);
        return true;
    }

    public async Task<Teacher> CreateTeacher(Teacher teacher)
    {
        if (teacher == null)
        {
            throw new ArgumentNullException(nameof(teacher));
        }

        teacher.Name = teacher.Name.Trim();
        teacher.Surname = teacher.Surname.Trim();
        
        if (string.IsNullOrEmpty(teacher.Name) && string.IsNullOrEmpty(teacher.Surname))
        {
            throw new ArgumentException("Teacher name and surname cannot be empty.");
        }
        
        if (teacher.Name.Contains(" ") || teacher.Surname.Contains(" "))
        {
            throw new ArgumentException("Name and surname should not contain spaces.");
        }

        await _teacherRepository.CreateTeacherAsync(teacher);
        return teacher;
    }

    public async Task<Teacher> UpdateTeacher(Teacher teacher)
    {
        if (teacher == null)
        {
            throw new ArgumentNullException(nameof(teacher), "Updated student object cannot be null.");
        }

        var existingTeacher = await _teacherRepository.GetTeacherByIdAsync(teacher.Id);

        if (existingTeacher == null)
        {
            throw new KeyNotFoundException($"Teacher with ID {teacher.Id} not found.");
        }
        
        if (!string.IsNullOrWhiteSpace(teacher.Name))
        {
            existingTeacher.Name = teacher.Name.Trim();
        }
        
        if (!string.IsNullOrWhiteSpace(teacher.Surname))
        {
            existingTeacher.Surname = teacher.Surname.Trim();
        }
        
        if (!string.IsNullOrWhiteSpace(teacher.Email))
        {
            existingTeacher.Email = teacher.Email.Trim();
        }
        
        if (!string.IsNullOrWhiteSpace(teacher.Password))
        {
            existingTeacher.Password = teacher.Password; 
        }
        
        if (!string.IsNullOrWhiteSpace(teacher.ProfileImagePath))
        {
            existingTeacher.ProfileImagePath = teacher.ProfileImagePath;
        }

        await _teacherRepository.UpdateTeacherAsync(teacher);
        return teacher;
    }
    
}