using BlogAPI.Core.DTO;

namespace BlogAPI.Core.ServiceInterfaces;

public interface IBlogService
{
    Task<List<BlogDto>> GetListOfBlogs();

    Task<BlogDto?> GetBlogById(Guid id);
}

