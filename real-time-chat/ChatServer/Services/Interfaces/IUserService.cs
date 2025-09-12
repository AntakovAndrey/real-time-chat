using ChatServer.Dto;

namespace ChatServer.Services.Interfaces;

public interface IUserService
{
    public Task AddUserAsync(RegisterDto user, CancellationToken cancellationToken);
    //public Task<GetUserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    public string GenerateToken(UserDto user);
    public Task<UserDto> AuthenticateAsync(LoginDto user, CancellationToken cancellationToken);    
}
