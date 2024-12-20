using Homework_track_API.DTOs;
using Homework_track_API.Entities;
using Homework_track_API.Enums;
using Homework_track_API.Services.CourseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Homework_track_API.Controllers
{
    [Route("api/course")]
    [ApiController]
    
    public class CourseController(ICourseService courseService): ControllerBase
    {
        private readonly ICourseService _courseService = courseService;
        
        [HttpGet("courses")]
        public async Task<IActionResult> GetAllCourses()
        {
            try
            {
                var courses = await _courseService.GetAllCourses();
                return Ok(new ApiResponse<List<Course>>(200,courses,null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404,null,$"Not Found: {e.Message}"));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500,null,$"Internal server error: {e.Message}"));
            }
        }
        
        [Authorize(Policy = "StudentOrTeacher")]
        [HttpGet("course/{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiResponse<string>(400,null,"Invalid Course Id"));
            }

            try
            {
                var course = await _courseService.GetCourseById(id);
                return Ok(new ApiResponse<Course>(200,course,null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404,null,$"Not Found: {e.Message}"));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500,null,$"Internal server error: {e.Message}"));
            }
        }
        
        [Authorize(Policy = "TeacherOnly")]
        [HttpGet("teacher/{id}")]
        public async Task<IActionResult> GetActiveCoursesByTeacherId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiResponse<string>(400,null,"Invalid Teacher Id"));
            }

            try
            {
                var courses = await _courseService.GetActiveCoursesByTeacherId(id);
                return Ok(new ApiResponse<List<Course>>(200,courses,null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404,null,$"Not Found: {e.Message}"));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500,null,$"Internal server error: {e.Message}"));
            }
        }
        
        [Authorize(Policy = "TeacherOnly")]
        [HttpGet("archived/teacher/{id}")]
        public async Task<IActionResult> GetArchivedCoursesByTeacherId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiResponse<string>(400,null,"Invalid Teacher Id"));
            }

            try
            {
                var courses = await _courseService.GetArchivedCoursesByTeacherId(id);
                return Ok(new ApiResponse<List<Course>>(200,courses,null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404,null,$"Not Found: {e.Message}"));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500,null,$"Internal server error: {e.Message}"));
            }
        }
        
        [Authorize(Policy = "TeacherOnly")]
        [HttpGet("deleted/teacher/{id}")]
        public async Task<IActionResult> GetDeletedCoursesByTeacherId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiResponse<string>(400,null,"Invalid Teacher Id"));
            }

            try
            {
                var courses = await _courseService.GetDeletedCoursesByTeacherId(id);
                return Ok(new ApiResponse<List<Course>>(200,courses,null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404,null,$"Not Found: {e.Message}"));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500,null,$"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "TeacherOnly")]
        [HttpPost("create/teacher/{id}")]
        public async Task<IActionResult> CreateCourseByTeacherId(int id , [FromBody] CreateCourse courseDto)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiResponse<string>(400,null,"Invalid Teacher Id"));
            }

            try
            {
                var createdCourse = await _courseService.CreateCourseByTeacherId(id , courseDto);
                return Ok(new ApiResponse<Course>(200,createdCourse,null));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500,null,$"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetCourseByCode(string code)
        {
            if (code.Length !=  10)
            {
                return BadRequest(new ApiResponse<string>(400,null,"Code length is not enough"));
            }

            try
            {
                var course = await _courseService.GetCourseByCode(code);
                return Ok(new ApiResponse<Course>(200,course,null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404,null,$"Not Found: {e.Message}")

                );
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500,null,$"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "TeacherOnly")]
        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateCourseById(int id , Course course)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiResponse<string>(400,null,"Invalid Course Id"));
            }
            
            try
            {
                var updatedCourse = await _courseService.UpdateCourseById(id, course);
                return Ok(new ApiResponse<Course>(200,updatedCourse,null));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500,null,$"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "TeacherOnly")]
        [HttpGet("find-course/teacher/{teacherId}")]
        public async Task<IActionResult> FindCoursesByTeacherId(int teacherId, string courseName)
        {
            if (teacherId <= 0)
            {
                return BadRequest(new ApiResponse<string>(400,null,"Invalid Teacher Id"));
            }

            if (courseName.Length == 0)
            {
                return BadRequest(new ApiResponse<string>(400,null,"Need Input Character"));
            }

            try
            {
                var courses = await _courseService.FindCoursesByTeacherId(teacherId, courseName);
                if (courses == null || !courses.Any())
                {
                    return NotFound(new ApiResponse<string>(404,null,"Not Found"));
                }
                return Ok(new ApiResponse<IEnumerable<Course>>(200,courses,null));
            }   
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500,null,$"Internal server error: {e.Message}"));
            }
        }
        
        [Authorize(Policy = "StudentOnly")]
        [HttpGet("find-course/student/{studentId}")]
        public async Task<IActionResult> FindCoursesByStudentId(int studentId, string courseName)
        {
            if (studentId <= 0)
            {
                return BadRequest(new ApiResponse<string>(400,null,"Invalid Student Id"));
            }

            if (courseName.Length == 0)
            {
                return BadRequest(new ApiResponse<string>(400,null,"Need Input Character"));
            }

            try
            {
                var courses = await _courseService.FindCoursesByStudentId(studentId, courseName);
                if (courses == null || !courses.Any())
                {
                    return NotFound(new ApiResponse<string>(404,null,"Not Found"));
                }
                return Ok(new ApiResponse<IEnumerable<Course>>(200,courses,null));
            }   
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500,null,$"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "TeacherOnly")]
        [HttpPatch("change-status/course/{courseId}")]
        public async Task<IActionResult> ChangeCourseStatus(int courseId, CourseStatus newStatus)
        {
            if (courseId <= 0)
            {
                return BadRequest(new ApiResponse<string>(400,null,"Invalid Student Id"));
            }
            
            if (!Enum.IsDefined(typeof(CourseStatus),newStatus))
            {
                return BadRequest(new ApiResponse<string>(400,null,"Invalid Status Type"));
            }

            try
            {
                var result = await _courseService.ChangeCourseStatus(courseId, newStatus);
                return Ok(new ApiResponse<bool>(200, true, null));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500,null,$"Internal server error: {e.Message}"));
            }
        }
    }
}



