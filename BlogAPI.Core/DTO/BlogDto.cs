using BlogAPI.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Core.DTO;

public class BlogDto
{
    public Guid Id { get; set; }

    public DateTime CreateDate { get; set; } 

    public string Title { get; set; }

    public string Content { get; set; }
    

    //public ICollection<Comment> Comments { get; set; }
}

