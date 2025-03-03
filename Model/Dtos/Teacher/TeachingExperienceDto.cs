﻿namespace Astravon.Model.Dtos.Teacher;

public record TeachingExperienceDto
{
    public int Id { get; set; }
    public string Institution { get; set; } = string.Empty;
    public string InstitutionType { get; set; } = string.Empty;
    public string TeacherType { get; set; } = string.Empty;
    public string JobDescription { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TeacherId { get; set; }
}