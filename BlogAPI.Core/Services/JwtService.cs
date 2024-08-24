using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogAPI.Core.Domain.Entities;
using BlogAPI.Core.Domain.RepositoryInterfaces;
using BlogAPI.Core.DTO.User;
using BlogAPI.Core.ServiceInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BlogAPI.Core.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public JwtService(IConfiguration configuration,IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }

    public async Task<AuthenticationResponseDto> CreateJwtToken(string phoneNumber)
    {
        ApplicationUser? user = await _userRepository.GetUserByPhoneNumber(phoneNumber);
        DateTime expiration = DateTime.Now.AddMinutes(
            Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));
        Claim[] claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), //Subject(user id)
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //JWT unique Id
            new Claim(JwtRegisteredClaimNames.Iat,
                DateTime.Now.ToString()), //Issued at (date and time of token generation)
            new Claim(ClaimTypes.NameIdentifier, user.PhoneNumber),//Unique name identifier of the user
            new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),//Name of the user
        };

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        SigningCredentials signingCredentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken tokenGenerator = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expiration,
            signingCredentials: signingCredentials
            );

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        string token = tokenHandler.WriteToken(tokenGenerator);

        return new AuthenticationResponseDto()
        {
            Token = token,
            ExpirationTime = expiration,
            PersonName = user.FirstName + " " + user.LastName,
            PhoneNumber = user.PhoneNumber
        };
    }
}

