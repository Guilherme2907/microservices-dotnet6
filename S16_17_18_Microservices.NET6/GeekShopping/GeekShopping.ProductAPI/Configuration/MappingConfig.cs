using AutoMapper;
using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Model;

namespace GeekShopping.ProductAPI.Configuration
{
    public static class MappingConfig
    {

        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();

            services.AddSingleton(mapper);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }

        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductVO, Product>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
