using BlogAPI.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Infrastructure.AppDbContext;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options):base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Blog> Blogs { get; set; }

    public DbSet<Comment> Comments { get; set; }
}

