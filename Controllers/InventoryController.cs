using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Data;

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
            var movie = _context.Movies.Find(movieId);

            if (movie == null) {
            
                return NotFound();

            }

            movie.QuantityAvailable = movie.QuantityAvailable + quantity;

            await _context.SaveChangesAsync();

            return Ok();
        }

        // nje metode qe merr gjithe filmat qe nuk kemi gjendje ne databaze
        // nje metode qe merr filmat qe kemi ne gjendje
        // nje metode qe merr filmat me te shitur
        // nje metode qe merr filmat me pak te shitur
    }
}
