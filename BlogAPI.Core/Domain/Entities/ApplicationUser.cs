using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Core.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    [StringLength(50, MinimumLength = 3)]
    public string FirstName { get; set; }

    [StringLength(50, MinimumLength = 3)]
    public string LastName { get; set; }

    public DateTime RegisterDate { get; set; } = DateTime.Now;

    public bool IsDeleted { get; set; }

    public ICollection<Comment> Comments { get; set; }
}

