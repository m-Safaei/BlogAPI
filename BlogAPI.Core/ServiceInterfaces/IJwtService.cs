using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.DTO;

namespace BlogAPI.Core.ServiceInterfaces;

public interface IJwtService
{
    Task<AuthenticationResponseDto> CreateJwtToken(string phoneNumber);
}

