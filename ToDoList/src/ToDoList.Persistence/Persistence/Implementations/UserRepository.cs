using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Abstractions;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Persistence.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
            => await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
            => await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        public async Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default)
            => await _context.Users.AnyAsync(u => u.UserName == userName, cancellationToken);

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
            => await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);

        public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return user;
        }
    }
}
