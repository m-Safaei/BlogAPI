using BlogAPI.Core.DTO.User;

namespace BlogAPI.Core.ServiceInterfaces;

public interface IUserService
{
    Task<bool> IsPhoneNumberAlreadyRegistered(string phoneNumber);

    Task<object> AddUser(RegisterDto registerDto);

    Task<LoginResponseDto?> GetUserByPhoneNumber(string phoneNumber);
}

