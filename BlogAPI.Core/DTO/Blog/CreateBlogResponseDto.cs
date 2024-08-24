namespace BlogAPI.Core.DTO.Blog;

public class CreateBlogResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    public string Content { get; set; }
}

