namespace Core.Communication;

public class PagedResult<T> where T : class
{
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int TotalResults { get; set; }
    public string? Query { get; set; }
    public IEnumerable<T> Items { get; set; } = [];
}