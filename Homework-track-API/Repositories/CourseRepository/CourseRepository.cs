using Homework_track_API.Data;
using Homework_track_API.Entities;
using Homework_track_API.Enums;
using Microsoft.EntityFrameworkCore;

namespace Homework_track_API.Repositories.CourseRepository;

public class CourseRepository:ICourseRepository
{
    private readonly HomeworkTrackDbContext _context;

    public CourseRepository(HomeworkTrackDbContext context)
    {
        _context = context;
    }

    public async Task<List<Course>> GetAllCoursesAsync()
    {
        return await _context.Courses
            .Where(c => c.Status == CourseStatus.Active)
            .Include(c => c.Homeworks)
            .Include(c=> c.StudentCourses)
            .ToListAsync();
    }
    
    public async Task<Course> GetCourseByIdAsync(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        return course;
    }

    public async Task<List<Course>> GetCoursesByTeacherId(int id)
    {
        return await _context.Courses
            .Where(c => c.TeacherId == id)
            .ToListAsync();
    }
    
    public async Task<Course> CreateCourseAsync(Course course)
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return course;
    }
    
    public async Task<Course> UpdateCourseAsync(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
        return course;
    }
    
    public async Task<bool> SoftDeleteCourseByIdAsync(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course !=  null)
        {
            course.Status = CourseStatus.Deleted;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        return false;
    }

    public async Task<bool> ArchiveCourseByIdAsync(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course != null)
        {
            course.Status = CourseStatus.Archived;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        return false;
    }

    public async Task<Course?> GetCourseByCodeAsync(string code)
    {
        return await _context.Courses
            .FirstOrDefaultAsync(c => c.Code == code && c.Status == CourseStatus.Active);
    }

    public async Task<IEnumerable<Course?>> FindCoursesByTeacherIdAsync(int teacherId ,string courseName)
    {
        return await _context.Courses
            .Where(c => c.Name.ToLower().Contains(courseName.Trim().ToLower())
                        && c.Status == CourseStatus.Active 
                        && c.TeacherId == teacherId)
            .OrderBy(c => c.Id)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Course?>> FindCoursesByStudentIdAsync(int studentId, string courseName)
    {
        return await _context.Courses
            .Where(c => c.Status == CourseStatus.Active
                        && c.Name.ToLower().Contains(courseName.Trim().ToLower())
                        && c.StudentCourses.Any(sc => sc.StudentId == studentId))
            .OrderBy(c => c.Id)
            .ToListAsync();
    }

}