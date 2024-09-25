using HostMaster.Backend.Data;
using HostMaster.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HostMaster.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccommodationsController : ControllerBase
{
    private readonly DataContext _context;

    public AccommodationsController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _context.Accommodations.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var accommodation = await _context.Accommodations.FindAsync(id);
        if (accommodation == null)
        {
            return NotFound();
        }
        return Ok(accommodation);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(Accommodation accommodation)
    {
        _context.Add(accommodation);
        await _context.SaveChangesAsync();
        return Ok(accommodation);
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync(Accommodation accommodation)
    {
        var currentAccommodation = await _context.Accommodations.FindAsync(accommodation.Id);
        if (currentAccommodation == null)
        {
            return NotFound();
        }

        currentAccommodation.Name = accommodation.Name;

        _context.Update(currentAccommodation);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var accommodation = await _context.Accommodations.FindAsync(id);
        if (accommodation == null)
        {
            return NotFound();
        }

        _context.Remove(accommodation);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}