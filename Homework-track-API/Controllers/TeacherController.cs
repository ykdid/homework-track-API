using Homework_track_API.DTOs;
using Homework_track_API.Services.TeacherService;
using Microsoft.AspNetCore.Mvc;
using Homework_track_API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Homework_track_API.Controllers{

    [Route("api/[controller]")]
    [ApiController]

    public class TeacherController(ITeacherService teacherService):ControllerBase
    {
        private readonly ITeacherService _teacherService = teacherService;
        
        [HttpGet("getAllTeachers")]
        public async Task<IActionResult> GetAllTeachers()
        {
            try
            {
                var teachers = await _teacherService.GetAllTeachers();
                                
                if (teachers.IsNullOrEmpty() || !teachers.Any())
                {
                    return NoContent();
                }

                return Ok(teachers);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
        
        [Authorize(Policy = "StudentOrTeacher")]
        [HttpGet("getTeacherBy/{id}")]
        public async Task<IActionResult> GetTeacherById(int id)
        {
            try
            {
                var teacher = await _teacherService.GetTeacherById(id);

                if (teacher == null)
                {
                    return NoContent(); 
                }

                return Ok(teacher);
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
        
        [Authorize(Policy = "Teacher")]
        [HttpDelete("deleteTeacherBy/{id}")]
        public async Task<IActionResult> DeleteTeacherById(int id)
        {
            try
            {
                var result = await _teacherService.DeleteTeacherById(id);

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
        
        [Authorize(Policy = "Teacher")]
        [HttpPost("createTeacher")]
        public async Task<IActionResult> CreateTeacher(Teacher teacher)
        {
            try
            {
                var createdTeacher = await _teacherService.CreateTeacher(teacher);
                return CreatedAtAction(nameof(GetTeacherById), new { id = createdTeacher.Id }, createdTeacher);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [Authorize(Policy = "Teacher")]
        [HttpPatch("updateTeacherBy/{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] Teacher teacher)
        {
                        
            if (id != teacher.Id)
            {
                return BadRequest("Teacher ID mismatch.");
            }

            try
            {

                var updatedTeacher = await _teacherService.UpdateTeacher(id ,teacher);
                return Ok(updatedTeacher);

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
        
        [Authorize(Policy = "Teacher")]
        [HttpPatch("changeTeacherPasswordBy/{id}")]
        public async Task<IActionResult> ChangePasswordById(int id, [FromBody] ChangePassword changePassword)
        {
            try
            {
                var result = await _teacherService.ChangePasswordById(id , changePassword.currentPassword, changePassword.newPassword);

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