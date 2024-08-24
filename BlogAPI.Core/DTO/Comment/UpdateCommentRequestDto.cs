using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Core.DTO.Comment;

public class UpdateCommentRequestDto
{
    [Required]
    [StringLength(10, MinimumLength = 3)]
    public string CommentTitle { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 5)]
    public string CommentMessage { get; set; }
}

