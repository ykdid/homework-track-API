using Homework_track_API.DTOs;
using Homework_track_API.Entities;
using Homework_track_API.Services.CourseService;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("getCourseBy/{id}")]
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
        [HttpGet("getActiveCoursesByTeacher/{id}")]
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
        [HttpGet("getArchivedCoursesByTeacher/{id}")]
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
        [HttpGet("getDeletedCoursesByTeacher/{id}")]
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
        [HttpPost("createCourseByTeacher/{id}")]
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
        [HttpGet("getCourseByCode/{code}")]
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
        [HttpPatch("updateCourseBy/{id}")]
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
        [HttpPatch("softDeleteCourseBy/{id}")]
        public async Task<IActionResult> SoftDeleteCourseById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiResponse<string>(400,null,"Invalid Course Id"));
            }

            try
            {
                var result = await _courseService.SoftDeleteCourseById(id);
                return Ok(new ApiResponse<bool>(200,true,null));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500,null,$"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "TeacherOnly")]
        [HttpPatch("archiveCourseBy/{id}")]
        public async Task<IActionResult> ArchiveCourseById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiResponse<string>(400,null,"Invalid Course Id"));
            }

            try
            {
                var result = await _courseService.ArchiveCourseById(id);
                return Ok(new ApiResponse<bool>(200,true,null)
                );
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500,null,$"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "TeacherOnly")]
        [HttpGet("findCoursesByTeacher/{teacherId}")]
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
        [HttpGet("findCoursesByStudent/{studentId}")]
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
    }
}



