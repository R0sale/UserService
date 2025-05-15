using Contracts;
using System.Linq.Expressions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace UserRepository
{
    public abstract class RepositoryBase : IRepositoryBase
    {
            private readonly UserRepositoryContext _context;

            public RepositoryBase(UserRepositoryContext context)
            {
                _context = context;
            }

            public IQueryable<User> FindAll(bool trackChanges)
            {
                return trackChanges ?
                    _context.Set<User>() :
                    _context.Set<User>().AsNoTracking();
            }

            public IQueryable<User> FindByCondition(Expression<Func<User, bool>> expression, bool trackChanges)
            {
                return !trackChanges ? _context.Set<User>()
                    .Where(expression)
                    .AsNoTracking() :
                    _context.Set<User>()
                    .Where(expression);
            }

            public void CreateUser(User product)
            {
                _context.Users.Add(product);
            }

            public void DeleteUser(User product)
            {
                _context.Users.Remove(product);
            }

            public void UpdateUser(User product)
            {
                _context.Users.Update(product);
            }

            public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
    
}
