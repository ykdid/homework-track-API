using Homework_track_API.DTOs;
using Homework_track_API.Entities;
using Homework_track_API.Services.HomeworkService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Homework_track_API.Controllers
{
    [Route("api/homework")]
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

                return Ok(new ApiResponse<List<Homework>>(200,homeworks,null));
            }
            catch (Exception e) 
            {
                return StatusCode(500, new ApiResponse<string>(500,null,$"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "StudentOrTeacher")]
        [HttpGet("getHomeworkBy/{id}")]
        public async Task<IActionResult> GetHomeworkById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Invalid Homework Id"));
            }

            try
            {
                var homework = await _homeworkService.GetHomeworkById(id);

                if (homework == null)
                {
                    return NotFound(new ApiResponse<string>(404, null, "Homework not found"));
                }

                return Ok(new ApiResponse<Homework>(200, homework, null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404, null, $"Key not found: {e.Message}"));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ApiResponse<string>(400, null, $"Invalid argument: {e.Message}"));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
            }
        }
        
        [Authorize(Policy = "Teacher")]
        [HttpPost("createHomeworkByCourse/{courseId}")]
        public async Task<IActionResult> CreateHomeworkByCourseId(int courseId, Homework homework)
        {
            if (courseId <= 0)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Invalid Course Id"));
            }

            if (homework == null)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Homework data is required"));
            }

            try
            {
                var createdHomework = await _homeworkService.CreateHomeworkByCourseId(courseId, homework);
                return CreatedAtAction(
                    nameof(GetHomeworkById),
                    new { id = createdHomework.Id },
                    new ApiResponse<Homework>(201, createdHomework, null)
                );
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "Teacher")]
        [HttpPatch("updateHomeworkBy/{id}")]
        public async Task<IActionResult> UpdateHomeworkById(int id, [FromBody] Homework homework)
        {
            if (id != homework.Id)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Homework ID mismatch"));
            }

            try
            {
                var updatedHomework = await _homeworkService.UpdateHomeworkById(id, homework);
                return Ok(new ApiResponse<Homework>(200, updatedHomework, null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404, null, $"Homework not found: {e.Message}"));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "Teacher")]
        [HttpPatch("softDeleteHomeworkBy/{id}")]
        public async Task<IActionResult> SoftDeleteHomeworkById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Invalid Homework ID"));
            }

            try
            {
                var result = await _homeworkService.SoftDeleteHomeworkById(id);
                if (!result)
                {
                    return NotFound(new ApiResponse<string>(404, null, "Homework not found"));
                }

                return Ok(new ApiResponse<bool>(200, result, null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404, null, $"Homework not found: {e.Message}"));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "StudentOrTeacher")]
        [HttpGet("getHomeworksByCourse/{id}")]
        public async Task<IActionResult> GetHomeworksByCourseId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Invalid Course ID"));
            }

            try
            {
                var homeworks = await _homeworkService.GetHomeworksByCourseId(id);
                if (homeworks == null || !homeworks.Any())
                {
                    return NotFound(new ApiResponse<string>(404, null, "No homeworks found for the given course ID"));
                }

                return Ok(new ApiResponse<List<Homework>>(200, homeworks, null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404, null, $"Homeworks not found: {e.Message}"));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "Teacher")]
        [HttpGet("getExpiredHomeworksByCourse/{id}")]
        public async Task<IActionResult> GetExpiredHomeworksByCourseId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Invalid Course ID"));
            }

            try
            {
                var homeworks = await _homeworkService.GetExpiredHomeworksByCourseId(id);
                if (homeworks == null || !homeworks.Any())
                {
                    return NotFound(new ApiResponse<string>(404, null, "No expired homeworks found for the given course ID"));
                }

                return Ok(new ApiResponse<List<Homework>>(200, homeworks, null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404, null, $"Expired homeworks not found: {e.Message}"));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
            }
        }
        
    }
}