using Homework_track_API.Entities;
using Homework_track_API.Repositories.HomeworkRepository;
using Homework_track_API.Repositories.StudentRepository;
using Homework_track_API.Repositories.SubmissionRepository;


namespace Homework_track_API.Services.SubmissionService;

public class SubmissionService(ISubmissionRepository submissionRepository , IStudentRepository studentRepository , IHomeworkRepository homeworkRepository) : ISubmissionService
{
    private readonly ISubmissionRepository _submissionRepository = submissionRepository;
    private readonly IStudentRepository _studentRepository = studentRepository;
    private readonly IHomeworkRepository _homeworkRepository = homeworkRepository;

    public async Task<List<Submission>> GetAllSubmissions()
    {
        return await _submissionRepository.GetAllSubmissionsAsync();
    }

    public async Task<Submission> GetSubmissionById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid submission ID.");
        }

        var submission = await _submissionRepository.GetSubmissionByIdAsync(id);

        if (submission == null)
        {
            throw new KeyNotFoundException($"Homework with ID {id} not found.");
        }

        return submission;
    }

    public async Task<Submission> CreateSubmission(Submission submission)
    {
        if (submission == null)
        {
            throw new ArgumentNullException(nameof(submission));
        }

        if (string.IsNullOrEmpty(submission.SubmissionFilePath))
        {
            throw new ArgumentException("Submission cannot be empty.");
        }

        return await _submissionRepository.CreateSubmissionAsync(submission);
    }

    public async Task<bool> DeleteSubmissionById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid submission ID.");
        }

        var submission = await _submissionRepository.GetSubmissionByIdAsync(id);

        if (submission ==  null)
        {
            throw new ArgumentNullException(nameof(submission));
        }

        await _submissionRepository.DeleteSubmissionByIdAsync(id);
        return true;
    }

    public async Task<Submission> UpdateSubmission(Submission submission)
    {
        if (submission == null)
        {
            throw new ArgumentNullException(nameof(submission));
        }

        var existingSubmission = await _submissionRepository.GetSubmissionByIdAsync(submission.Id);

        if (submission == null)
        {
            throw new KeyNotFoundException($"Homework with ID {submission.Id} not found.");
        }

        if (!string.IsNullOrWhiteSpace(submission.SubmissionFilePath))
        {
            existingSubmission.SubmissionFilePath = submission.SubmissionFilePath;
        }

        await _submissionRepository.UpdateSubmissionAsync(submission);
        return submission;
    }

    public async Task<List<Submission>> GetSubmissionsByStudentId(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid student ID.");
        }

        var student = await _studentRepository.GetStudentByIdAsync(id);

        if (student == null)
        {
            throw new KeyNotFoundException($"Student with ID {student.Id} not found.");
        }
        
        return await _submissionRepository.GetSubmissionsByStudentIdAsync(id);
    }

    public async Task<List<Submission>> GetSubmissionsByHomeworkId(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid homework ID.");
        }

        var homework = await _homeworkRepository.GetHomeworkByIdAsync(id);

        if (homework == null)
        {
            throw new KeyNotFoundException($"Homework with ID {homework.Id} not found.");
        }

        return await _submissionRepository.GetSubmissionsByHomeworkIdAsync(id);
    }
}