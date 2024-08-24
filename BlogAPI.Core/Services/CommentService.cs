using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.Domain.RepositoryInterfaces;
using BlogAPI.Core.DTO.Comment;
using BlogAPI.Core.Mappers;
using BlogAPI.Core.ServiceInterfaces;

namespace BlogAPI.Core.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBlogRepository _blogRepository;

    public CommentService(ICommentRepository commentRepository
                         ,IUserRepository userRepository
                         ,IBlogRepository blogRepository)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _blogRepository = blogRepository;
    }

    public async Task<List<CommentDto>> GetAllComments()
    {
        List<Comment> comments = await _commentRepository.GetAllComments();

        return comments.Select(c => c.ToCommentDto()).ToList();
    }

    public async Task<CommentDto?> GetCommentById(Guid id)
    {
        Comment? comment = await _commentRepository.GetCommentById(id);

        if (comment == null) return null;

        return comment.ToCommentDto();
    }

    public async Task<CreateCommentResponseDto?> CreateComment(Guid? userId, Guid blogId, CreateCommentRequestDto createCommentRequestDto)
    {
        Blog? blog = await _blogRepository.GetBlogById(blogId);
        if (blog == null) return null;

        ApplicationUser? user = await _userRepository.GetUserById(userId);
        if (user == null) return null;

        Comment comment = new Comment()
        {
            ApplicationUserId = user.Id,
            BlogId = blogId,
            UserName = user.FirstName + " " + user.LastName,
            CommentTitle = createCommentRequestDto.CommentTitle,
            CommentMessage = createCommentRequestDto.CommentMessage
        };
        Comment createdComment = await _commentRepository.AddComment(comment);

        return new CreateCommentResponseDto()
        {
            Id = createdComment.Id,
            UserName = createdComment.UserName,
            BlogId = createdComment.BlogId,
            CreatedDate = createdComment.CreatedDate,
            IsSeen = createdComment.IsSeen,
            CommentTitle = createdComment.CommentTitle,
            CommentMessage = createdComment.CommentMessage,
        };
    }

    public async Task<UpdateCommentResponseDto?> UpdateComment(Guid id, UpdateCommentRequestDto commentDto)
    {
        Comment? comment = await _commentRepository.GetCommentById(id);
        if (comment == null) return null;

        comment.CommentTitle = commentDto.CommentTitle;
        comment.CommentMessage = commentDto.CommentMessage;

        Comment updatedComment = await _commentRepository.UpdateComment(comment);

        return new UpdateCommentResponseDto()
        {
            CommentTitle = updatedComment.CommentTitle,
            CommentMessage = updatedComment.CommentMessage
        };
    }

    public async Task<bool> DeleteComment(Guid id)
    {
        return await _commentRepository.DeleteComment(id);
    }
}

