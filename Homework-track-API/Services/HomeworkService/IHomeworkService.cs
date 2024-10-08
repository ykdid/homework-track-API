using Homework_track_API.Entities;

namespace Homework_track_API.Services.HomeworkService;

public interface IHomeworkService
{
    Task<List<Homework>> GetAllHomeworks();
    Task<Homework> GetHomeworkById(int id);
    Task<Homework> CreateHomework(Homework homework);
    Task<bool> SoftDeleteHomeworkById(int id);
    Task<Homework> UpdateHomework(Homework homework);
    Task<List<Homework>> GetHomeworksByTeacherId(int id);
    Task<List<Homework>> GetExpiredHomeworksByTeacherId(int id);
}