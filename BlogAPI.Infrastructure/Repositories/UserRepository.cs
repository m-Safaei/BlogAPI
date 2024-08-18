using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.Domain.RepositoryInterfaces;
using BlogAPI.Infrastructure.AppDbContext;

namespace BlogAPI.Infrastructure.Repositories;

public class UserRepository:IUserRepository
{
    private readonly BlogDbContext _context;

    public UserRepository(BlogDbContext context)
    {
        _context = context;
    }


    public async Task<User> AddUser(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserById(Guid id)
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

    public async Task<User> UpdateUser(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<List<User>> GetAllUsers()
    {
        throw new NotImplementedException();
    }
}

