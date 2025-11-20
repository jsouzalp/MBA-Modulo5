namespace Core.Communication.Filters;

public class CursoFilter
{
    public int PageSize { get; set; } = 8;
    public int PageIndex { get; set; } = 1;
    public string? Query { get; set; }
    public bool IncludeAulas { get; set; }
    public bool Ativos { get; set; } = true;
}