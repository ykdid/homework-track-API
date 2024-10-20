using Homework_track_API.Entities;
using Homework_track_API.Repositories.CourseRepository;
using Homework_track_API.Repositories.StudentCourseRepository;
using Homework_track_API.Repositories.StudentRepository;

namespace Homework_track_API.Services.StudentCourseService;

public class StudentCourseService(IStudentCourseRepository studentCourseRepository, IStudentRepository studentRepository, ICourseRepository courseRepository):IStudentCourseService
{
    private readonly IStudentCourseRepository _studentCourseRepository = studentCourseRepository;
    private readonly IStudentRepository _studentRepository = studentRepository;
    private readonly ICourseRepository _courseRepository = courseRepository;

    public async Task<StudentCourse> AddStudentToCourse(int studentId, int courseId)
    {
        if (studentId <= 0)
        {
            throw new ArgumentException("Invalid student ID.");
        }

        if (courseId <= 0)
        {
            throw new ArgumentException("Invalid course ID.");
        }

        var student = await _studentRepository.GetStudentByIdAsync(studentId);

        if (student == null)
        {
            throw new InvalidOperationException("Student not found.");
        }

        var course = await _courseRepository.GetCourseByIdAsync(courseId);

        if (course == null)
        {
            throw new InvalidOperationException("Course not found.");
        }
        
        if (await _studentCourseRepository.IsStudentEnrolledInCourseAsync(studentId, courseId))
        {
            throw new InvalidOperationException("Student is already enrolled in this course.");
        }

        var studentCourse = await _studentCourseRepository.AddStudentToCourseAsync(studentId, courseId);
        return studentCourse;
        
    }

    public async Task<bool> RemoveStudentFromCourse(int studentId, int courseId)
    {
        if (studentId <= 0)
        {
            throw new ArgumentException("Invalid student ID.");
        }

        if (courseId <= 0)
        {
            throw new ArgumentException("Invalid course ID.");
        }
        
        var student = await _studentRepository.GetStudentByIdAsync(studentId);

        if (student == null)
        {
            throw new InvalidOperationException("Student not found.");
        }

        var course = await _courseRepository.GetCourseByIdAsync(courseId);

        if (course == null)
        {
            throw new InvalidOperationException("Course not found.");
        }

        if (!await _studentCourseRepository.IsStudentEnrolledInCourseAsync(studentId, courseId))
        {
            throw new InvalidOperationException("Student has not enrolled in this course.");
        }

        var result = await _studentCourseRepository.RemoveStudentFromCourseAsync(studentId, courseId);
        return result;
    }

    public async Task<List<Student>> GetStudentsByCourseId(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid course ID.");
        }

        var course = await _courseRepository.GetCourseByIdAsync(id);

        if (course == null)
        {
            throw new InvalidOperationException("Course not found.");
        }

        var students = await _studentCourseRepository.GetStudentsByCourseIdAsync(id);

        if (students.Count == 0)
        {
            throw new ArgumentException("Empty student list.");
        }

        return students;
    }

    public async Task<List<Course>> GetCoursesByStudentId(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid student ID.");
        }

        var student = await _studentRepository.GetStudentByIdAsync(id);

        if (student == null)
        {
            throw new InvalidOperationException("Student not found.");
        }

        var courses = await _studentCourseRepository.GetCoursesByStudentIdAsync(id);

        if (courses.Count == 0)
        {
            throw new ArgumentException("Empty course list.");
        }

        return courses;
    }
}