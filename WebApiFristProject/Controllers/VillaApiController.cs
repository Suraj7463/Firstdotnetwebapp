using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApiFristProject.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiFristProject.Controllers
{
    [Route("Api/VillaApi")]
    [ApiController]
    public class VillaApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<VillaApiController> _logger;

        public VillaApiController(ApplicationDbContext db, ILogger<VillaApiController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Villa>>> GetVillas()
        {
            _logger.LogInformation("Fetching all villas.");
            var villas = await _db.Villas.ToListAsync();
            return Ok(villas);
        }

        [HttpGet("{id:int}", Name = "GetVillaDataWithId")]
        public async Task<ActionResult<Villa>> GetVillaDataWithId(int id)
        {
            _logger.LogInformation("Fetching villa with ID: {Id}", id);
            var villa = await _db.Villas.FindAsync(id);
            if (villa == null)
            {
                _logger.LogWarning("Villa with ID {Id} not found.", id);
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        public async Task<ActionResult<Villa>> CreateVilla([FromBody] Villa villa)
        {
            _logger.LogInformation("Creating a new villa: {VillaName}", villa.Name);

            if (await _db.Villas.AnyAsync(u => u.Name.ToLower() == villa.Name.ToLower()))
            {
                _logger.LogWarning("Villa '{VillaName}' already exists.", villa.Name);
                return BadRequest("Villa already exists");
            }

            _db.Villas.Add(villa);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Villa '{VillaName}' created successfully with ID: {VillaId}.", villa.Name, villa.Id);
            return CreatedAtAction(nameof(GetVillaDataWithId), new { id = villa.Id }, villa);
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public async Task<ActionResult<Villa>> UpdateVilla(int id, [FromBody] Villa villa)
        {
            _logger.LogInformation("Updating villa with ID: {Id}", id);

            if (id != villa.Id)
            {
                _logger.LogWarning("ID mismatch for update.");
                return BadRequest("Id mismatch");
            }

            var villaData = await _db.Villas.FindAsync(id);
            if (villaData == null)
            {
                _logger.LogWarning("Villa with ID {Id} not found.", id);
                return NotFound();
            }

            villaData.Name = villa.Name;
            villaData.Details = villa.Details;
            villaData.Rate = villa.Rate;
            villaData.Sqft = villa.Sqft;
            villaData.Occupancy = villa.Occupancy;
            villaData.ImageUrl = villa.ImageUrl;
            villaData.Amenity = villa.Amenity;
            villaData.CreateDate = villa.CreateDate;
            villaData.UpdateDate = villa.UpdateDate;

            await _db.SaveChangesAsync();
            _logger.LogInformation("Villa with ID {Id} updated successfully.", id);

            return Ok(villaData);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public async Task<ActionResult<Villa>> DeleteVilla(int id)
        {
            _logger.LogInformation("Attempting to delete villa with ID: {Id}", id);

            var villa = await _db.Villas.FindAsync(id);
            if (villa == null)
            {
                _logger.LogWarning("Villa with ID {Id} not found for deletion.", id);
                return NotFound();
            }

            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Villa with ID {Id} deleted successfully.", id);

            return Ok(villa);
        }
    }
}
