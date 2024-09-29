using Homework_track_API.Data;
using Homework_track_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Homework_track_API.Repositories.TeacherRepository;

public class TeacherRepository:ITeacherRepository
{
    private readonly HomeworkTrackDbContext _context;

    public TeacherRepository(HomeworkTrackDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
    {
        return await _context.Teachers.ToListAsync();
    }

    public async Task<Teacher> CreateTeacherAsync(Teacher teacher)
    {
        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();
        return teacher;
    }

    public async Task<bool> DeleteTeacherAsync(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        
        if (teacher != null)
        {
            _context.Teachers.Remove(teacher); 
            var result = await _context.SaveChangesAsync();
            return result > 0;  
        }

        return false;
    }

    public async Task<Teacher> GetTeacherByIdAsync(int id)
    {
        return await _context.Teachers.FindAsync(id); 
    }

    public async Task<Teacher> UpdateTeacherByIdAsync(Teacher teacher)
    {
        _context.Teachers.Update(teacher);
        await _context.SaveChangesAsync();
        return teacher;
    }
}