using Homework_track_API.DTOs;
using Homework_track_API.Entities;
using Homework_track_API.Services.SubmissionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Homework_track_API.Controllers{

    [Route("api/submission")]
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

                if (submissions == null || !submissions.Any())
                {
                    return NoContent();
                }

                return Ok(new ApiResponse<List<Submission>>(200, submissions, null));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
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

                return Ok(new ApiResponse<Submission>(200, submission, null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404, null, e.Message));
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

        [Authorize(Policy = "StudentOrTeacher")]
        [HttpGet("getSubmissionsByStudent/{id}")]
        public async Task<IActionResult> GetSubmissionsByStudentId(int id)
        {
            try
            {
                var submissions = await _submissionService.GetSubmissionsByStudentId(id);

                if (submissions == null || !submissions.Any())
                {
                    return NoContent();
                }

                return Ok(new ApiResponse<IEnumerable<Submission>>(200, submissions, null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404, null, e.Message));
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
        
        [Authorize(Policy = "Teacher")]
        [HttpGet("getSubmissionsByHomework/{id}")]
        public async Task<IActionResult> GetSubmissionsByHomeworkId(int id)
        {
            try
            {
                var submissions = await _submissionService.GetSubmissionsByHomeworkId(id);

                if (submissions == null || !submissions.Any())
                {
                    return NoContent();
                }

                return Ok(new ApiResponse<List<Submission>>(200, submissions, null));
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new ApiResponse<string>(404, null, e.Message));
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
        [HttpPost("createSubmissionByStudent/{studentId}")]
        public async Task<IActionResult> CreateSubmissionByStudentId(int studentId, Submission submission)
        {
            try
            {
                var createdSubmission = await _submissionService.CreateSubmissionByStudentId(studentId, submission);
                return CreatedAtAction(nameof(GetSubmissionById), new { id = createdSubmission.Id }, createdSubmission);
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
                return BadRequest(new ApiResponse<string>(400, null, e.Message));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Internal server error: {e.Message}"));
            }
        }

        [Authorize(Policy = "Student")]
        [HttpPatch("updateSubmissionBy/{id}")]
        public async Task<IActionResult> UpdateSubmissionById(int id, [FromBody] Submission submission)
        {
            if (id != submission.Id)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Submission ID mismatch"));
            }
    
            try
            {
                var updatedSubmission = await _submissionService.UpdateSubmission(id, submission);
                return Ok(updatedSubmission);
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
        [HttpPatch("updateMarkBySubmission{submissionId}")]
        public async Task<IActionResult> UpdateMarkBySubmissionId(int submissionId, int mark)
        {
            if (submissionId <= 0)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Invalid submission ID."));
            }

            if (mark < 0 || mark > 100)
            {
                return BadRequest(new ApiResponse<string>(400, null, "Mark must be between 0 and 100."));
            }

            try
            {
                var submission = await _submissionService.UpdateMarkBySubmissionId(submissionId, mark);
                return Ok(new ApiResponse<Submission>(200, submission, null));
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