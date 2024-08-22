using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.Domain.RepositoryInterfaces;
using BlogAPI.Core.DTO;
using BlogAPI.Core.Mappers;
using BlogAPI.Core.ServiceInterfaces;

namespace BlogAPI.Core.Services;

public class BlogService:IBlogService
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
}

