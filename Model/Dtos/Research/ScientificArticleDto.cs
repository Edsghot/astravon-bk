﻿namespace Astragon.Model.Dtos.Research;

public record ScientificArticleDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Summary { get; set; }
    public DateTime Date { get; set; }
    public string Doi { get; set; }
    public string Authors { get; set; }
    public string Pdf { get; set; }
    public int IdNivel { get; set; }
    public int? Estatus { get; set; }
}