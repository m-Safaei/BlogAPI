using BlogAPI.Core.DTO.Blog;

namespace BlogAPI.Core.ServiceInterfaces;

public interface IBlogService
{
    Task<List<BlogDto>> GetListOfBlogs();

    Task<BlogDto?> GetBlogById(Guid id);

    Task<CreateBlogResponseDto> CreateBlog(CreateBlogRequestDto blogRequestDto);

    Task<UpdateBlogResponseDto?> UpdateBlog(Guid id,UpdateBlogRequestDto blogRequestDto);

    Task<bool> DeleteBlog(Guid id);
}

