using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.Domain.RepositoryInterfaces;
using BlogAPI.Core.DTO.Blog;
using BlogAPI.Core.Mappers;
using BlogAPI.Core.ServiceInterfaces;
using Microsoft.Extensions.Logging;

namespace BlogAPI.Core.Services;

public class BlogService : IBlogService
{
    private readonly IBlogRepository _blogRepository;
    private readonly ILogger<BlogService> _logger;

    public BlogService(IBlogRepository blogRepository,ILogger<BlogService> logger)
    {
        _blogRepository = blogRepository;
        _logger = logger;
    }

    public async Task<List<BlogDto>> GetListOfBlogs()
    {
        _logger.LogInformation("GetListOfBlogs of BlogService");

        List<Blog> blogs = await _blogRepository.GetListOfBlogs();

        return blogs.Select(b => b.ToBlogDto()).ToList();
    }

    public async Task<BlogDto?> GetBlogById(Guid id)
    {
        Blog? blog = await _blogRepository.GetBlogById(id);
        if (blog == null) return null;

        return blog.ToBlogDto();
    }

    public async Task<CreateBlogResponseDto> CreateBlog(CreateBlogRequestDto blogRequestDto)
    {
        Blog blog = blogRequestDto.ToBlog();

        Blog blogResponse = await _blogRepository.AddBlog(blog);

        return new CreateBlogResponseDto()
        {
            Id = blogResponse.Id,
            Title = blogResponse.Title,
            Content = blogResponse.Content,
        };
    }

    public async Task<UpdateBlogResponseDto?> UpdateBlog(Guid id,UpdateBlogRequestDto blogRequestDto)
    {
        Blog? blog = await _blogRepository.GetBlogById(id);

        if (blog == null) return null;

        blog.Title = blogRequestDto.Title;
        blog.Content = blogRequestDto.Content;

        Blog blogResponse = await _blogRepository.UpdateBlog(blog);
        return new UpdateBlogResponseDto()
        {
            Title = blogResponse.Title,
            Content = blogResponse.Content,
        };
    }

    public async Task<bool> DeleteBlog(Guid id)
    {
        return await _blogRepository.DeleteBlog(id);
    }
}

