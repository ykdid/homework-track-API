using Homework_track_API.Data;
using Homework_track_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Homework_track_API.Repositories.StudentRepository;

public class StudentRepository:IStudentRepository
{
    private readonly HomeworkTrackDbContext _context;
    
    public StudentRepository(HomeworkTrackDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        return await _context.Students.ToListAsync();
    }

    public async Task<Student> GetStudentByIdAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);
        return student;
    }

    public async Task<Student> CreateStudentAsync(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task<bool> DeleteStudentByIdAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);
        
        if (student != null)
        {
            _context.Students.Remove(student);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        
        return false; 
    }

    public async Task<Student> UpdateStudentAsync(Student student)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task<Student?> GetStudentByEmailAsync(string email)
    {
        return await _context.Students.FirstOrDefaultAsync(s => s.Email == email);
    }


}