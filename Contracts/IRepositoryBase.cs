using System.Linq.Expressions;
using Entities.Models;

namespace Contracts
{
    public interface IRepositoryBase
    {
        IQueryable<User> FindAll(bool trackChanges);
        IQueryable<User> FindByCondition(Expression<Func<User, bool>> expression, bool trackChanges);
        void CreateUser(User user);
        void DeleteUser(User user);
        void UpdateUser(User user);
        Task SaveAsync();
    }
}
