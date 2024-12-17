using Homework_track_API.DTOs;
using Homework_track_API.Entities;
using Homework_track_API.Services.StudentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Homework_track_API.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController(IStudentService studentService) : ControllerBase
    {
        private readonly IStudentService _studentService = studentService;
        
        [HttpGet("students")]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudents();

                if (students == null || !students.Any())
                {
                    return NoContent();
                }

                return Ok(new ApiResponse<IEnumerable<Student>>(200, students, null));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "StudentOrTeacher")]
        [HttpGet("student/{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Invalid Student ID"));
            }

            try
            {
                var student = await _studentService.GetStudentById(id);

                if (student == null)
                {
                    return NotFound(new ApiResponse<string>(404, null, "Student not found"));
                }

                return Ok(new ApiResponse<Student>(200, student, null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404, null, $"Student not found: {e.Message}"));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ApiResponse<string>(400, null, e.Message));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "Student")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteStudentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Invalid Student ID"));
            }

            try
            {
                var result = await _studentService.DeleteStudentById(id);

                if (result)
                {
                    return NoContent();
                }

                return NotFound(new ApiResponse<string>(404, null, "Student not found"));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ApiResponse<string>(400, null, e.Message));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "Student")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            if (student == null)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Student data is null"));
            }

            try
            {
                var createdStudent = await _studentService.CreateStudent(student);
                return CreatedAtAction(nameof(GetStudentById), new { id = createdStudent.Id }, new ApiResponse<Student>(201, createdStudent, null));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ApiResponse<string>(400, null, e.Message));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "Student")]
        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
        {
            if (id != student.Id)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Student ID mismatch"));
            }

            try
            {
                var updatedStudent = await _studentService.UpdateStudent(id, student);
                return Ok(new ApiResponse<Student>(200, updatedStudent, null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404, null, $"Student not found: {e.Message}"));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "Student")]
        [HttpPatch("change-password/{id}")]
        public async Task<IActionResult> ChangePasswordById(int id, [FromBody] ChangePassword changePassword)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Invalid Student ID"));
            }

            try
            {
                var result = await _studentService.ChangePasswordById(id, changePassword.currentPassword, changePassword.newPassword);

                if (result)
                {
                    return Ok(new ApiResponse<string>(200, "Password changed successfully", null));
                }
                else
                {
                    return BadRequest(new ApiResponse<string>(400, null, "Failed to change password."));
                }
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(404, null, ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ApiResponse<string>(401, null, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {ex.Message}"));
            }
        }
    }
}
