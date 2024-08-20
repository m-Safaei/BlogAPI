using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Core.DTO;

public class RegisterDto
{
    [StringLength(50, MinimumLength = 3)] 
    public string FirstName { get; set; } = string.Empty;

    [StringLength(50, MinimumLength = 3)] 
    public string LastName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid Email address")]
    public string Email { get; set; } = string.Empty;

    [StringLength(11, MinimumLength = 11,ErrorMessage = "Phone number is not valid")]
    [RegularExpression("^[0-9]*$",ErrorMessage = "Phone number should contain only digits")]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(40,MinimumLength = 5)]
    public string Password { get; set; } = string.Empty;

    [Compare("Password",ErrorMessage = "Password and confirm password do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

