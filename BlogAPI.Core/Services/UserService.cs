using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.Domain.RepositoryInterfaces;
using BlogAPI.Core.DTO.User;
using BlogAPI.Core.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;

namespace BlogAPI.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<object> AddUser(RegisterDto registerDto)
    {
        ApplicationUser user = new()
        {
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            UserName = registerDto.PhoneNumber,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
        };

        IdentityResult result = await _userRepository.AddUser(user, registerDto.Password);

        if (result.Succeeded)
        {
            return user;
        }

        return result.Errors;
    }

    public async Task<LoginResponseDto?> GetUserByPhoneNumber(string phoneNumber)
    {
        ApplicationUser? user = await _userRepository.GetUserByPhoneNumber(phoneNumber);
        if (user == null) return null;

        return new LoginResponseDto()
        {
            PersonName = user.FirstName + " " + user.LastName,
            PhoneNumber = user.PhoneNumber
        };
    }

    public async Task<bool> IsPhoneNumberAlreadyRegistered(string phoneNumber)
    {
        return await _userRepository.GetUserByPhoneNumber(phoneNumber) != null;
    }
}

