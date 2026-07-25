using ToDoList.Application.Abstractions;
using ToDoList.Application.Converters;
using ToDoList.Application.Dtos;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Services;

public class AuthService : IAuthService
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IPasswordHasherService _passwordHasherService;

    public AuthService(IBaseRepository<User> userRepository, IPasswordHasherService passwordHasherService)
    {
        _userRepository = userRepository;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        var users = _userRepository.GetAllQuery();

        var user = await users
                    .Include(u => u.Password)
                    .FirstOrDefaultAsync(u =>
                    u.UserName == loginDto.UserNameOrEmail
                    || u.Email == loginDto.UserNameOrEmail);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid username or email.");
        }

        var isPasswordValid = PasswordHasher.Verify(loginDto.Password, user.Password.PasswordHash, user.Password.Salt);

        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Invalid password.");
        }

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

        var token = TokenService.GetToken(userGetDto);

        var loginResponseDto = new LoginResponseDto()
        {
            AccessToken = token,
            RefreshToken = null,
            TokenType = "Bearer",
            Expires = 5,
        };


        return loginResponseDto;
    }

    public async Task<long> RegisterAsync(RegisterDto registerDto)
    {
        var user = _userRepository.GetAllQuery()
            .FirstOrDefault(u => u.UserName == registerDto.UserName || u.Email == registerDto.Email);

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
