using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Core.DTO;

public class UserAddRequestDto
{
    [StringLength(50, MinimumLength = 3)]
    public string FirstName { get; set; }

    [StringLength(50, MinimumLength = 3)]
    public string LastName { get; set; }

    [StringLength(11, MinimumLength = 11,ErrorMessage = "Phone number is not valid")]
    public string PhoneNumber { get; set; }

    [StringLength(60, MinimumLength = 4)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}

