using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Abstractions
{
    public interface IUserRepository
    {
        Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<User> AddAsync(User user, CancellationToken cancellationToken = default);
    }
}
