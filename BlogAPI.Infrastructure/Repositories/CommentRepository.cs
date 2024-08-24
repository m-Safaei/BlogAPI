using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.Domain.RepositoryInterfaces;
using BlogAPI.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly BlogDbContext _context;

    public CommentRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<Comment> AddComment(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment?> GetCommentById(Guid id)
    {
        return await _context.Comments.FindAsync(id);
    }

    public async Task<List<Comment>> GetAllComments()
    {
        return await _context.Comments.ToListAsync();
    }

    public async Task<Comment> UpdateComment(Comment comment)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<bool> DeleteComment(Guid id)
    {
        Comment? comment = await GetCommentById(id);
        if (comment == null) return false;

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CommentExists(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Comment>> GetCommentsByUserId(Guid userId)
    {
        return await _context.Comments.Where(c=>c.ApplicationUserId==userId)
                                      .ToListAsync();
    }

    public async Task<List<Comment>> GetCommentsByBlogId(Guid blogId)
    {
        return await _context.Comments.Where(c=>c.BlogId==blogId)
                                      .ToListAsync();
    }
}

