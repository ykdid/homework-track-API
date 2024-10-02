using Homework_track_API.Data;
using Homework_track_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Homework_track_API.Repositories.SubmissionRepository;

public class SubmissionRepository:ISubmissionRepository
{
    private readonly HomeworkTrackDbContext _context;

    public SubmissionRepository(HomeworkTrackDbContext context)
    {
        _context = context;
    }

    public async Task<List<Submission>> GetAllSubmissionsAsync()
    {
        return await _context.Submissions.ToListAsync();
    }

    public async Task<Submission> GetSubmissionByIdAsync(int id)
    {
        var submission = await _context.Submissions.FindAsync(id);
        return submission;
    }

    public async Task<Submission> CreateSubmissionAsync(Submission submission)
    {
        _context.Submissions.Add(submission);
        await _context.SaveChangesAsync();
        return submission;
    }

    public async Task<bool> DeleteSubmissionByIdAsync(int id)
    {
        var submission = await _context.Submissions.FindAsync(id);
        
        if (submission != null)
        {
            _context.Submissions.Remove(submission);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        return false;
    }

    public async Task<Submission> UpdateSubmissionAsync(Submission submission)
    {
        _context.Submissions.Update(submission);
        await _context.SaveChangesAsync();
        return submission;
    }

    public async Task<List<Submission>> GetSubmissionsByStudentIdAsync(int id)
    {
        return await _context.Submissions
            .Include(sb => sb.StudentId == id)
            .ToListAsync();
    }

    public async Task<List<Submission>> GetSubmissionsByHomeworkIdAsync(int id)
    {
        return await _context.Submissions
            .Where(sb => sb.HomeworkId == id)
            .ToListAsync();
    }
}