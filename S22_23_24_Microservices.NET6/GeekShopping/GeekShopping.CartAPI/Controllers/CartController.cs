using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.RabbitMQSender;
using GeekShopping.CartAPI.Repository;
using GeekShopping.CartAPI.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private ICartRepository _cartRepository;
        private IRabbitMQMessageSender _rabbitMQMessageSender;
        private ICouponRepository _couponRepository;

        public CartController(ICartRepository cartRepository, IRabbitMQMessageSender rabbitMQMessageSender, ICouponRepository couponRepository)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _rabbitMQMessageSender = rabbitMQMessageSender ?? throw new ArgumentNullException(nameof(rabbitMQMessageSender));
            _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
        }

        [HttpGet("find-cart/{id}")]
        public async Task<ActionResult<CartVO>> FindById(string id)
        {
            var cart = await _cartRepository.FindCartByUserId(id);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost("add-cart")]
        public async Task<ActionResult<CartVO>> AddCart(CartVO vo)
        {
            var cart = await _cartRepository.SaveOrUpdateCart(vo);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPut("update-cart")]
        public async Task<ActionResult<CartVO>> UpdateCart(CartVO vo)
        {
            var cart = await _cartRepository.SaveOrUpdateCart(vo);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult<CartVO>> RemoveCart(int id)
        {
            var status = await _cartRepository.RemoveFromCart(id);
            if (!status) return BadRequest();
            return Ok(status);
        }

        [HttpPost("apply-coupon")]
        public async Task<ActionResult<bool>> ApplyCoupon(CartVO vo)
        {
            var status = await _cartRepository.ApplyCoupon(vo.CartHeader.UserId, vo.CartHeader.CupomCode);
            if (!status) return NotFound();
            return Ok(status);
        }

        [HttpDelete("remove-coupon/{userId}")]
        public async Task<ActionResult<bool>> RemoveCoupon(string userId)
        {
            var status = await _cartRepository.RemoveCoupon(userId);
            if (!status) return NotFound();
            return Ok(status);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CartVO>> Checkout(CheckoutHeaderVO vo)
        {
            string token = Request.Headers["Authorization"];

            if(vo?.UserId == null) return BadRequest();

            var cart = await _cartRepository.FindCartByUserId(vo.UserId);
            if (cart == null) return NotFound();

            if (!string.IsNullOrEmpty(vo.CupomCode))
            {
                CouponVO coupon = await _couponRepository.GetCouponByCouponCode(vo.CupomCode, token);

                if (vo.DiscountAmount != coupon.DiscountAmount)
                {
                    return StatusCode(StatusCodes.Status412PreconditionFailed);
                }
            }

            vo.CartDetails = cart.CartDetails;
            vo.DateTime = DateTime.Now;

            _rabbitMQMessageSender.SendMessage(vo, "checkoutqueue");

            await _cartRepository.ClearCart(vo.UserId);

            return Ok(vo);
        }
    }
}
