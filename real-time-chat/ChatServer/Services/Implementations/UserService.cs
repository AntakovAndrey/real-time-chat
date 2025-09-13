using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ChatServer.Configurations;
using ChatServer.Dto;
using ChatServer.Models;
using ChatServer.Repositories.Interfaces;
using ChatServer.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ChatServer.Services.Implementations;

// ToDo: add password hash
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly AuthConfiguration _authConfiguration;
    
    public UserService(IUserRepository userRepository, IOptions<AuthConfiguration> authConfiguration)
    {
        _userRepository = userRepository;
        _authConfiguration = authConfiguration.Value;
    }
    
    public async Task AddUserAsync(RegisterDto user, CancellationToken cancellationToken)
    {
        var foundUser = (await _userRepository.GetAllAsync(cancellationToken: cancellationToken))
            .FirstOrDefault(x=>x.Name==user.Username || x.Email == user.Email);
        if (foundUser != null)
        {
            throw new InvalidOperationException("User already exists");
        }
        var addingUser = new User
        {
            Name = user.Username,
            Surname = user.Surname,
            Username = user.Username,
            Email = user.Email,
            Password = user.Password
        };
        await _userRepository.CreateAsync(addingUser, cancellationToken);
    }

    public string GenerateToken(UserDto user)
    {
        var key = System.Text.Encoding.ASCII.GetBytes(_authConfiguration.Secret);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()), 
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.Username)
            }),
            Issuer = _authConfiguration.Issuer,
            Audience = _authConfiguration.Audience,
            Expires = DateTime.UtcNow.Add(_authConfiguration.Expiration),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<UserDto> AuthenticateAsync(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var foundUser = (await _userRepository.GetAllAsync(cancellationToken: cancellationToken))
            .FirstOrDefault(x=>x.Name==loginDto.Username && x.Password == loginDto.Password);
        if (foundUser == null)
        {
            throw new InvalidDataException("Invalid login or password");
        }
        return new UserDto
        {
            Id = foundUser.Id,
            Email = foundUser.Email,
            Username = foundUser.Name,
        };
    }
}
