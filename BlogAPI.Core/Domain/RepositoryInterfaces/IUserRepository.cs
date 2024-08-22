using BlogAPI.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogAPI.Core.Domain.RepositoryInterfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetUserByPhoneNumber(string phoneNumber);
    Task<IdentityResult> AddUser(ApplicationUser user,string password);

}

