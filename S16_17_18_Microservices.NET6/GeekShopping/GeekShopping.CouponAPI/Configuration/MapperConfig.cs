using AutoMapper;
using GeekShopping.CouponAPI.Data.ValueObjects;
using GeekShopping.CouponAPI.Model;

namespace GeekShopping.CouponAPI.Configuration
{
    public static class MapperConfig
    {
        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            IMapper mapper = RegisterMaps().CreateMapper();

            services.AddSingleton(mapper);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }

        private static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponVO, Coupon>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
