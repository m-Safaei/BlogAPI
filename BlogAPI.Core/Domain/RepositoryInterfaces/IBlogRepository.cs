using BlogAPI.Core.Domain.Entities;

namespace BlogAPI.Core.Domain.RepositoryInterfaces;

public interface IBlogRepository
{
    Task<Blog> AddBlog(Blog blog);
     
    Task<Blog?> GetBlogById(Guid id);

    Task<List<Blog>> GetListOfBlogs();

    Task<Blog> UpdateBlog(Blog blog);

    Task<bool> BlogExists(Guid id);

    Task<bool> DeleteBlog(Guid id);
}

