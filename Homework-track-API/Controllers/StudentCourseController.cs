using Homework_track_API.Services.StudentCourseService;
using Microsoft.AspNetCore.Mvc;

namespace Homework_track_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class StudentCourseController(IStudentCourseService studentCourseService) : ControllerBase
    {
        private IStudentCourseService _studentCourseService = studentCourseService;

        [HttpPost("AddStudentToCourseBy/{studentId}/{courseId}")]
        public async Task<IActionResult> AddStudentToCourse(int studentId, int courseId)
        {
            if (studentId <= 0 || courseId <= 0)
            {
                return BadRequest("Invalid studentId or courseId");
            }

            try
            {
                var studentCourse = await _studentCourseService.AddStudentToCourse(studentId, courseId);
                return Ok(studentCourse);
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

        [HttpDelete("RemoveStudentFromCourseBy/{studentId}/{courseId}")]
        public async Task<IActionResult> RemoveStudentFromCourse(int studentId, int courseId)
        {
            if (studentId <= 0 || courseId <= 0)
            {
                return BadRequest("Invalid studentId or courseId");
            }

            try
            {
                var studentCourse = await _studentCourseService.RemoveStudentFromCourse(studentId, courseId);
                return Ok(studentCourse);
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

        [HttpGet("GetStudentsByCourse/{courseId}")]
        public async Task<IActionResult> GetStudentsByCourseId(int courseId)
        {
            if (courseId <= 0)
            {
                return BadRequest("Invalid courseId");
            }

            try
            {
                var students = await _studentCourseService.GetStudentsByCourseId(courseId);
                return Ok(students);
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

        [HttpGet("GetCoursesByStudent/{studentId}")]
        public async Task<IActionResult> GetCoursesByStudentId(int studentId)
        {
            if (studentId <= 0)
            {
                return BadRequest("Invalid studentId");
            }

            try
            {
                var courses = await _studentCourseService.GetCoursesByStudentId(studentId);
                return Ok(courses);
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