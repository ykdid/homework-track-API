using Homework_track_API.Enums;
using Homework_track_API.Repositories.HomeworkRepository;

namespace Homework_track_API.Services.HomeworkService;

public class HomeworkBackgroundService(IServiceScopeFactory scopeFactory):IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private Timer _timer;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));
        return Task.CompletedTask;
    }
    
    private void DoWork(object state)
    {
        _ = DoWorkAsync();
    }

    private async Task DoWorkAsync()
    {
        using (var scope  = _scopeFactory.CreateScope())
        {
            var homeworkRepository = scope.ServiceProvider.GetRequiredService<IHomeworkRepository>();
            var expiredHomeworks = await homeworkRepository.GetExpiredHomeworks();

            foreach (var homework in expiredHomeworks)
            {
                homework.Status = HomeworkStatus.Expired;
                await homeworkRepository.UpdateHomeworkAsync(homework);
            }
        }
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }
    
    public void Dispose()
    {
        _timer?.Dispose();
    }
}