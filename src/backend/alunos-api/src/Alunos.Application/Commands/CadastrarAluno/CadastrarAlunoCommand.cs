using Core.Communication;
using Core.Messages;
using MediatR;

namespace Alunos.Application.Commands.CadastrarAluno;

public class CadastrarAlunoCommand : RaizCommand, IRequest<CommandResult>
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

    public CadastrarAlunoCommand(Guid id,
        string nome,
        string email,
        string cpf,
        DateTime dataNascimento,
        string telefone,
        string genero,
        string cidade,
        string estado,
        string cep,
        string foto)
    {
        DefinirRaizAgregacao(id);

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
