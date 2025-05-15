using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRepository
{
    public class UserRepositoryContext : DbContext
    {
        public UserRepositoryContext(DbContextOptions opts) : base(opts)
        { }

        public DbSet<User> Users { get; set; }
    }
}
