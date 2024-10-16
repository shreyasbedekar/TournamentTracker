using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackerLibrary.Models;
using TrackerAPI.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TeamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Teams
        [HttpGet]
        public async Task<IActionResult> GetTeams()
        {
            var teams = await _context.teams
                .Include(t => t.TeamMembers) // Include team members
                .ToListAsync();

            return Ok(teams);
        }

        // GET: api/Teams/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeam(int id)
        {
            var team = await _context.teams
                .Include(t => t.TeamMembers)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (team == null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        // POST: api/Teams
        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] TeamModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Model validation failed", errors = ModelState });
            }

            // Add the new team to the database
            await _context.teams.AddAsync(model);
            await _context.SaveChangesAsync(); // Save to get the Id

            // Now set the team ID for each team member
            foreach (var teamMember in model.TeamMembers)
            {
                teamMember.TeamId = model.Id; // Set TeamId for new members

                // Check if the member exists
                var existingMember = await _context.people
                    .FirstOrDefaultAsync(p => p.Id == teamMember.Id);

                if (existingMember == null)
                {
                    // Create the member if they do not exist
                    await _context.people.AddAsync(teamMember); // Add the team member
                }
                else
                {
                    // If the member exists, update the information
                    existingMember.TeamId = model.Id;
                    existingMember.FirstName = teamMember.FirstName;
                    existingMember.LastName = teamMember.LastName;
                    existingMember.EmailAddress = teamMember.EmailAddress;
                    existingMember.CellphoneNumber = teamMember.CellphoneNumber;

                    _context.Entry(existingMember).State = EntityState.Modified;
                }
            }

            // Save changes for the team members
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeam), new { id = model.Id }, model);
        }

        // PUT: api/Teams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] TeamModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                // Save the team changes
                await _context.SaveChangesAsync();

                // Remove any team members that are no longer in the updated team
                var existingMembers = await _context.people.Where(p => p.TeamId == id).ToListAsync();
                var updatedMembersIds = model.TeamMembers.Select(m => m.Id).ToList();

                // Remove members not in the updated list
                var membersToRemove = existingMembers.Where(m => !updatedMembersIds.Contains(m.Id)).ToList();
                if (membersToRemove.Any())
                {
                    _context.people.RemoveRange(membersToRemove);
                }

                // Update the existing members or add new ones
                foreach (var teamMember in model.TeamMembers)
                {
                    teamMember.TeamId = model.Id; // Ensure TeamId is updated

                    var existingMember = await _context.people
                        .FirstOrDefaultAsync(p => p.Id == teamMember.Id);

                    if (existingMember == null)
                    {
                        // Add new members to the team
                        await _context.people.AddAsync(teamMember);
                    }
                    else
                    {
                        // Update existing members
                        existingMember.TeamId = model.Id;
                        existingMember.FirstName = teamMember.FirstName;
                        existingMember.LastName = teamMember.LastName;
                        existingMember.EmailAddress = teamMember.EmailAddress;
                        existingMember.CellphoneNumber = teamMember.CellphoneNumber;

                        _context.Entry(existingMember).State = EntityState.Modified;
                    }
                }

                // Save the updated team members
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
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

        // DELETE: api/Teams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            // Remove all members of this team
            var members = await _context.people.Where(p => p.TeamId == id).ToListAsync();
            _context.people.RemoveRange(members);

            // Remove the team itself
            _context.teams.Remove(team);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeamExists(int id)
        {
            return _context.teams.Any(e => e.Id == id);
        }
    }
}
