using Homework_track_API.DTOs;
using Homework_track_API.Services.TeacherService;
using Microsoft.AspNetCore.Mvc;
using Homework_track_API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Homework_track_API.Controllers{

    [Route("api/teacher")]
    [ApiController]

    public class TeacherController(ITeacherService teacherService):ControllerBase
    {
        private readonly ITeacherService _teacherService = teacherService;
        
        [HttpGet("teachers")]
        public async Task<IActionResult> GetAllTeachers()
        {
            try
            {
                var teachers = await _teacherService.GetAllTeachers();

                if (teachers.IsNullOrEmpty() || !teachers.Any())
                {
                    return NoContent();
                }

                return Ok(new ApiResponse<IEnumerable<Teacher>>(200, teachers, null));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
            }
        }
        
        [Authorize(Policy = "StudentOrTeacher")]
        [HttpGet("teacher/{id}")]
        public async Task<IActionResult> GetTeacherById(int id)
        {
            try
            {
                var teacher = await _teacherService.GetTeacherById(id);

                if (teacher == null)
                {
                    return NotFound(new ApiResponse<string>(404, null, "Teacher not found"));
                }

                return Ok(new ApiResponse<Teacher>(200, teacher, null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404, null, e.Message));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ApiResponse<string>(400, null, e.Message));
            }
        }
        
        [Authorize(Policy = "Teacher")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTeacherById(int id)
        {
            try
            {
                var result = await _teacherService.DeleteTeacherById(id);

                if (result)
                {
                    return NoContent();
                }

                return NotFound(new ApiResponse<string>(404, null, "Teacher not found"));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ApiResponse<string>(400, null, e.Message));
            }
        }
        
        [Authorize(Policy = "Teacher")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateTeacher(Teacher teacher)
        {
            try
            {
                var createdTeacher = await _teacherService.CreateTeacher(teacher);
                return CreatedAtAction(nameof(GetTeacherById), new { id = createdTeacher.Id }, new ApiResponse<Teacher>(201, createdTeacher, null));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ApiResponse<string>(400, null, e.Message));
            }
        }
        
        [Authorize(Policy = "Teacher")]
        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Teacher ID mismatch."));
            }

            try
            {
                var updatedTeacher = await _teacherService.UpdateTeacher(id, teacher);
                return Ok(new ApiResponse<Teacher>(200, updatedTeacher, null));
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
        
        [Authorize(Policy = "Teacher")]
        [HttpPatch("change-password/{id}")]
        public async Task<IActionResult> ChangePasswordById(int id, [FromBody] ChangePassword changePassword)
        {
            try
            {
                var result = await _teacherService.ChangePasswordById(id, changePassword.currentPassword, changePassword.newPassword);

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
        }
    }
}