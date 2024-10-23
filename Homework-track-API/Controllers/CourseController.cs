using Homework_track_API.Services.CourseService;
using Microsoft.AspNetCore.Mvc;

namespace Homework_track_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CourseController(ICourseService courseService): ControllerBase
    {
        private readonly ICourseService _courseService = courseService;
        
        
    }
}



