using GeekShopping.CartAPI.ValueObjects;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GeekShopping.CartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _client;

        public CouponRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<CouponVO> GetCouponByCouponCode(string couponCode, string token)
        {
            //"api/v1/coupon"
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.GetAsync($"api/v1/coupon/{couponCode}");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new CouponVO();
            }
            else
            {
                return JsonSerializer.Deserialize<CouponVO>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }
    }
}
