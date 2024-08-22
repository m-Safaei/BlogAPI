using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.Domain.RepositoryInterfaces;
using BlogAPI.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Infrastructure.Repositories;

public class BlogRepository : IBlogRepository
{
    private readonly BlogDbContext _context;

    public BlogRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<Blog> AddBlog(Blog blog)
    {
        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();
        return blog;
    }

    public async Task<Blog?> GetBlogById(Guid id)
    {
        return await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
    }

    public async Task<List<Blog>> GetListOfBlogs()
    {
        return await _context.Blogs.Where(b => !b.IsDeleted).ToListAsync();
    }

    public async Task<Blog> UpdateBlog(Blog blog)
    {
        _context.Blogs.Update(blog);
        await _context.SaveChangesAsync();
        return blog;
    }

    public async Task<bool> BlogExists(Guid id)
    {
        return await _context.Blogs.AnyAsync(b => b.Id == id && !b.IsDeleted);
    }

    public async Task<bool> DeleteBlog(Guid id)
    {
        Blog? blog = await GetBlogById(id);

        if (blog == null) return false;

        blog.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }
}

