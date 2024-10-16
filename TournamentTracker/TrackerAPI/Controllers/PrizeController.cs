using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackerAPI.Data;
using TrackerLibrary.Models;

namespace TrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrizeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrizeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Prizes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrizeModel>>> GetPrizes()
        {
            return await _context.prizes.ToListAsync();
        }

        // GET: api/Prizes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrizeModel>> GetPrize(int id)
        {
            var prize = await _context.prizes.FindAsync(id);

            if (prize == null)
            {
                return NotFound();
            }

            return prize;
        }

        // POST: api/Prizes
        [HttpPost]
        public async Task<ActionResult<PrizeModel>> CreatePrize(PrizeModel prizeModel)
        {
            _context.prizes.Add(prizeModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPrize), new { id = prizeModel.Id }, prizeModel);
        }

        // PUT: api/Prizes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrize(int id, PrizeModel prizeModel)
        {
            if (id != prizeModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(prizeModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrizeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Prizes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrize(int id)
        {
            var prize = await _context.prizes.FindAsync(id);
            if (prize == null)
            {
                return NotFound();
            }

            _context.prizes.Remove(prize);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrizeExists(int id)
        {
            return _context.prizes.Any(e => e.Id == id);
        }
    }
}
