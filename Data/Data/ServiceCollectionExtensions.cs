using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Data.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEntityFrameworkWithSqlite(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(opts =>
                    opts.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IDbContext, DataContext>();
        }
    }
}
