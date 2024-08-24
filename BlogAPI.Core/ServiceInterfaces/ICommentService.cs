using BlogAPI.Core.DTO.Comment;

namespace BlogAPI.Core.ServiceInterfaces;

public interface ICommentService
{
    Task<List<CommentDto>> GetAllComments();

    Task<CommentDto?> GetCommentById(Guid id);

    Task<CreateCommentResponseDto?> CreateComment(Guid? userId, Guid blogId, CreateCommentRequestDto createCommentRequestDto);

    Task<UpdateCommentResponseDto?> UpdateComment(Guid id, UpdateCommentRequestDto commentDto);

    Task<bool> DeleteComment(Guid id);
}

