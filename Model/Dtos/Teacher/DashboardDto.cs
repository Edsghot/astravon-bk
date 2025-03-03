namespace Astravon.Model.Dtos.Teacher;

public class ReportDataDto
{
    public UserDto User { get; set; }
    public List<StatDto> Stats { get; set; }
    public GraficoDto Grafico { get; set; }
    public List<TablaDto> Tabla { get; set; }
}

public class UserDto
{
    public string Name { get; set; }
    public string ProfileImage { get; set; }
}

public class StatDto
{
    public string Title { get; set; }
    public string Total { get; set; }
    public string Rate { get; set; }
    public bool LevelUp { get; set; }
    public string Icon { get; set; }
}

public class GraficoDto
{
    public List<SeriesDto> Series { get; set; }
}

public class SeriesDto
{
    public string Name { get; set; }
    public List<int> Data { get; set; }
}

public class TablaDto
{
    public string Name { get; set; }
    public string Date { get; set; }
}