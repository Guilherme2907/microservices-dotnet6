using GeekShopping.Email.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Configuration
{
    public static class SqlServerContextConfig
    {
        public static IServiceCollection ConfigureSqlServerContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SqlServerContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            return services;
        }
    }
}
