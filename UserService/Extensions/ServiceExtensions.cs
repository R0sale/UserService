using UserRepository;
using Microsoft.EntityFrameworkCore;
using Contracts;
using Microsoft.AspNetCore.Identity;
using Entities.Models;

namespace UserService.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureRepository(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository.UserRepository>();
        }

        public static void ConfigureService(this IServiceCollection services)
        {
            services.AddScoped<IUserService, Service.UserService>();
        }
        
        public static void ConfigureSqlContext(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<UserRepositoryContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                    b => b.MigrationsAssembly("UserService"));
            });
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequireDigit = true;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireUppercase = true;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequiredLength = 8;
                opts.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<UserRepositoryContext>()
            .AddDefaultTokenProviders();
        }
    }
}
