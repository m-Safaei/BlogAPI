using BlogAPI.Core.Domain.Entities;

namespace BlogAPI.Core.Domain.RepositoryInterfaces;

public interface ICommentRepository
{
    Task<Comment> AddComment(Comment comment);

    Task<Comment?> GetCommentById(Guid id);

    Task<List<Comment>> GetAllComments();

    Task<Comment> UpdateComment(Comment comment);

    Task<bool> DeleteComment(Guid id);

    Task<bool> CommentExists(Guid id);

    Task<List<Comment>> GetCommentsByUserId(Guid userId);

    Task<List<Comment>> GetCommentsByBlogId(Guid blogId);


}

