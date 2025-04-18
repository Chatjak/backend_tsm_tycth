using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendTSM.Models;

namespace BackendTSM.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly TsmDbContext _context;

    public UsersController(TsmDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.TsmUser.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _context.TsmUser.FindAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TsmUser user)
    {
        user.CreatedAt = DateTime.UtcNow;
        _context.TsmUser.Add(user);
        await _context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TsmUser updated)
    {
        var user = await _context.TsmUser.FindAsync(id);
        if (user == null) return NotFound();

        user.EmpNo = updated.EmpNo;
        user.EmpName = updated.EmpName;
        user.EmpEmail = updated.EmpEmail;
        await _context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.TsmUser.FindAsync(id);
        if (user == null) return NotFound();
        _context.TsmUser.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}