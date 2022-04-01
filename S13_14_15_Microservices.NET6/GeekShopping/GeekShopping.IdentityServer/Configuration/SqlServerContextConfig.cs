using GeekShopping.IdentityServer.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.IdentityServer.Configuration
{
    public static class SqlServerContextConfig
    {
        public static IServiceCollection ConfigureSqlServerContext(this IServiceCollection services, 
                                                                        IConfiguration configuration)
        {
            services.AddDbContext<SqlServerContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
