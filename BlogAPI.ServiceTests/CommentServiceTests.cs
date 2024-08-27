using AutoFixture;
using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.Domain.RepositoryInterfaces;
using BlogAPI.Core.DTO.Comment;
using BlogAPI.Core.Mappers;
using BlogAPI.Core.ServiceInterfaces;
using BlogAPI.Core.Services;
using FluentAssertions;
using Moq;

namespace BlogAPI.ServiceTests;

public class CommentServiceTests
{
    private readonly IFixture _fixture;
    private readonly ICommentRepository _commentRepository;
    private readonly Mock<ICommentRepository> _commentRepositoryMock;
    private readonly ICommentService _commentService;
    private readonly IBlogRepository _blogRepository;
    private readonly Mock<IBlogRepository> _blogRepositoryMock;
    private readonly IUserRepository _userRepository;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    public CommentServiceTests()
    {
        _fixture = new Fixture();

        _commentRepositoryMock = new Mock<ICommentRepository>();
        _commentRepository = _commentRepositoryMock.Object;

        _blogRepositoryMock = new Mock<IBlogRepository>();
        _blogRepository = _blogRepositoryMock.Object;

        _userRepositoryMock = new Mock<IUserRepository>();
        _userRepository = _userRepositoryMock.Object;

        _commentService = new CommentService(_commentRepository,_userRepository,_blogRepository);

    }

