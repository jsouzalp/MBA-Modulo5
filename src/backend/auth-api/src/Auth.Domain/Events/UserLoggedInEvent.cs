namespace Auth.Domain.Events;

public class UserLoggedInEvent
{
    public UserLoggedInEvent(Guid userId, string email, string nome, DateTime dataLogin, string ipAddress)
    {
        UserId = userId;
        Email = email;
        Nome = nome;
        DataLogin = dataLogin;
        IpAddress = ipAddress;
    }

    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public DateTime DataLogin { get; set; }
    public string IpAddress { get; set; } = string.Empty;
}
