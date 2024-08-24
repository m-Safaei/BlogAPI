using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Core.DTO.User;

public class LoginDto
{
    [Required]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number is not valid")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number should contain only digits")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(40, MinimumLength = 5)]
    public string Password { get; set; } = string.Empty;
}

