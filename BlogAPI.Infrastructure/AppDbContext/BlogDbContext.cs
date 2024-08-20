using BlogAPI.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Infrastructure.AppDbContext;

public class BlogDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {

    }


    public DbSet<Blog> Blogs { get; set; }

    public DbSet<Comment> Comments { get; set; }
}

