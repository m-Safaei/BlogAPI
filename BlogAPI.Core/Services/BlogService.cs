using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.Domain.RepositoryInterfaces;
using BlogAPI.Core.DTO.Blog;
using BlogAPI.Core.Mappers;
using BlogAPI.Core.ServiceInterfaces;

namespace BlogAPI.Core.Services;

public class BlogService : IBlogService
{
    private readonly IBlogRepository _blogRepository;

    public BlogService(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<List<BlogDto>> GetListOfBlogs()
    {
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

