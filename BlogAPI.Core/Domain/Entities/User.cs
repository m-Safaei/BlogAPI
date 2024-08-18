using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Core.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    [StringLength(50,MinimumLength = 3)]
    public string FirstName { get; set; }

    [StringLength(50,MinimumLength = 3)]
    public string LastName { get; set; }

    public DateTime CreateDate { get; set; }=DateTime.Now;

    [StringLength(11,MinimumLength = 11)]
    public string PhoneNumber { get; set; }

    [StringLength(60,MinimumLength = 4)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<Comment> Comments { get; set; }
}

