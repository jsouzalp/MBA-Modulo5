namespace Core.Communication;

public class ApiError
{
    public string Message { get; set; } = string.Empty;
    public IEnumerable<string>? Details { get; set; }
}