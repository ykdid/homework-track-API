using Homework_track_API.Entities;
using Homework_track_API.Services.HomeworkService;
using Microsoft.AspNetCore.Mvc;

namespace Homework_track_API.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class HomeworkController(IHomeworkService homeworkService):ControllerBase
    {
        private readonly IHomeworkService _homeworkService = homeworkService;
        
        
    }
}