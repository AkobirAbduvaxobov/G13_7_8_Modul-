using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Abstractions;
using ToDoList.Application.Dtos;

namespace ToDoList.Infrastructure.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings JwtSettings;

        public TokenService(JwtSettings jwtSettings)
        {
            JwtSettings = jwtSettings;
        }

        public string GetToken(UserGetDto userGetDto)
        {
            var IdentityClaims = new Claim[]
            {
            new Claim("UserId",userGetDto.UserId.ToString()),
            new Claim("FirstName",userGetDto.FirstName.ToString()),
            new Claim("LastName",userGetDto.LastName.ToString()),
            new Claim("UserName",userGetDto.UserName.ToString()),
            new Claim(ClaimTypes.Role,userGetDto.Role.ToString()),
            new Claim(ClaimTypes.Email,userGetDto.Email.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.SecretKey));
            var keyCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiresMinutes = JwtSettings.Lifetime;
            var token = new JwtSecurityToken(
                issuer: JwtSettings.Issuer,
                audience: JwtSettings.Audience,
                claims: IdentityClaims,
                expires: DateTime.Now.AddMinutes(expiresMinutes),
                signingCredentials: keyCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
