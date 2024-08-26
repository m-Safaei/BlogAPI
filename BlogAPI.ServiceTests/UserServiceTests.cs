using AutoFixture;
using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.Domain.RepositoryInterfaces;
using BlogAPI.Core.DTO.User;
using BlogAPI.Core.ServiceInterfaces;
using BlogAPI.Core.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BlogAPI.ServiceTests;

public class UserServiceTests
{
    private readonly IFixture _fixture;

    private readonly IUserService _userService;

    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly IUserRepository _userRepository;
    public UserServiceTests()
    {
        _fixture = new Fixture();

        _userRepositoryMock = new Mock<IUserRepository>();
        _userRepository = _userRepositoryMock.Object;

        _userService = new UserService(_userRepository);
    }

    [Fact]
    public async Task AddUser_ReturnUser()
    {
        //Arrange
        RegisterDto registerDto = _fixture.Build<RegisterDto>()
            .With(u => u.Email, "someone@example.com")
            .With(u => u.PhoneNumber, "09328564178")
            .Create();

        _userRepositoryMock.Setup(m => m.AddUser(
                It.IsAny<ApplicationUser>(), It.IsAny<string>()))
               .ReturnsAsync(IdentityResult.Success);
        //Act
        object userResponse = await _userService.AddUser(registerDto);

        //Assert
        userResponse.Should().BeOfType(typeof(ApplicationUser));
    }

    [Fact]
    public async Task GetUserByPhoneNumber_WrongPhoneNumber_ReturnNull()
    {
        //Arrange
        _userRepositoryMock.Setup(m => m.GetUserByPhoneNumber(
            It.IsAny<string>())).ReturnsAsync(null as ApplicationUser);
        //Act
        LoginResponseDto? userResponse = await _userService.GetUserByPhoneNumber("096558741235");

        //Assert
        userResponse.Should().BeNull();
    }

    [Fact]
    public async Task GetUserByPhoneNumber_PhoneNumberExists_ReturnUser()
    {
        //Arrange
        ApplicationUser user = _fixture.Build<ApplicationUser>()
            .With(u => u.Email, "someone@example.com")
            .With(u => u.PhoneNumber, "09328564178")
            .With(u => u.Comments, null as List<Comment>)
            .Create();
        LoginResponseDto userExpected = new LoginResponseDto()
        {
            PersonName = user.FirstName + " " + user.LastName,
            PhoneNumber = user.PhoneNumber
        };

        _userRepositoryMock.Setup(m => m.GetUserByPhoneNumber(
                                  It.IsAny<string>())).ReturnsAsync(user);
        //Act
        LoginResponseDto? userResponse = await _userService.GetUserByPhoneNumber("09328564178");
        //Assert
        userResponse.Should().BeEquivalentTo(userExpected);
    }

    [Fact]
    public async Task IsPhoneNumberAlreadyRegistered_WrongPhoneNumber_ReturnFalse()
    {
        //Arrange 
        _userRepositoryMock.Setup(m => m.GetUserByPhoneNumber(
            It.IsAny<string>())).ReturnsAsync(null as ApplicationUser);
        //Act
        bool result = await _userService.IsPhoneNumberAlreadyRegistered("09328564178");
        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsPhoneNumberAlreadyRegistered_PhoneNumberExists_ReturnTrue()
    {
        //Arrange
        ApplicationUser user = _fixture.Build<ApplicationUser>()
            .With(u => u.Email, "someone@example.com")
            .With(u => u.PhoneNumber, "09328564178")
            .With(u => u.Comments, null as List<Comment>)
            .Create();
        _userRepositoryMock.Setup(m => m.GetUserByPhoneNumber(
            It.IsAny<string>())).ReturnsAsync(user);
        //Act
        bool result = await _userService.IsPhoneNumberAlreadyRegistered("09328564178");
        //Assert
        result.Should().BeTrue();
    }
}

