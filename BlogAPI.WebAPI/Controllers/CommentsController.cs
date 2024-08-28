using BlogAPI.Core.DTO.Comment;
using BlogAPI.Core.ServiceInterfaces;
using BlogAPI.Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            return Ok(await _commentService.GetAllComments());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(Guid id)
        {
            CommentDto? comment = await _commentService.GetCommentById(id);

            if (comment == null) return NotFound();
            return Ok(comment);
        }

        [HttpPost("{blogId}")]
        public async Task<IActionResult> CreateComment(Guid blogId, CreateCommentRequestDto createCommentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Guid? userId = User.GetUserId();
            if (userId == null) return BadRequest("You should login first for sending comments!");

            CreateCommentResponseDto? commentResponse = await _commentService.CreateComment(
                                                       userId, blogId, createCommentDto);
            if (commentResponse == null)
            {
                return BadRequest("Blog does not exist.");
            }

            return CreatedAtAction(nameof(GetComment), new { id = commentResponse.Id }, commentResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(Guid id, UpdateCommentRequestDto commentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            UpdateCommentResponseDto? commentResponse = await _commentService.UpdateComment(id, commentDto);
            if (commentResponse == null) return NotFound();

            return Ok(commentResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            bool result = await _commentService.DeleteComment(id);

            if (!result) return NotFound();

            return NoContent();
        }
    }
}
