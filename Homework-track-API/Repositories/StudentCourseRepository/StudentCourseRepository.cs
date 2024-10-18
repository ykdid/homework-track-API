using Homework_track_API.Data;
using Homework_track_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Homework_track_API.Repositories.StudentCourseRepository;

public class StudentCourseRepository:IStudentCourseRepository
{
    private readonly HomeworkTrackDbContext _context;

    public StudentCourseRepository(HomeworkTrackDbContext context)
    {
        _context = context; 
    }
    
    public async Task<StudentCourse> AddStudentToCourseAsync(int studentId, int courseId)
    {
        if (await IsStudentEnrolledInCourseAsync(studentId, courseId))
        {
            throw new Exception("Student is already enrolled in this course.");
        }

        var studentCourse = new StudentCourse
        {
            StudentId = studentId,
            CourseId = courseId
        };

        await _context.StudentCourses.AddAsync(studentCourse);
        await _context.SaveChangesAsync();

        return studentCourse;
    }

    public async Task<bool> RemoveStudentFromCourseAsync(int studentId, int courseId)
    {
        var studentCourse = await _context.StudentCourses
            .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

        if (studentCourse == null)
        {
            throw new Exception("Student is not enrolled in this course.");
        }

        _context.StudentCourses.Remove(studentCourse);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<Student>> GetStudentsByCourseIdAsync(int courseId)
    {
        var students = await _context.StudentCourses
            .Where(sc => sc.CourseId == courseId)
            .Select(sc => sc.Student)
            .ToListAsync();

        return students;
    }

    public async Task<List<Course>> GetCoursesByStudentIdAsync(int studentId)
    {
        var courses = await _context.StudentCourses
            .Where(sc => sc.StudentId == studentId)
            .Select(sc => sc.Course)
            .ToListAsync();

        return courses;
    }

    public async Task<bool> IsStudentEnrolledInCourseAsync(int studentId, int courseId)
    {
        return await _context.StudentCourses
            .AnyAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);
    }
}