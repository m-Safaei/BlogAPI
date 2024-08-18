using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Core.Domain.Entities;

public class Blog
{
    public Guid Id { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.Now;

    [StringLength(100,MinimumLength = 3)]
    public string Title { get; set; }

    [StringLength(2000,MinimumLength = 20)]
    public string Content { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<Comment> Comments { get; set; }
}

