using Homework_track_API.Repositories.HomeworkRepository;
using Homework_track_API.Entities;
using Homework_track_API.Enums;
using Homework_track_API.Repositories.CourseRepository;
using Homework_track_API.Repositories.TeacherRepository;
using ArgumentException = System.ArgumentException;

namespace Homework_track_API.Services.HomeworkService;

public class HomeworkService(IHomeworkRepository homeworkRepository , ICourseRepository courseRepository):IHomeworkService
{
    private readonly IHomeworkRepository _homeworkRepository = homeworkRepository;
    private readonly ICourseRepository _courseRepository = courseRepository;
    

    public async Task<List<Homework>> GetAllHomeworks()
    {
        return await _homeworkRepository.GetAllHomeworksAsync();
    }

    public async Task<Homework> GetHomeworkById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid homework ID.");
        }

        var homework = await _homeworkRepository.GetHomeworkByIdAsync(id);
        
        if (homework == null)
        {
            throw new KeyNotFoundException($"Homework with ID {id} not found.");
        }
    
        return homework;
    }

    
    public async Task<Homework> CreateHomework(Homework homework)
    {
        if (homework ==  null)
        {
            throw new ArgumentNullException(nameof(homework));
        }

        if (string.IsNullOrEmpty(homework.Title) && homework.Title.Length < 3)
        {
            throw new ArgumentException("Homework title cannot be empty and should be long enough.");
        }

        if (string.IsNullOrEmpty(homework.Description) && homework.Description.Length > 10)
        {
            throw new ArgumentException("Homework description cannot be empty and should be long enough.");
        }

        if (homework.ExpireDate < DateTime.Now)
        {
            throw new InvalidOperationException("Homework due date cannot be in the past.");
        }

        homework.Status = HomeworkStatus.Active;
        
        return await _homeworkRepository.CreateHomeworkAsync(homework);
    }       

    public async Task<bool> SoftDeleteHomeworkById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid homework ID.");    
        }
        var existingHomework = await _homeworkRepository.GetHomeworkByIdAsync(id);

        if (existingHomework == null)
        {
            throw new KeyNotFoundException($"Homework with ID {id} not found.");
        }
        
        if (existingHomework.Status == HomeworkStatus.Deleted)
        {   
            throw new InvalidOperationException("Homework is already deleted.");
        }

        return await _homeworkRepository.SoftDeleteHomeworkByIdAsync(id);
    }

    public async Task<Homework> UpdateHomework(Homework homework)
    {
        if (homework == null)
        {
            throw new ArgumentNullException(nameof(homework));
        }
        
        var existingHomework = await _homeworkRepository.GetHomeworkByIdAsync(homework.Id);
        
        if (existingHomework == null)
        {
            throw new KeyNotFoundException($"Homework with ID {homework.Id} not found.");
        }

        if (homework.ExpireDate < DateTime.Now)
        {
            throw new InvalidOperationException("Cannot set a due date in the past.");
        }

        if (homework.Status !=  HomeworkStatus.Active)
        {
            throw new InvalidOperationException("Cannot update inactive homework.");
        }

        return await _homeworkRepository.UpdateHomeworkAsync(homework);
    }

    public async Task<List<Homework>> GetHomeworksByCourseId(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid course ID.");
        }

        var course = await _courseRepository.GetCourseByIdAsync(id);

        if (course == null)
        {
            throw new ArgumentException($"Course with ID {course.Id} not found.");
        }
        
        return await _homeworkRepository.GetHomeworksByCourseIdAsync(id);
    }

    public async Task<List<Homework>> GetExpiredHomeworksByCourseId(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid course ID.");
        }
        
        var course = await _courseRepository.GetCourseByIdAsync(id);

        if (course == null)
        {
            throw new ArgumentException($"Course with ID {course.Id} not found.");
        }
        
        return await _homeworkRepository.GetExpiredHomeworksByCourseIdAsync(id);
    }
}