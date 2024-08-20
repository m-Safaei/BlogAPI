using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.Domain.RepositoryInterfaces;
using BlogAPI.Infrastructure.AppDbContext;

namespace BlogAPI.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BlogDbContext _context;

    public UserRepository(BlogDbContext context)
    {
        _context = context;
    }


    public async Task<ApplicationUser> AddUser(ApplicationUser user)
    {
        throw new NotImplementedException();
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

