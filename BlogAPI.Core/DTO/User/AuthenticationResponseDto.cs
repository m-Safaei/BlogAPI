namespace BlogAPI.Core.DTO.User;

public class AuthenticationResponseDto
{
    public string? PersonName { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; } = string.Empty;

    public string? Token { get; set; } = string.Empty;

    public DateTime ExpirationTime { get; set; }
}

