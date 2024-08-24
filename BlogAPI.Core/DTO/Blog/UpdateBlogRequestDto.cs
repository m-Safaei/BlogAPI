using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Core.DTO.Blog;

public class UpdateBlogRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; }

    [Required]
    [StringLength(2000, MinimumLength = 20)]
    public string Content { get; set; }
}

