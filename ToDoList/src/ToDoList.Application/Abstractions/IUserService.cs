using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Dtos;

namespace ToDoList.Application.Abstractions
{
    public interface IUserService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
    }
}