    [Fact]
    public async Task GetAllComments_ReturnEmptyList()
    {
        //Arrange
        _commentRepositoryMock.Setup(m => m.GetAllComments())
            .ReturnsAsync(new List<Comment>());
        //Act
        List<CommentDto> expectedComments = await _commentService.GetAllComments();
        //Assert 
        expectedComments.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllComments_WithFewComments_ToBeSuccessful()
    {
        //Arrange
        List<Comment> comments = new()
        {
            _fixture.Build<Comment>().With(c => c.ApplicationUser, null as ApplicationUser)
                .With(c => c.Blog, null as Blog).Create(),
            _fixture.Build<Comment>().With(c => c.ApplicationUser, null as ApplicationUser)
                .With(c => c.Blog, null as Blog).Create(),
            _fixture.Build<Comment>().With(c => c.ApplicationUser, null as ApplicationUser)
                .With(c => c.Blog, null as Blog).Create(),
        };

        List<CommentDto> expectedComments = comments.Select(c => c.ToCommentDto()).ToList();
        _commentRepositoryMock.Setup(m => m.GetAllComments())
            .ReturnsAsync(comments);
        //Act
        List<CommentDto> actualComments = await _commentService.GetAllComments();
        //Assert
        actualComments.Should().BeEquivalentTo(expectedComments);
    }

    [Fact]
    public async Task CreateComment_InvalidBlogId_ReturnNull()
    {
        //Arrange
        _blogRepositoryMock.Setup(m => m.GetBlogById(It.IsAny<Guid>()))
            .ReturnsAsync(null as Blog);

        CreateCommentRequestDto commentRequest = _fixture.Create<CreateCommentRequestDto>();
        //Act
        CreateCommentResponseDto? commentResponse =
            await _commentService.CreateComment(
                           Guid.NewGuid(), Guid.NewGuid(), commentRequest);
        //Assert
        commentResponse.Should().BeNull();
    }

    [Fact]
    public async Task CreateComment_InvalidUserId_ReturnNull()
    {
        Blog blog = _fixture.Build<Blog>().With(b => b.Comments, new List<Comment>())
            .Create();
        _blogRepositoryMock.Setup(m => m.GetBlogById(It.IsAny<Guid>()))
            .ReturnsAsync(blog);
        _userRepositoryMock.Setup(m => m.GetUserById(It.IsAny<Guid>()))
            .ReturnsAsync(null as ApplicationUser);
        CreateCommentRequestDto commentRequest = _fixture.Create<CreateCommentRequestDto>();
        //Act
        CreateCommentResponseDto? commentResponse =
            await _commentService.CreateComment(
                Guid.NewGuid(), Guid.NewGuid(), commentRequest);
        //Assert
        commentResponse.Should().BeNull();

    }

    [Fact]
    public async Task CreateComment_ToBeSuccessful()
    {
        //Arrange
        Blog blog = _fixture.Build<Blog>().With(b => b.Comments, new List<Comment>())
            .Create();
        ApplicationUser user = _fixture.Build<ApplicationUser>()
            .With(u => u.Email, "someone@example.com")
            .With(u => u.PhoneNumber, "09328564178")
            .With(u => u.Comments, new List<Comment>())
            .Create();

        _blogRepositoryMock.Setup(m => m.GetBlogById(It.IsAny<Guid>()))
            .ReturnsAsync(blog);
        _userRepositoryMock.Setup(m => m.GetUserById(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        Comment comment = _fixture.Build<Comment>()
            .With(c => c.ApplicationUser, null as ApplicationUser)
            .With(c => c.Blog, null as Blog).Create();

        CreateCommentRequestDto commentRequestDto = new()
            { CommentTitle = comment.CommentTitle, CommentMessage = comment.CommentMessage };

        CreateCommentResponseDto commentResponseExpected = new()
        {
            Id = comment.Id, BlogId = comment.BlogId, CommentMessage = comment.CommentMessage,
            CommentTitle = comment.CommentTitle, UserName = comment.UserName, CreatedDate = comment.CreatedDate,
            IsSeen = comment.IsSeen
        };
        _commentRepositoryMock.Setup(m => m.AddComment(It.IsAny<Comment>()))
            .ReturnsAsync(comment);

        //Act
        CreateCommentResponseDto? commentResponseActual =
            await _commentService.CreateComment(user.Id, blog.Id, commentRequestDto);

        //Assert
        commentResponseActual.Should().BeEquivalentTo(commentResponseExpected);
    }

    [Fact]
    public async Task UpdateComment_InvalidCommentId_ReturnNull()
    {
        //Arrange
        UpdateCommentRequestDto commentRequestDto = _fixture.Create<UpdateCommentRequestDto>();
        _commentRepositoryMock.Setup(m => m.GetCommentById(It.IsAny<Guid>()))
            .ReturnsAsync(null as Comment);
        //Act
        UpdateCommentResponseDto? commentResponse =
            await _commentService.UpdateComment(Guid.NewGuid(), commentRequestDto);

        //Assert
        commentResponse.Should().BeNull();

    }

    [Fact]
    public async Task UpdateComment_ValidCommentId_ToBeSuccessful()
    {
        //Arrange
        Comment comment = _fixture.Build<Comment>()
            .With(c => c.ApplicationUser, null as ApplicationUser)
            .With(c => c.Blog, null as Blog).Create();
        _commentRepositoryMock.Setup(m => m.GetCommentById(It.IsAny<Guid>()))
            .ReturnsAsync(comment);

        UpdateCommentRequestDto commentRequestDto = new()
            { CommentTitle = comment.CommentTitle, CommentMessage = comment.CommentMessage };

        _commentRepositoryMock.Setup(m => m.UpdateComment(It.IsAny<Comment>()))
            .ReturnsAsync(comment);

        UpdateCommentResponseDto commentResponseExpected = new()
            { CommentTitle = comment.CommentTitle, CommentMessage = comment.CommentMessage };
        //Act
        UpdateCommentResponseDto? commentResponseActual =
            await _commentService.UpdateComment(Guid.NewGuid(), commentRequestDto);

        //Assert
        commentResponseActual.Should().BeEquivalentTo(commentResponseExpected);
    }

    [Fact]
    public async Task DeleteComment_InvalidCommentId_ReturnFalse()
    {
        //Arrange
        _commentRepositoryMock.Setup(m => m.DeleteComment(It.IsAny<Guid>()))
            .ReturnsAsync(false);
        //Act
        bool result = await _commentService.DeleteComment(Guid.NewGuid());
        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteComment_ValidCommentId_ReturnTrue()
    {
        //Arrange
        _commentRepositoryMock.Setup(m => m.DeleteComment(It.IsAny<Guid>()))
            .ReturnsAsync(true);
        //Act
        bool result = await _commentService.DeleteComment(Guid.NewGuid());
        //Assert
        result.Should().BeTrue();
    }
}

