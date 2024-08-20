using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.Domain.RepositoryInterfaces;
using BlogAPI.Core.DTO;
using BlogAPI.Infrastructure.AppDbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;


    public UserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }


    public async Task<IdentityResult> AddUser(ApplicationUser user, string password)
    {
        IdentityResult result = await _userManager.CreateAsync(user, password);
        return result;

    }

    public async Task<ApplicationUser?> GetUserByPhoneNumber(string phoneNumber)
    {
        return await _userManager.Users
            .FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);
    }
    public async Task<ApplicationUser?> GetUserById(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UserExists(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteUserById(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<ApplicationUser> UpdateUser(ApplicationUser user)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ApplicationUser>> GetAllUsers()
    {
        throw new NotImplementedException();
    }

}

