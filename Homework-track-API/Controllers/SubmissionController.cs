using Homework_track_API.Entities;
using Homework_track_API.Services.SubmissionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Homework_track_API.Controllers{

    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController(ISubmissionService submissionService):ControllerBase
    {
        private readonly ISubmissionService _submissionService = submissionService;
        
        [HttpGet("getAllSubmissions")]
        public async Task<IActionResult> GetAllSubmissions()
        {
            try
            {
                var submissions = await _submissionService.GetAllSubmissions();

                if (submissions == null)
                {
                    return NoContent();
                }

                return Ok(submissions);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        [Authorize(Policy = "StudentOrTeacher")]
        [HttpGet("getSubmissionBy/{id}")]
        public async Task<IActionResult> GetSubmissionById(int id)
        {
            try
            {
                var submission = await _submissionService.GetSubmissionById(id);

                if (submission == null)
                {
                    return NoContent();
                }

                return Ok(submission);
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

        [Authorize(Policy = "StudentOrTeacher")]
        [HttpGet("getSubmissionsByStudent/{id}")]
        public async Task<IActionResult> GetSubmissionsByStudentId(int id)
        {
            try
            {
                var submissions = await _submissionService.GetSubmissionsByStudentId(id);

                if (submissions == null)
                {
                    return NoContent();
                }
                
                return Ok(submissions);
                
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
        [HttpGet("getSubmissionsByHomework/{id}")]
        public async Task<IActionResult> GetSubmissionsByHomeworkId(int id)
        {
            try
            {
                var submissions = await _submissionService.GetSubmissionsByHomeworkId(id);

                if (submissions == null)
                {
                    return NoContent();
                }
                
                return Ok(submissions);
                
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
        [HttpPost("createSubmissionByStudent/{studentId}")]
        public async Task<IActionResult> CreateSubmissionByStudentId(int studentId ,Submission submission)
        {
            try
            {
                var createdSubmission = await _submissionService.CreateSubmissionByStudentId(studentId,submission);
                return CreatedAtAction(nameof(GetSubmissionById), new { id = createdSubmission.Id }, createdSubmission);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Policy = "Student")]
        [HttpDelete("deleteSubmissionBy/{id}")]
        public async Task<IActionResult> DeleteSubmissionById(int id)
        {
            try
            {
                var result = await _submissionService.DeleteSubmissionById(id);

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
        [HttpPatch("updateSubmissionBy/{id}")]
        public async Task<IActionResult> UpdateSubmissionById(int id, [FromBody] Submission submission)
        {
            if (id != submission.Id)
            {
                return BadRequest();
            }
            
            try
            {
                var updatedSubmission = await _submissionService.UpdateSubmission(id,submission);
                return Ok(submission);
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
        [HttpPatch("updateMarkBySubmission{submissionId}")]
        public async Task<IActionResult> UpdateMarkBySubmissionId(int submissionId, int mark)
        {
            if (submissionId <= 0)
            {
                 if (mark < 0 || mark > 100)
                 {
                     return BadRequest();
                 }
            }
            try
            {
                var submission = await _submissionService.UpdateMarkBySubmissionId(submissionId,mark);
                return Ok(submission);
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