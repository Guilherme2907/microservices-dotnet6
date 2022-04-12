using AutoMapper;
using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Model;
using GeekShopping.CartAPI.ValueObjects;

namespace GeekShopping.CartAPI.Configuration
{
    public static class MappingConfig
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
                config.CreateMap<ProductVO, Product>().ReverseMap();
                config.CreateMap<CartHeaderVO, CartHeader>().ReverseMap();
                config.CreateMap<CartDetailVO, CartDetail>().ReverseMap();
                config.CreateMap<CartVO, Cart>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
