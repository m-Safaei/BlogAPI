using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Core.DTO.Comment;

public class UpdateCommentResponseDto
{
    public string CommentTitle { get; set; }

  
    public string CommentMessage { get; set; }
}

