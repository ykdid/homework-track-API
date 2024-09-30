using Homework_track_API.Entities;

namespace Homework_track_API.Repositories.SubmissionRepository;

public interface ISubmissionRepository
{
    Task<List<Submission>> GetAllSubmissionsAsync();
    Task<Submission> GetSubmissionByIdAsync(int id);
    Task<Submission> CreateSubmissionAsync(Submission submission);
    Task<bool> DeleteSubmissionByIdAsync(int id);
    Task<Submission> UpdateSubmissionAsync(Submission submission);
}