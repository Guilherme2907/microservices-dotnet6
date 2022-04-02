using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        public const string BASE_PATH = "api/v1/product";

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<ProductViewModel>> FindAllProducts(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(BASE_PATH);

            return await response.ReadContentAs<List<ProductViewModel>>();
        }
        public async Task<ProductViewModel> FindProductById(long id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{BASE_PATH}/{id}");

            return await response.ReadContentAs<ProductViewModel>();
        }

        public async Task<ProductViewModel> CreateProduct(ProductViewModel product, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJson(BASE_PATH, product);

            return await response.ReadContentAs<ProductViewModel>();
        }

        public async Task<ProductViewModel> UpdateProduct(ProductViewModel product, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJson(BASE_PATH, product);

            return await response.ReadContentAs<ProductViewModel>();
        }

        public async Task<bool> DeleteProductById(long id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"{BASE_PATH}/{id}");

            return await response.ReadContentAs<bool>();
        }
    }
}
