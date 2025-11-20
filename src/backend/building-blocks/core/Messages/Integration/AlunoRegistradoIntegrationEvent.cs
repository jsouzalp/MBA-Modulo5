namespace Core.Messages.Integration;

public class AlunoRegistradoIntegrationEvent : IntegrationEvent
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string Cpf { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public string Telefone { get; private set; }
    public string Genero { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }
    public string Cep { get; private set; }
    public string? Foto { get; private set; }

    public AlunoRegistradoIntegrationEvent(
        Guid id,
        string nome,
        string email,
        string cpf,
        DateTime dataNascimento,
        string telefone,
        string genero,
        string cidade,
        string estado,
        string cep,
        string? foto)
    {
        Id = id;
        Nome = nome;
        Email = email;
        Cpf = cpf;
        DataNascimento = dataNascimento;
        Telefone = telefone;
        Genero = genero;
        Cidade = cidade;
        Estado = estado;
        Cep = cep;
        Foto = foto;
    }
}
