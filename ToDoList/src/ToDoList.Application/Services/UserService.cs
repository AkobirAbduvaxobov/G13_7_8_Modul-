using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Abstractions;
using ToDoList.Application.Converters;
using ToDoList.Application.Dtos;
using ToDoList.Application.Exceptions;
using ToDoList.Application.Validators;

namespace ToDoList.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly RegisterRequestValidator _validator;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _validator = new RegisterRequestValidator();
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new AppValidationException(errors);
            }

            if (await _userRepository.ExistsByUserNameAsync(request.UserName, cancellationToken))
                throw new ConflictException($"UserName '{request.UserName}' is already taken.");

            if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
                throw new ConflictException($"Email '{request.Email}' is already registered.");

            var hashedPassword = _passwordHasher.HashPassword(request.Password);
            var user = request.ToEntity(hashedPassword);

            var createdUser = await _userRepository.AddAsync(user, cancellationToken);

            return createdUser.ToResponseDto();
        }
    }
}
