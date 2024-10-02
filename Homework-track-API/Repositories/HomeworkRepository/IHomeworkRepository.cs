using Homework_track_API.Entities;

namespace Homework_track_API.Repositories.HomeworkRepository;

public interface IHomeworkRepository
{
    Task<List<Homework>> GetAllHomeworksAsync();
    Task<Homework> GetHomeworkByIdAsync(int id);
    Task<Homework> CreateHomeworkAsync(Homework  homework);
    Task<bool> DeleteHomeworkByIdAsync(int id);
    Task<Homework> UpdateHomeworkAsync(Homework homework);
    Task<List<Homework>> GetHomeworksByTeacherIdAsync(int id);
}