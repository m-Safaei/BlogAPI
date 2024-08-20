using BlogAPI.Core.Domain.Entities;

namespace BlogAPI.Core.Domain.RepositoryInterfaces;

public interface IUserRepository
{
    Task<ApplicationUser> AddUser(ApplicationUser user);

    Task<ApplicationUser?> GetUserById(Guid id);

    Task<bool> UserExists(Guid id);

    Task<bool> DeleteUserById(Guid id);

    Task<ApplicationUser> UpdateUser(ApplicationUser user);

    Task<List<ApplicationUser>> GetAllUsers();


}

