using Homework_track_API.Data;
using Homework_track_API.Entities;
using Homework_track_API.Enums;
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
        return await _context.Homeworks
            .Where(hw =>hw.Status == HomeworkStatus.Active)
            .ToListAsync();
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

    public async Task<bool> SoftDeleteHomeworkByIdAsync(int id)
    {
        var homework = await _context.Homeworks.FindAsync(id);

        if (homework !=  null)
        {
            homework.Status = HomeworkStatus.Deleted;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        return false;
    }

    public async Task<List<Homework>> GetExpiredHomeworksByCourseIdAsync(int id)
    {
        return await _context.Homeworks
            .Where(hw => hw.CourseId == id && hw.Status == HomeworkStatus.Expired)
            .ToListAsync();
    }

    public async Task<Homework> UpdateHomeworkAsync(Homework homework)
    {
        _context.Homeworks.Update(homework);
        await _context.SaveChangesAsync();
        return homework;
    }

   public async Task<List<Homework>> GetHomeworksByCourseIdAsync(int id)
    {
       return await _context.Homeworks
            .Where(hw => hw.CourseId == id && hw.Status == HomeworkStatus.Active)
            .ToListAsync();
    }

    public async Task<IEnumerable<Homework>> GetExpiredHomeworks()
    {
        return await _context.Homeworks
            .Where(hw => hw.ExpireDate < DateTime.UtcNow && hw.Status != HomeworkStatus.Expired)
            .ToListAsync();
    }
    
}