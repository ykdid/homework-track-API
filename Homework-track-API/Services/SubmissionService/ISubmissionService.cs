using Homework_track_API.Entities;

namespace Homework_track_API.Services.SubmissionService;

public interface ISubmissionService
{
    Task<List<Submission>> GetAllSubmissions();
    Task<Submission> GetSubmissionById(int id);
    Task<Submission> CreateSubmissionByStudentId(int studentId,Submission submission);
    Task<bool> DeleteSubmissionById(int id);
    Task<Submission> UpdateSubmission(int id,Submission submission);
    Task<List<Submission>> GetSubmissionsByStudentId(int id);
    Task<List<Submission>> GetSubmissionsByHomeworkId(int id);
    Task<Submission> UpdateMarkBySubmissionId(int submissionId ,int mark);
}