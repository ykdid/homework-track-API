using Homework_track_API.Entities;

namespace Homework_track_API.Services.HomeworkService;

public interface IHomeworkService
{
    Task<List<Homework>> GetAllHomeworks();
    Task<Homework> GetHomeworkById(int id);
    Task<Homework> CreateHomeworkByCourseId(int courseId, Homework homework);
    Task<bool> SoftDeleteHomeworkById(int id);
    Task<Homework> UpdateHomework(int id, Homework homework);
    Task<List<Homework>> GetHomeworksByCourseId(int id);
    Task<List<Homework>> GetExpiredHomeworksByCourseId(int id);
}