using GeekShopping.CouponAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CouponAPI.Configuration
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
