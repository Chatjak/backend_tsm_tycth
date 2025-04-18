﻿public class CreateProjectDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int OwnerId { get; set; }
    public DateTime? ProjectStart { get; set; }
    public DateTime? ProjectEnd { get; set; }
    public string? Priority { get; set; }
}