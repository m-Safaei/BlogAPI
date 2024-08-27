using AutoFixture;
using BlogAPI.Core.DTO.Blog;
using BlogAPI.Core.DTO.Comment;
using BlogAPI.Core.ServiceInterfaces;
using BlogAPI.WebAPI.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BlogAPI.ControllerTests;
public class BlogsControllerTest
{
    private readonly IFixture _fixture;

    private readonly IBlogService _blogService;
    private readonly Mock<IBlogService> _blogServiceMock;
    public BlogsControllerTest()
    {
        _fixture = new Fixture();

        _blogServiceMock = new Mock<IBlogService>();
        _blogService = _blogServiceMock.Object;
    }

    [Fact]
    public async Task GetListOfBlogs_ReturnOk()
    {
        //Arrange
        List<BlogDto> blogs = new()
        {
            _fixture.Build<BlogDto>().With(b=>b.Comments,new List<CommentDto>()).Create(),
            _fixture.Build<BlogDto>().With(b=>b.Comments,new List<CommentDto>()).Create(),
            _fixture.Build<BlogDto>().With(b=>b.Comments,new List<CommentDto>()).Create(),
        };
        _blogServiceMock.Setup(m => m.GetListOfBlogs()).ReturnsAsync(blogs);

        BlogsController controller = new BlogsController(_blogService);

        //Act
        IActionResult result = await controller.GetListOfBlogs();

        //Assert
        result.Should().NotBeNull();
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        okResult.Value.Should().BeEquivalentTo(blogs);

    }

    [Fact]
    public async Task GetBlog_InvalidBlogId_ReturnNotFound()
    {
        //Assert
        _blogServiceMock.Setup(m => m.GetBlogById(It.IsAny<Guid>()))
            .ReturnsAsync(null as BlogDto);
        BlogsController controller = new BlogsController(_blogService);

        //Act
        IActionResult result = await controller.GetBlog(Guid.NewGuid());

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NotFoundResult));
    }

    [Fact]
    public async Task GetBlog_ValidBlogId_ReturnOk()
    {
        //Assert
        BlogDto blog = _fixture.Build<BlogDto>().With(b => b.Comments, new List<CommentDto>())
            .Create();
        _blogServiceMock.Setup(m => m.GetBlogById(It.IsAny<Guid>()))
            .ReturnsAsync(blog);
        BlogsController controller = new BlogsController(_blogService);

        //Act
        IActionResult result = await controller.GetBlog(Guid.NewGuid());

        //Assert
        result.Should().NotBeNull();
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        okResult.Value.Should().BeEquivalentTo(blog);
    }

    [Fact]
    public async Task CreateBlog_IfModelErrors_ReturnBadRequest()
    {
        //Arrange
        CreateBlogRequestDto blogRequest = _fixture.Create<CreateBlogRequestDto>();
        BlogsController controller = new BlogsController(_blogService);

        //Act
        controller.ModelState.AddModelError("Title", "Title can not be blank.");
        IActionResult result = await controller.CreateBlog(blogRequest);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BadRequestObjectResult));
    }

    [Fact]
    public async Task CreateBlog_NoModelErrors_ReturnCreatedAtAction()
    {
        //Arrange
        CreateBlogRequestDto blogRequestDto = _fixture.Create<CreateBlogRequestDto>();
        CreateBlogResponseDto blogResponseDto = _fixture.Create<CreateBlogResponseDto>();
        _blogServiceMock.Setup(m => m.CreateBlog(It.IsAny<CreateBlogRequestDto>()))
            .ReturnsAsync(blogResponseDto);
        BlogsController controller = new BlogsController(_blogService);

        //Act
        IActionResult result = await controller.CreateBlog(blogRequestDto);

        //Assert
        result.Should().NotBeNull();
        CreatedAtActionResult actionResult = Assert.IsType<CreatedAtActionResult>(result);
        actionResult.Value.Should().BeEquivalentTo(blogResponseDto);
    }

    [Fact]
    public async Task UpdateBlog_IfModelErrors_ReturnBadRequest()
    {
        //Arrange
        UpdateBlogRequestDto blogRequestDto = _fixture.Create<UpdateBlogRequestDto>();
        BlogsController controller = new BlogsController(_blogService);

        //Act
        controller.ModelState.AddModelError("Title", "Title can not be blank.");
        IActionResult result = await controller.UpdateBlog(Guid.NewGuid(), blogRequestDto);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(BadRequestObjectResult));
    }

    [Fact]
    public async Task UpdateBlog_InvalidBlogId_ReturnNotFound()
    {
        //Arrange
        UpdateBlogRequestDto blogRequestDto = _fixture.Create<UpdateBlogRequestDto>();
        _blogServiceMock.Setup(m => m.UpdateBlog(It.IsAny<Guid>(),It.IsAny<UpdateBlogRequestDto>()))
            .ReturnsAsync(null as UpdateBlogResponseDto);
        BlogsController controller = new BlogsController(_blogService);

        //Act
        IActionResult result = await controller.UpdateBlog(Guid.NewGuid(), blogRequestDto);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NotFoundResult));
    }

    [Fact]
    public async Task UpdateBlog_ReturnOk()
    {
        //Arrange
        UpdateBlogRequestDto blogRequestDto = _fixture.Create<UpdateBlogRequestDto>();
        UpdateBlogResponseDto blogResponseDto = _fixture.Create<UpdateBlogResponseDto>();
        _blogServiceMock.Setup(m => m.UpdateBlog(It.IsAny<Guid>(), It.IsAny<UpdateBlogRequestDto>()))
            .ReturnsAsync(blogResponseDto);
        BlogsController controller = new BlogsController(_blogService);

        //Act
        IActionResult result = await controller.UpdateBlog(Guid.NewGuid(), blogRequestDto);
        
        //Assert
        result.Should().NotBeNull();
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        okResult.Value.Should().BeEquivalentTo(blogResponseDto);
    }

    [Fact]
    public async Task DeleteBlog_InvalidBlogId_ReturnNotFound()
    {
        //Arrange
        _blogServiceMock.Setup(m => m.DeleteBlog(It.IsAny<Guid>()))
            .ReturnsAsync(false);
        BlogsController controller = new BlogsController(_blogService);

        //Act
        IActionResult result =await controller.DeleteBlog(Guid.NewGuid());

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NotFoundResult));
    }

    [Fact]
    public async Task DeleteBlog_ValidBlogId_ReturnNoContent()
    {
        //Arrange
        _blogServiceMock.Setup(m => m.DeleteBlog(It.IsAny<Guid>()))
            .ReturnsAsync(true);
        BlogsController controller = new BlogsController(_blogService);

        //Act
        IActionResult result = await controller.DeleteBlog(Guid.NewGuid());

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(NoContentResult));
    }
}

