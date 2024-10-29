using Homework_track_API.DTOs;
using Homework_track_API.Entities;
using Homework_track_API.Services.StudentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Homework_track_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController(IStudentService studentService) : ControllerBase
    {
        private readonly IStudentService _studentService = studentService;
        
        [HttpGet("getAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudents();
                
                if (students == null || !students.Any())
                {
                    return NoContent();
                }

                return Ok(students);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [Authorize(Policy = "StudentOrTeacher")]
        [HttpGet("getStudentBy/{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            try
            {
                var student = await _studentService.GetStudentById(id);

                if (student == null)
                {
                    return NoContent();
                }

                return Ok(student);
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

        [Authorize(Policy = "Student")]
        [HttpDelete("deleteStudentBy/{id}")]
        public async Task<IActionResult> DeleteStudentById(int id)
        {
            try
            {
                var result = await _studentService.DeleteStudentById(id);

                if (result)
                {
                    return NoContent();
                }

                return NotFound();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Policy = "Student")]
        [HttpPost("createStudent")]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            try
            {
                var createdStudent = await _studentService.CreateStudent(student);
                return CreatedAtAction(nameof(GetStudentById), new { id = createdStudent.Id }, createdStudent);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Policy = "Student")]
        [HttpPatch("updateStudentBy/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
        {
            if (id != student.Id)
            {
                return BadRequest("Student ID mismatch.");
            }

            try
            {
                var updatedStudent = await _studentService.UpdateStudent(id,student);
                return Ok(updatedStudent);
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

        [Authorize(Policy = "Student")]
        [HttpPatch("changeStudentPasswordBy/{id}")]
        public async Task<IActionResult> ChangePasswordById(int id, [FromBody] ChangePassword changePassword)
        {
            try
            {
                var result = await _studentService.ChangePasswordById(id , changePassword.currentPassword, changePassword.newPassword);

                if (result)
                {
                    return Ok("Password changed successfully");
                }
                else
                {
                    return BadRequest("Failed to change password.");
                }
                
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
