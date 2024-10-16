using Microsoft.AspNetCore.Mvc;
using TrackerAPI.Data;
using TrackerLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace TrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PeopleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/People
        [HttpGet]
        public ActionResult<IEnumerable<PersonModel>> GetPeople()
        {
            var p = _context.people.ToList(); // Assuming you have DbSet<PersonModel> in ApplicationDbContext
            if (p == null || !p.Any())
            {
                return NotFound("No people found.");
            }
            return Ok(p);
        }
    }
}
