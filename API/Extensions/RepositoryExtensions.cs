using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Redis;

namespace API.Extensions
{
    public static class RepositoryExtensions
    {
        public static void AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(x =>
            {
                x.UseSqlServer(configuration.GetConnectionString("SqlServer"), x => { x.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.GetName().Name); });
            });

            services.AddSingleton<RedisService>(x =>
            {
                return new RedisService(configuration.GetConnectionString("Redis")!);
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }
    }
}