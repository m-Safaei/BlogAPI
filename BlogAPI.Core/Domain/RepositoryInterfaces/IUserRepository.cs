using BlogAPI.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogAPI.Core.Domain.RepositoryInterfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetUserByPhoneNumber(string phoneNumber);
    Task<IdentityResult> AddUser(ApplicationUser user,string password);

    Task<ApplicationUser?> GetUserById(Guid id);

    Task<bool> UserExists(Guid id);

    Task<bool> DeleteUserById(Guid id);

    Task<ApplicationUser> UpdateUser(ApplicationUser user);

    Task<List<ApplicationUser>> GetAllUsers();


}

