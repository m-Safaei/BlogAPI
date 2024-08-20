using BlogAPI.Core.Domain.RepositoryInterfaces;
using BlogAPI.Core.ServiceInterfaces;

namespace BlogAPI.Core.Services;

public class UserService:IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> IsPhoneNumberAlreadyRegistered(string phoneNumber)
    {
        return await _userRepository.GetUserByPhoneNumber(phoneNumber) != null;
    }
}

