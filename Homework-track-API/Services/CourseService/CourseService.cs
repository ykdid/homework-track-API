using Homework_track_API.DTOs;
using Homework_track_API.Entities;
using Homework_track_API.Enums;
using Homework_track_API.Repositories.CourseRepository;
using Homework_track_API.Repositories.StudentRepository;
using Homework_track_API.Repositories.TeacherRepository;
using ArgumentException = System.ArgumentException;

namespace Homework_track_API.Services.CourseService;

public class CourseService(ICourseRepository courseRepository , ITeacherRepository teacherRepository , IStudentRepository studentRepository):ICourseService
{
    private readonly ICourseRepository _courseRepository = courseRepository;
    private readonly ITeacherRepository _teacherRepository = teacherRepository;
    private readonly IStudentRepository _studentRepository = studentRepository;

    public async Task<List<Course>> GetAllCourses()
    {
        return await _courseRepository.GetAllCoursesAsync();
    }

    public async Task<Course> GetCourseById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid course ID.");
        }

        var course = await _courseRepository.GetCourseByIdAsync(id);

        if (course == null)
        {
            throw new KeyNotFoundException($"Course with ID {id} not found.");
        }

        return course;
    }

    public async Task<List<Course>> GetCoursesByTeacherId(int id)
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

        var courses = await _courseRepository.GetCoursesByTeacherId(id);

        if (courses.Count == 0)
        {
            throw new Exception("Empty course list.");
        }

        return courses;
    }

    public async Task<Course> CreateCourseByTeacherId(int id, CreateCourse courseDto)
    {
        if (id != courseDto.TeacherId)
        {
            throw new Exception("Id and TeacherId did not match.");
        }
        
        if (string.IsNullOrEmpty(courseDto.Name) || courseDto.Name.Length < 3)
        {
            throw new ArgumentException("Course should have a valid name.");
        }

        var course = new Course
        {
            TeacherId = courseDto.TeacherId,
            Name = courseDto.Name,
            Description = courseDto.Description,
            ImagePath = courseDto.ImagePath,
            Code = await GenerateUniqueCourseCodeAsync(),
            Status = CourseStatus.Active
        };

        return await _courseRepository.CreateCourseAsync(course);
        
    }
    
    private async Task<string> GenerateUniqueCourseCodeAsync()
    {
        string newCode;
        bool isUnique = false;

        do
        {
            string randomPart = GenerateRandomString(10);

            newCode = randomPart;
            var existingCourse = await _courseRepository.GetCourseByCodeAsync(newCode);
            if (existingCourse == null)
            {
                isUnique = true;
            }
        }
        while (!isUnique);

        return newCode;
    }

    private string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public async Task<Course?> GetCourseByCode(string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            throw new ArgumentException("Course should be has a name.");
        }

        var course  = await _courseRepository.GetCourseByCodeAsync(code);

        if (course == null)
        {
            throw new ArgumentNullException(nameof(course));
        }

        return course;
    }

    public async Task<Course> UpdateCourseById(int id, Course course)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid course ID.");
        }

        var existedCourse = await _courseRepository.GetCourseByIdAsync(id);

        if (existedCourse == null)
        {
            throw new ArgumentNullException(nameof(course));
        }

        if (!string.IsNullOrEmpty(course.Name))
        {
            existedCourse.Name = course.Name.Trim();
        }
        
        existedCourse.ImagePath = !string.IsNullOrEmpty(course.ImagePath) ? course.ImagePath : existedCourse.ImagePath;
        existedCourse.Description = !string.IsNullOrEmpty(course.Description) ? course.Description.Trim() : existedCourse.Description;
        
        return await _courseRepository.UpdateCourseAsync(existedCourse);
    }

    public async Task<bool> SoftDeleteCourseById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid course ID.");
        }

        var existingCourse = await _courseRepository.GetCourseByIdAsync(id);

        if (existingCourse == null)
        {
            throw new ArgumentNullException(nameof(existingCourse));
        }

        if (existingCourse.Status == CourseStatus.Deleted)
        {
            throw new ArgumentException("Course is already deleted.");
        }

        return await _courseRepository.SoftDeleteCourseByIdAsync(id);
    }

    public async Task<bool> ArchiveCourseById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid course ID.");
        }
        
        var existingCourse = await _courseRepository.GetCourseByIdAsync(id);

        if (existingCourse == null)
        {
            throw new ArgumentNullException(nameof(existingCourse));
        }

        if (existingCourse.Status == CourseStatus.Archived)
        {
            throw new InvalidOperationException("Course is already archived.");
        }

        return await _courseRepository.ArchiveCourseByIdAsync(id);
    }

    public async Task<IEnumerable<Course?>> FindCoursesByTeacherId(int teacherId, string courseName)
    {
        if (teacherId <= 0)
        {
            throw new ArgumentException("Invalid teacher ID.");
        }

        var teacher = await _teacherRepository.GetTeacherByIdAsync(teacherId);

        if (teacher == null)
        {
            throw new ArgumentNullException(nameof(teacher));
        }

        if (courseName.Length == 0)
        {
            throw new ArgumentException("Need a input characters.");
        }

        return await _courseRepository.FindCoursesByTeacherIdAsync(teacherId, courseName);
    }
    
    public async Task<IEnumerable<Course?>> FindCoursesByStudentId(int studentId, string courseName)
    {
        if (studentId <= 0)
        {
            throw new ArgumentException("Invalid teacher ID.");
        }

        var student = await _studentRepository.GetStudentByIdAsync(studentId);

        if (student == null)
        {
            throw new ArgumentNullException(nameof(student));
        }

        if (courseName.Length == 0)
        {
            throw new ArgumentException("Need a input characters.");
        }

        return await _courseRepository.FindCoursesByStudentIdAsync(studentId, courseName);
    }
}