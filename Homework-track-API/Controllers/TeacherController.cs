using Homework_track_API.Services.TeacherService;
using Microsoft.AspNetCore.Mvc;
using Homework_track_API.Entities;

namespace Homework_track_API.Controllers{

    [Route("api/[controller]")]
    [ApiController]

    public class TeacherController:ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }
        
        [HttpGet("getAllTeachers")]
        public async Task<IActionResult> GetAllTeachers()
        {
            try
            {
                var teachers = await _teacherService.GetAllTeachers();
                                
                if (teachers == null || !teachers.Any())
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
        
        [HttpPatch("updateTeacherBy/{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] Teacher teacher)
        {
                        
            if (id != teacher.Id)
            {
                return BadRequest("Teacher ID mismatch.");
            }

            try
            {

                var updatedSTeacher = await _teacherService.UpdateTeacher(teacher);
                return Ok(updatedSTeacher);

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