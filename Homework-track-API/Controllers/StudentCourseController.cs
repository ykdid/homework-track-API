using Homework_track_API.DTOs;
using Homework_track_API.Entities;
using Homework_track_API.Services.StudentCourseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Homework_track_API.Controllers
{
    [Route("api/student-course")]
    [ApiController]

    public class StudentCourseController(IStudentCourseService studentCourseService) : ControllerBase
    {
        private IStudentCourseService _studentCourseService = studentCourseService;
        
        [Authorize(Policy = "Student")]
        [HttpPost("add-student/{studentId}/{courseId}")]
        public async Task<IActionResult> AddStudentToCourse(int studentId, int courseId)
        {
            if (studentId <= 0 || courseId <= 0)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Invalid studentId or courseId"));
            }

            try
            {
                var studentCourse = await _studentCourseService.AddStudentToCourse(studentId, courseId);
                return Ok(new ApiResponse<StudentCourse>(200, studentCourse, null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404, null, e.Message));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "StudentOrTeacher")]
        [HttpDelete("remove-student/{studentId}/{courseId}")]
        public async Task<IActionResult> RemoveStudentFromCourse(int studentId, int courseId)
        {
            if (studentId <= 0 || courseId <= 0)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Invalid studentId or courseId"));
            }

            try
            {
                var studentCourse = await _studentCourseService.RemoveStudentFromCourse(studentId, courseId);
                return Ok(new ApiResponse<bool>(200, true, null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404, null, e.Message));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "StudentOrTeacher")]
        [HttpGet("students/course/{courseId}")]
        public async Task<IActionResult> GetStudentsByCourseId(int courseId)
        {
            if (courseId <= 0)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Invalid courseId"));
            }

            try
            {
                var students = await _studentCourseService.GetStudentsByCourseId(courseId);
                return Ok(new ApiResponse<List<Student>>(200, students, null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404, null, e.Message));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "Student")]
        [HttpGet("courses/student/{studentId}")]
        public async Task<IActionResult> GetCoursesByStudentId(int studentId)
        {
            if (studentId <= 0)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Invalid studentId"));
            }

            try
            {
                var courses = await _studentCourseService.GetCoursesByStudentId(studentId);
                return Ok(new ApiResponse<List<Course>>(200, courses, null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404, null, e.Message));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
            }
        }
    }
}