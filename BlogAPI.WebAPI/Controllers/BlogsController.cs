using BlogAPI.Core.DTO;
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

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListOfBlogs()
        {
            return Ok(await _blogService.GetListOfBlogs());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlog(Guid id)
        {
            BlogDto? blogDto = await _blogService.GetBlogById(id);

            if (blogDto == null) return NotFound();
            return Ok(blogDto);
        }
    }
}
