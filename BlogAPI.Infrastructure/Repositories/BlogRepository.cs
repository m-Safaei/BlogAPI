using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.Domain.RepositoryInterfaces;
using BlogAPI.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Infrastructure.Repositories;

public class BlogRepository : IBlogRepository
{
    private readonly BlogDbContext _context;
    private readonly ICommentRepository _commentRepository;

    public BlogRepository(BlogDbContext context,ICommentRepository commentRepository)
    {
        _context = context;
        _commentRepository = commentRepository;
    }

    public async Task<Blog> AddBlog(Blog blog)
    {
        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();
        return blog;
    }

    public async Task<Blog?> GetBlogById(Guid id)
    {
        Blog? blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
        if (blog != null)
        {
            blog.Comments = await _commentRepository.GetCommentsByBlogId(blog.Id);
        }
        return blog;
    }

    public async Task<List<Blog>> GetListOfBlogs()
    {
        List<Blog> blogs = await _context.Blogs.Where(b => !b.IsDeleted).ToListAsync();
        foreach (Blog blog in blogs)
        {
            blog.Comments = await _commentRepository.GetCommentsByBlogId(blog.Id);
        }

        return blogs;
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

