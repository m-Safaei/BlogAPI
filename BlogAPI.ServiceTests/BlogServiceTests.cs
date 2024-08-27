using AutoFixture;
using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.Domain.RepositoryInterfaces;
using BlogAPI.Core.DTO.Blog;
using BlogAPI.Core.Mappers;
using BlogAPI.Core.ServiceInterfaces;
using BlogAPI.Core.Services;
using FluentAssertions;
using Moq;

namespace BlogAPI.ServiceTests;

public class BlogServiceTests
{
    private readonly IFixture _fixture;
    private readonly IBlogService _blogService;
    private readonly IBlogRepository _blogRepository;
    private readonly Mock<IBlogRepository> _blogRepositoryMock;

    public BlogServiceTests()
    {
        _fixture = new Fixture();

        _blogRepositoryMock = new Mock<IBlogRepository>();
        _blogRepository = _blogRepositoryMock.Object;

        _blogService = new BlogService(_blogRepository);
    }

    [Fact]
    public async Task GetListOfBlogs_ReturnEmptyList()
    {
        //Arrange
        _blogRepositoryMock.Setup(m => m.GetListOfBlogs())
            .ReturnsAsync(new List<Blog>());
        //Act 
        List<BlogDto> blogListResponse = await _blogService.GetListOfBlogs();
        //Assert
        blogListResponse.Should().BeEmpty();
    }

    [Fact]
    public async Task GetListOfBlogs_WithFewBlogs_ToBeSuccessful()
    {
        //Arrange
        List<Blog> blogs = new List<Blog>()
        {
            _fixture.Build<Blog>().With(b=>b.Comments,new List<Comment>()).Create(),
            _fixture.Build<Blog>().With(b=>b.Comments,new List<Comment>()).Create(),
            _fixture.Build<Blog>().With(b=>b.Comments,new List<Comment>()).Create(),
        };

        List<BlogDto> expectedBlogs = blogs.Select(b => b.ToBlogDto()).ToList();

        _blogRepositoryMock.Setup(m=>m.GetListOfBlogs())
            .ReturnsAsync(blogs);
        //Act
        List<BlogDto> blogListResponse = await _blogService.GetListOfBlogs();

        //Assert
        blogListResponse.Should().BeEquivalentTo(expectedBlogs);
    }

    [Fact]
    public async Task GetBlogById_InvalidId_ReturnNull()
    {
        //Arrange
        _blogRepositoryMock.Setup(m => m.GetBlogById(It.IsAny<Guid>()))
            .ReturnsAsync(null as Blog);
        //Act
        BlogDto? blogResponse = await _blogService.GetBlogById(Guid.NewGuid());
        //Assert
        blogResponse.Should().BeNull();
    }

    [Fact]
    public async Task GetBlogById_ValidId_ToBeSuccessful()
    {
        //Arrange
        Blog blog = _fixture.Build<Blog>().With(b=>b.Comments,new List<Comment>())
                    .Create();
        BlogDto expectedBlog = blog.ToBlogDto();
        _blogRepositoryMock.Setup(m => m.GetBlogById(It.IsAny<Guid>()))
            .ReturnsAsync(blog);
        //Act
        BlogDto? blogResponse = await _blogService.GetBlogById(blog.Id);
        blogResponse.Should().BeEquivalentTo(expectedBlog);
    }

    [Fact]
    public async Task CreateBlog_ToBeSuccessful()
    {
        //Arrange
        CreateBlogRequestDto blogRequest = _fixture.Create<CreateBlogRequestDto>();
        Blog blog = blogRequest.ToBlog();
        CreateBlogResponseDto expectedBlog = new CreateBlogResponseDto()
        {
            Id = blog.Id,
            Title = blog.Title,
            Content = blog.Content
        };
        _blogRepositoryMock.Setup(m=>m.AddBlog(It.IsAny<Blog>()))
            .ReturnsAsync(blog);
        //Act
        CreateBlogResponseDto blogResponse = await _blogService.CreateBlog(blogRequest);

        //Assert
        blogResponse.Should().BeEquivalentTo(expectedBlog);
    }

    [Fact]
    public async Task UpdateBlog_InvalidBlogId_ReturnNull()
    {
        //Arrange
        UpdateBlogRequestDto blogRequest = _fixture.Create<UpdateBlogRequestDto>();
        _blogRepositoryMock.Setup(m => m.GetBlogById(It.IsAny<Guid>()))
            .ReturnsAsync(null as Blog);
        //Act 
        UpdateBlogResponseDto? blogResponse = await _blogService.UpdateBlog(Guid.NewGuid(), blogRequest);
        
        //Assert
        blogResponse.Should().BeNull();
    }

    [Fact]
    public async Task UpdateBlog_ValidBlogId_ToBeSuccessful()
    {
        //Arrange
        Blog blog = _fixture.Build<Blog>().With(b => b.Comments, new List<Comment>())
            .Create();
        UpdateBlogResponseDto blogResponseExpected = new() { Title = blog.Title, Content = blog.Content };
        UpdateBlogRequestDto updateBlogRequest = new() { Title = blog.Title, Content = blog.Content };

        _blogRepositoryMock.Setup(m => m.GetBlogById(blog.Id))
            .ReturnsAsync(blog);
        _blogRepositoryMock.Setup(m => m.UpdateBlog(It.IsAny<Blog>()))
            .ReturnsAsync(blog);
        //Act
        UpdateBlogResponseDto? blogResponseActual = await _blogService.UpdateBlog(blog.Id, updateBlogRequest);
        //Assert
        blogResponseActual.Should().BeEquivalentTo(blogResponseExpected);
    }

    [Fact]
    public async Task DeleteBlog_InvalidBlogId_ReturnFalse()
    {
        //Arrange
        _blogRepositoryMock.Setup(m => m.DeleteBlog(It.IsAny<Guid>()))
            .ReturnsAsync(false);
        //Act
        bool result = await _blogService.DeleteBlog(Guid.NewGuid());
        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteBlog_ValidBlogId_ReturnTrue()
    {
        //Arrange
        _blogRepositoryMock.Setup(m => m.DeleteBlog(It.IsAny<Guid>()))
            .ReturnsAsync(true);
        //Act
        bool result = await _blogService.DeleteBlog(Guid.NewGuid());
        //Assert
        result.Should().BeTrue();
    }
}

