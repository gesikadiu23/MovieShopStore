using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Data;
using MovieStore.Models;
using System.Collections;
using System.Linq;

namespace MovieStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public InventoryController(AppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }


        [HttpPut("ShtoFilmaNeInventar")]
        public async Task<IActionResult> ShtoFilmaNeInventar(int quantity, int movieId)
        {
            try
            {
                var movie = _context.Movies.Find(movieId);

                if (movie == null)
                {
                    return NotFound();
                }

                movie.QuantityAvailable = movie.QuantityAvailable + quantity;

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch(Exception ex)
            {
                 Console.WriteLine(ex.ToString());
                 return BadRequest(ex.ToString());
            }

           
        }
       

        // nje metode qe merr gjithe filmat qe nuk kemi gjendje ne databaze

        [HttpGet("FilmatQeNukKaneGjendje")]
        public async Task<ActionResult<List<Movie>>> MerrFilmatQeNukKemiGjendje()
        {
            try
            {
                return _context.Movies.Where(m => m.QuantityAvailable == 0).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(ex.Message);
            }
          
        }

        // nje metode qe merr filmat qe kemi ne gjendje



        [HttpGet("FilmatQeKaneGjendje")]
        public async Task<ActionResult<List<Movie>>> FilmatQeKaneGjendje()
        {
            try
            {
                return _context.Movies.Where(m => m.QuantityAvailable > 0).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(ex.Message);
            }

        }

        // nje metode qe merr filmat me te shitur

        [HttpGet("FilmatMeTeShitur")]
        public async Task<ActionResult<List<List<Movie>>>> FilmatMeTeShitur()
        {
            try
            {

                var orders = _context.Order.Select(o => o.Movies).ToList();

                return orders;


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(ex.ToString());
            }



        }

            // nje metode qe merr filmat me pak te shitur
    }
}
