using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Core.Domain.Entities;

public class Comment
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(10, MinimumLength = 3)]
    public string CommentTitle { get; set; }

    [StringLength(200, MinimumLength = 5)]
    public string CommentMessage { get; set; }

    [StringLength(100, MinimumLength = 3)] 
    public string UserName { get; set; } = string.Empty;

    public bool IsSeen { get; set; }

    public Guid ApplicationUserId { get; set; }

    public Guid BlogId { get; set; }

    //Navigation Properties:
    public ApplicationUser ApplicationUser { get; set; }

    public Blog Blog { get; set; }

}

