using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Abstractions;
using ToDoList.Application.Converters;
using ToDoList.Application.Dtos;
using ToDoList.Application.Exceptions;
using ToDoList.Application.Settings;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Services;

public class AuthService : IAuthService
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<RefreshToken> _refreshTokenRepository;
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly ITokenService _tokenService;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IBaseRepository<User> userRepository, 
        IPasswordHasherService passwordHasherService, 
        ITokenService tokenService, 
        IBaseRepository<RefreshToken> refreshTokenRepository, 
        JwtSettings jwtSettings)
    {
        _userRepository = userRepository;
        _passwordHasherService = passwordHasherService;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtSettings = jwtSettings;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        var users = _userRepository.GetAllQuery();

        var user =  await users.FirstOrDefaultAsync(u =>
                    u.UserName == loginDto.UserNameOrEmail
                    || u.Email == loginDto.UserNameOrEmail);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid username or email.");
        }

        var isPasswordValid = _passwordHasherService.Verify(loginDto.Password, user.Password, user.Salt);

        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Invalid password.");
        }

        var loginResponseDto = await GenerateLoginResponseAsync(user);

        return loginResponseDto;
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        var storedToken = await _refreshTokenRepository.GetAllQuery()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == refreshTokenRequestDto.RefreshToken);

        if (storedToken == null || !storedToken.IsActive)
        {
            throw new UnauthorizedException("Invalid or expired refresh token.");
        }

        var loginResponseDto = await GenerateLoginResponseAsync(storedToken.User);

        // Rotate: revoke the old refresh token and link it to the newly issued one.
        storedToken.RevokedAt = DateTime.Now;
        storedToken.ReplacedByToken = loginResponseDto.RefreshToken;
        _refreshTokenRepository.Update(storedToken);
        await _refreshTokenRepository.SaveChangesAsync();

        return loginResponseDto;
    }

    public async Task LogoutAsync(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        var storedToken = await _refreshTokenRepository.GetAllQuery()
            .FirstOrDefaultAsync(x => x.Token == refreshTokenRequestDto.RefreshToken);

        if (storedToken == null)
        {
            throw new NotFoundException("Refresh token not found.");
        }

        if (storedToken.IsActive)
        {
            storedToken.RevokedAt = DateTime.Now;
            _refreshTokenRepository.Update(storedToken);
            await _refreshTokenRepository.SaveChangesAsync();
        }
    }

    private async Task<LoginResponseDto> GenerateLoginResponseAsync(User user)
    {
        var userGetDto = new UserGetDto()
        {
            UserId = user.UserId,
            UserName = user.UserName,
            Email = user.Email,
            Role = user.Role,
            FirstName = user.FirstName,
            LastName = user.LastName,
            EmailConfirmed = user.EmailConfirmed,
            CreatedAt = user.CreatedAt
        };

        var accessToken = _tokenService.GetToken(userGetDto);
        var refreshTokenValue = _tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken()
        {
            Token = refreshTokenValue,
            UserId = user.UserId,
            CreatedAt = DateTime.Now,
            ExpiresAt = DateTime.Now.AddDays(_jwtSettings.RefreshTokenLifetimeDays),
        };

        await _refreshTokenRepository.AddAsync(refreshToken);
        await _refreshTokenRepository.SaveChangesAsync();

        return new LoginResponseDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue,
            TokenType = "Bearer",
            Expires = _jwtSettings.Lifetime,
        };
    }

    public async Task<long> RegisterAsync(RegisterDto registerDto)
    {
        var user = await _userRepository.GetAllQuery()
            .FirstOrDefaultAsync(u => u.UserName == registerDto.UserName || u.Email == registerDto.Email);

        if (user != null)
        {
            throw new Exception("User with the same username or email already exists.");
        }

        var hashedPassword = _passwordHasherService.Hasher(registerDto.Password);
        registerDto.Password = hashedPassword.Item1;

        var newUser = registerDto.ToEntity();
        newUser.CreatedAt = DateTime.UtcNow;
        newUser.UpdatedAt = DateTime.UtcNow;
        newUser.Salt = hashedPassword.Item2;
        newUser.Role = UserRole.User;
        newUser.EmailConfirmed = false;

        await _userRepository.AddAsync(newUser);
        await _userRepository.SaveChangesAsync();

        return newUser.UserId;
    }
}
