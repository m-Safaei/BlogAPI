using BlogAPI.Core.Domain.Entities;

namespace BlogAPI.Core.Domain.RepositoryInterfaces;

public interface IUserRepository
{
    Task<User> AddUser(User user);

    Task<User?> GetUserById(Guid id);

    Task<bool> UserExists(Guid id);

    Task<bool> DeleteUserById(Guid id);

    Task<User> UpdateUser(User user);

    Task<List<User>> GetAllUsers();


}

