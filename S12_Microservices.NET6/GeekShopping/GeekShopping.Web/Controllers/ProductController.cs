using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [Authorize]
        public async Task<IActionResult> ProductIndex()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var products = await _productService.FindAllProducts(accessToken);

            return View(products);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductModel model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            if (ModelState.IsValid)
            {
                var product = await _productService.CreateProduct(model, accessToken);

                if (product != null) return RedirectToAction(nameof(ProductIndex));
            }

            return View(model);
        }

        public async Task<IActionResult> ProductUpdate(long id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var product = await _productService.FindProductById(id, accessToken);

            if (product != null) return View(product);

            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ProductUpdate(ProductModel model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            if (ModelState.IsValid)
            {
                var product = await _productService.UpdateProduct(model, accessToken);

                if (product != null) return RedirectToAction(nameof(ProductIndex));
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> ProductDelete(long id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var product = await _productService.FindProductById(id, accessToken);

            if (product != null) return View(product);

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ProductDelete(ProductModel model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _productService.DeleteProductById(model.Id, accessToken);

            if (response) return RedirectToAction(nameof(ProductIndex));

            return View(model);
        }
    }
}
