using UserRepository;
using Microsoft.EntityFrameworkCore;
using Contracts;

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
    }
}
