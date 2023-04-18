using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MovieStore.Data;
using MovieStore.DTOs;
using MovieStore.Models;

namespace MovieStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public OrdersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("ShtoOrder")]
        public async Task<IActionResult> ShtoOrder(OrderDTO order)
        {
            int couponVal = 0;

            var movies = await _dbContext.Movies.Where(m => order.MovieIds.Contains(m.Id)).ToListAsync();


            foreach(var movie in movies)
            {
                if (movie.QuantityAvailable < 1)
                { 
                    movies.Remove(movie);
                }
                else
                {
                    movie.QuantityAvailable = movie.QuantityAvailable - 1;
                }
            }

            var couponExists = await _dbContext.Coupons.AnyAsync(a => a.CouponCode == order.CouponCode 
                                                                 && a.CouponExpirationDate >= DateTime.Now 
                                                                 && a.CouponExpirationDate.Year == DateTime.Now.Year);

            if (couponExists)
            {
                 couponVal = _dbContext.Coupons.Where(a => a.CouponCode == order.CouponCode
                                                    && a.CouponExpirationDate >= DateTime.Now
                                                    && a.CouponExpirationDate.Year == DateTime.Now.Year)
                                                .OrderByDescending(a => a.CouponValue)
                                                .Take(1)
                                                .First()
                                                .CouponValue;
            }


            var orderPrice = _dbContext.Movies.Where(m => order.MovieIds.Contains(m.Id)).Sum(a => a.price);

            var ord = new Order
            {
                OrderID = order.OrderDate.ToString() + order.CustomerID.ToString() + orderPrice.ToString(),
                OrderDate = order.OrderDate,
                CustomerID = order.CustomerID,
                Price = orderPrice,        
                DiscountPrice = orderPrice - (orderPrice * (couponVal / 100)),
                Movies = movies
            };

            await _dbContext.AddAsync(ord);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }


        [HttpGet("GetOrder")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = _dbContext.Order.Include(a => a.Movies).Where(o => o.Id == id).FirstOrDefault();

            return Ok(order);
        }

    }
}
