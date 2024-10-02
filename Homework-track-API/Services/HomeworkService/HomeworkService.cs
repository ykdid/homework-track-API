using Homework_track_API.Repositories.HomeworkRepository;
using Homework_track_API.Entities;
namespace Homework_track_API.Services.HomeworkService;

public class HomeworkService:IHomeworkService
{
    private readonly IHomeworkRepository _homeworkRepository;

    public HomeworkService(IHomeworkRepository homeworkRepository)
    {
        _homeworkRepository = homeworkRepository;
    }

    public async Task<List<Homework>> GetAllHomeworks()
    {
        return await _homeworkRepository.GetAllHomeworksAsync();
    }

    public async Task<Homework> GetHomeworkById(int id)
    {
        return await _homeworkRepository.GetHomeworkByIdAsync(id);
    }

    public async Task<Homework> GetHomeworkByTeacherId(int id)
    {
        return await _homeworkRepository.GetHomeworkByIdAsync(id);
    }

    public async Task<Homework> CreateHomework(Homework homework)
    {
        if (homework ==  null)
        {
            throw new ArgumentNullException(nameof(homework));
        }
        return await _homeworkRepository.CreateHomeworkAsync(homework);
    }

    public async Task<bool> SoftDeleteHomeworkById(int id)
    {
        var existingHomework = await _homeworkRepository.GetHomeworkByIdAsync(id);

        if (existingHomework == null)
        {
            return false;
        }

        return await _homeworkRepository.SoftDeleteHomeworkByIdAsync(id);
    }

    public async Task<Homework> UpdateHomework(Homework homework)
    {
        if (homework == null)
        {
            throw new ArgumentNullException(nameof(homework));
        }

        return await _homeworkRepository.UpdateHomeworkAsync(homework);
    }

    public async Task<List<Homework>> GetHomeworksByTeacherId(int id)
    {
        return await _homeworkRepository.GetHomeworksByTeacherIdAsync(id);
    }

    public async Task<List<Homework>> GetExpiredHomeworksByTeacherId(int id)
    {
        return await _homeworkRepository.GetExpiredHomeworksByTeacherIdAsync(id);
    }
}