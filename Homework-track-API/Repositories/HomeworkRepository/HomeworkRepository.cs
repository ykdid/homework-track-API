using Homework_track_API.Data;
using Homework_track_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Homework_track_API.Repositories.HomeworkRepository;

public class HomeworkRepository:IHomeworkRepository
{
    private readonly HomeworkTrackDbContext _context;

    public HomeworkRepository(HomeworkTrackDbContext context)
    {
        _context = context;
    }

    public async Task<List<Homework>> GetAllHomeworksAsync()
    {
        return await _context.Homeworks.ToListAsync();
    }

    public async Task<Homework> GetHomeworkByIdAsync(int id)
    {
        var homework = await _context.Homeworks.FindAsync(id);
        return homework;
    }

    public async Task<Homework> CreateHomeworkAsync(Homework homework)
    {
        _context.Homeworks.Add(homework);
        await _context.SaveChangesAsync();
        return homework;
    }

    public async Task<bool> DeleteHomeworkByIdAsync(int id)
    {
        var homework = await _context.Homeworks.FindAsync(id);

        if (homework !=  null)
        {
            _context.Homeworks.Remove(homework);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        return false;
    }

    public async Task<Homework> UpdateHomeworkAsync(Homework homework)
    {
        _context.Homeworks.Update(homework);
        await _context.SaveChangesAsync();
        return homework;
    }

    public async Task<List<Homework>> GetHomeworksByTeacherIdAsync(int id)
    {
       return await _context.Homeworks
            .Where(hw => hw.TeacherId == id)
            .ToListAsync();
    }
}