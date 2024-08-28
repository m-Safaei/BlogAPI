using AutoFixture;
using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.DTO.Comment;
using BlogAPI.Core.ServiceInterfaces;
using BlogAPI.WebAPI.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BlogAPI.ControllerTests;

public class CommentsControllerTest
{
    private readonly IFixture _fixture;

    private readonly ICommentService _commentService;
    private readonly Mock<ICommentService> _commentServiceMock;
    public CommentsControllerTest()
    {
        _fixture = new Fixture();

        _commentServiceMock = new Mock<ICommentService>();
        _commentService = _commentServiceMock.Object;
    }

    [Fact]
    public async Task GetAllComments_ReturnOk()
    {
        //Arrange
        List<CommentDto> comments = _fixture.Create<List<CommentDto>>();
        _commentServiceMock.Setup(m => m.GetAllComments()).ReturnsAsync(comments);
        CommentsController controller = new CommentsController(_commentService);
        
        //Act
        IActionResult result = await controller.GetAllComments();

        //Assert
        result.Should().NotBeNull();
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        okResult.Value.Should().BeEquivalentTo(comments);
    }

    [Fact]
    public async Task GetComment_InvalidCommentId_ReturnNotFound()
    {
        //Arrange
        _commentServiceMock.Setup(m => m.GetCommentById(It.IsAny<Guid>()))
            .ReturnsAsync(null as CommentDto);
        CommentsController controller = new CommentsController(_commentService);

        //Act
        IActionResult result = await controller.GetComment(Guid.NewGuid());

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NotFoundResult));

    }

    [Fact]
    public async Task GetComment_ValidCommentId_ReturnOk()
    {
        //Arrange
        CommentDto comment = _fixture.Create<CommentDto>();
        _commentServiceMock.Setup(m => m.GetCommentById(It.IsAny<Guid>()))
            .ReturnsAsync(comment);
        CommentsController controller = new CommentsController(_commentService);

        //Act
        IActionResult result = await controller.GetComment(Guid.NewGuid());

        //Assert
        result.Should().NotBeNull();
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        okResult.Value.Should().BeEquivalentTo(comment);

    }

    [Fact]
    public async Task CreateComment_IfModelErrors_ReturnBadRequest()
    {
        //Arrange
        CreateCommentRequestDto commentRequestDto = _fixture.Create<CreateCommentRequestDto>();
        CommentsController controller = new CommentsController(_commentService);

        //Act
        controller.ModelState.AddModelError("CommentTitle", "Title can not be blank.");
        IActionResult result = await controller.CreateComment(Guid.NewGuid(), commentRequestDto);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BadRequestObjectResult));
    }

    [Fact]
    public async Task UpdateComment_IfModelErrors_ReturnBadRequest()
    {
        //Arrange
        UpdateCommentRequestDto commentRequestDto = _fixture.Create<UpdateCommentRequestDto>();
        CommentsController controller = new CommentsController(_commentService);

        //Act
        controller.ModelState.AddModelError("CommentTitle", "Title can not be blank.");
        IActionResult result = await controller.UpdateComment(Guid.NewGuid(), commentRequestDto);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BadRequestObjectResult));
    }

    [Fact]
    public async Task UpdateComment_InvalidCommentId_ReturnNotFound()
    {
        //Arrange
        UpdateCommentRequestDto commentRequestDto = _fixture.Create<UpdateCommentRequestDto>();
        _commentServiceMock.Setup(m => m.UpdateComment(It.IsAny<Guid>(), It.IsAny<UpdateCommentRequestDto>()))
            .ReturnsAsync(null as UpdateCommentResponseDto);
        CommentsController controller = new CommentsController(_commentService);

        //Act
        IActionResult result = await controller.UpdateComment(Guid.NewGuid(), commentRequestDto);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NotFoundResult));
    }

    [Fact]
    public async Task UpdateComment_ReturnOk()
    {
        //Arrange
        UpdateCommentRequestDto commentRequestDto = _fixture.Create<UpdateCommentRequestDto>();
        UpdateCommentResponseDto commentResponseDto = _fixture.Create<UpdateCommentResponseDto>();
        _commentServiceMock.Setup(m => m.UpdateComment(It.IsAny<Guid>(), It.IsAny<UpdateCommentRequestDto>()))
            .ReturnsAsync(commentResponseDto);
        CommentsController controller = new CommentsController(_commentService);

        //Act
        IActionResult result = await controller.UpdateComment(Guid.NewGuid(), commentRequestDto);

        //Assert
        result.Should().NotBeNull();
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        okResult.Value.Should().BeEquivalentTo(commentResponseDto);
    }

    [Fact]
    public async Task DeleteComment_InvalidCommentId_ReturnNotFound()
    {
        //Arrange
        _commentServiceMock.Setup(m => m.DeleteComment(It.IsAny<Guid>()))
            .ReturnsAsync(false);
        CommentsController controller = new CommentsController(_commentService);

        //Act
        IActionResult result = await controller.DeleteComment(Guid.NewGuid());

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteComment_ValidCommentId_ReturnNoContent()
    {
        //Arrange
        _commentServiceMock.Setup(m => m.DeleteComment(It.IsAny<Guid>()))
            .ReturnsAsync(true);
        CommentsController controller = new CommentsController(_commentService);

        //Act
        IActionResult result = await controller.DeleteComment(Guid.NewGuid());

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NoContentResult>();
    }
}

