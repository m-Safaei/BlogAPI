using BlogAPI.Core.DTO.Blog;
using BlogAPI.Core.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly ILogger<BlogsController> _logger;

        public BlogsController(IBlogService blogService,ILogger<BlogsController> logger)
        {
            _blogService = blogService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetListOfBlogs()
        {
            _logger.LogInformation("GetListOfBlogs action method of BlogsController");

            return Ok(await _blogService.GetListOfBlogs());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlog(Guid id)
        {
            BlogDto? blogDto = await _blogService.GetBlogById(id);

            if (blogDto == null) return NotFound();
            return Ok(blogDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog(CreateBlogRequestDto createBlogDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            CreateBlogResponseDto responseBlog = await _blogService.CreateBlog(createBlogDto);

            return CreatedAtAction(nameof(GetBlog), new { id = responseBlog.Id }, responseBlog);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(Guid id, UpdateBlogRequestDto updateBlogDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            UpdateBlogResponseDto? blogResponseDto = await _blogService.UpdateBlog(id, updateBlogDto);

            if(blogResponseDto == null) return NotFound();

            return Ok(blogResponseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            bool isDeleted = await _blogService.DeleteBlog(id);

            if(!isDeleted) return NotFound();

            return NoContent();
        }
    }
}
