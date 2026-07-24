using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Dtos;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Converters
{
    public static class UserConverter
    {
        public static User ToEntity(this RegisterRequestDto dto, string hashedPassword)
        {
            var now = DateTime.UtcNow;

            return new User
            {
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = hashedPassword,
                EmailConfirmed = false,
                Role = UserRole.User,
                CreatedAt = now,
                UpdatedAt = now
            };
        }

        public static RegisterResponseDto ToResponseDto(this User user)
        {
            return new RegisterResponseDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
