using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.DTO.Comment;

namespace BlogAPI.Core.Mappers;

public static class CommentMappers
{
    public static CommentDto ToCommentDto(this Comment comment)
    {
        return new CommentDto()
        {
            Id = comment.Id,
            CommentTitle = comment.CommentTitle,
            CommentMessage = comment.CommentMessage,
            UserName = comment.UserName,
            CreatedDate = comment.CreatedDate,
            IsSeen = comment.IsSeen,
        };
    }
}

