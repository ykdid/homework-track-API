using Homework_track_API.Entities;
using Homework_track_API.Services.CourseService;
using Microsoft.AspNetCore.Mvc;

namespace Homework_track_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CourseController(ICourseService courseService): ControllerBase
    {
        private readonly ICourseService _courseService = courseService;

        [HttpGet("getAllCourses")]
        public async Task<IActionResult> GetAllCourses()
        {
            try
            {
                var courses = await _courseService.GetAllCourses();
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

        [HttpGet("getCourseBy/{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid course ID.");
            }

            try
            {
                var course = await _courseService.GetCourseById(id);
                return Ok(course);
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

        [HttpGet("getCoursesByTeacher/{id}")]
        public async Task<IActionResult> GetCoursesByTeacherId(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid teacher ID.");
            }

            try
            {
                var courses = await _courseService.GetCoursesByTeacherId(id);
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

        [HttpPost("createCourseByTeacher/{id}")]
        public async Task<IActionResult> CreateCourseByTeacherId(int id , [FromBody] Course course)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid teacher ID.");
            }

            try
            {
                var createdCourse = await _courseService.CreateCourseByTeacherId(id , course);
                return Ok(createdCourse);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpGet("getCourseByCode/{code}")]
        public async Task<IActionResult> GetCourseByCode(string code)
        {
            if (code.Length !=  10)
            {
                return BadRequest("Course code should be 10 characters.");
            }

            try
            {
                var course = await _courseService.GetCourseByCode(code);
                return Ok(course);
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

        [HttpPatch("updateCourseBy/{id}")]
        public async Task<IActionResult> UpdateCourseById(int id , Course course)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }
            
            try
            {
                var updatedCourse = await _courseService.UpdateCourseById(id, course);
                return Ok(updatedCourse);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpPatch("softDeleteCourseBy/{id}")]
        public async Task<IActionResult> SoftDeleteCourseById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid course ID.");
            }

            try
            {
                var result = await _courseService.SoftDeleteCourseById(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [HttpPatch("archiveCourseBy/{id}")]
        public async Task<IActionResult> ArchiveCourseById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid course ID.");
            }

            try
            {
                var result = await _courseService.ArchiveCourseById(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
    }
}



