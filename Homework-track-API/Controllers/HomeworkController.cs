using Homework_track_API.Entities;
using Homework_track_API.Services.HomeworkService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Homework_track_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeworkController(IHomeworkService homeworkService):ControllerBase
    {
        private readonly IHomeworkService _homeworkService = homeworkService;

        [HttpGet("getAllHomeworks")]
        public async Task<IActionResult> GetAllHomeworks()
        {
            try
            {
                var homeworks = await _homeworkService.GetAllHomeworks();

                if (!homeworks.Any() || homeworks.IsNullOrEmpty())
                {
                    return NoContent();
                }

                return Ok(homeworks);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpGet("GetHomeworkBy/{id}")]
        public async Task<IActionResult> GetHomeworkById(int id)
        {
            try
            {
                var homework = await _homeworkService.GetHomeworkById(id);

                if (homework == null)
                {
                    return NoContent();
                }

                return Ok(homework);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message); 
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message); 
            }
        }

        [HttpPost("CreateHomeworkByCourse/{id}")]
        public async Task<IActionResult> CreateHomeworkByCourseId(int courseId, Homework homework)
        {
            try
            {
                var createdHomework = await _homeworkService.CreateHomeworkByCourseId(courseId, homework);
                return CreatedAtAction(nameof(GetHomeworkById), new { id = createdHomework.Id }, createdHomework);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("UpdateHomeworkBy/{id}")]
        public async Task<IActionResult> UpdateHomeworkById(int id, [FromBody] Homework homework)
        {
            if (id != homework.Id)
            {
                return BadRequest("Teacher ID mismatch.");
            }
            
            try
            {
                var updatedHomework = await _homeworkService.UpdateHomeworkById(id, homework);
                return Ok(updatedHomework);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpPatch("SoftDeleteHomeworkBy/{id}")]
        public async Task<IActionResult> SoftDeleteHomeworkById(int id)
        {
            try
            {
                var result = await _homeworkService.SoftDeleteHomeworkById(id);
                return Ok(result);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpGet("GetHomeworksByCourse/{id}")]
        public async Task<IActionResult> GetHomeworksByCourseId(int id)
        {
            try
            {
                var homeworks = await _homeworkService.GetHomeworksByCourseId(id);
                return Ok(homeworks);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpGet("GetExpiredHomeworksByCourse/{id}")]
        public async Task<IActionResult> GetExpiredHomeworksByCourseId(int id)
        {
            try
            {
                var homeworks = await _homeworkService.GetExpiredHomeworksByCourseId(id);
                return Ok(homeworks);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
        
    }
}