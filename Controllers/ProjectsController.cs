// File: Controllers/ProjectsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendTSM.Models;

namespace BackendTSM.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly TsmDbContext _context;

    public ProjectsController(TsmDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.TsmProject.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var project = await _context.TsmProject.FindAsync(id);
        return project == null ? NotFound() : Ok(project);
    }

    [HttpGet("owner/{ownerId}")]
    public async Task<IActionResult> GetByOwner(int ownerId)
    {
        var projects = await _context.TsmProject
            .Where(p => p.OwnerId == ownerId)
            .ToListAsync();
        return Ok(projects);
    }

    [HttpGet("assignee/{userId}")]
    public async Task<IActionResult> GetByAssignee(int userId)
    {
        var projects = await _context.TsmAssignee
            .Where(a => a.AssigneeId == userId)
            .Join(_context.TsmTask,
                  assignee => assignee.Id,
                  task => task.Id,
                  (assignee, task) => task)
            .Include(t => t.Project)
            .Select(t => t.Project)
            .Distinct()
            .ToListAsync();

        return Ok(projects);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
    {
        var project = new TsmProject
        {
            Name = dto.Name,
            Description = dto.Description,
            OwnerId = dto.OwnerId,
            ProjectStart = dto.ProjectStart,
            ProjectEnd = dto.ProjectEnd,
            Priority = dto.Priority,
            CreatedAt = DateTime.UtcNow
        };

        _context.TsmProject.Add(project);
        await _context.SaveChangesAsync();
        return Ok(project);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TsmProject updated)
    {
        var project = await _context.TsmProject.FindAsync(id);
        if (project == null) return NotFound();

        project.Name = updated.Name;
        project.Description = updated.Description;
        project.Priority = updated.Priority;
        project.ProjectStart = updated.ProjectStart;
        project.ProjectEnd = updated.ProjectEnd;
        await _context.SaveChangesAsync();
        return Ok(project);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _context.TsmProject.FindAsync(id);
        if (project == null) return NotFound();
        _context.TsmProject.Remove(project);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}