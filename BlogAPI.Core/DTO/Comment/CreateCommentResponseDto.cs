namespace BlogAPI.Core.DTO.Comment;

public class CreateCommentResponseDto
{
    public Guid Id { get; set; }

    public string CommentTitle { get; set; }

    public string CommentMessage { get; set; }

    public string UserName { get; set; } = string.Empty;

    public bool IsSeen { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid BlogId { get; set; }
}

