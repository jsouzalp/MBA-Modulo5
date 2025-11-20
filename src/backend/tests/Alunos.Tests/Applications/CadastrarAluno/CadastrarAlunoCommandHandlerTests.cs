using Alunos.Application.Commands.CadastrarAluno;
using Alunos.Domain.Entities;
using Alunos.Domain.Interfaces;
using Core.Data;
using Core.Mediator;
using FluentAssertions;
using Moq;
using Core.Messages;

namespace Alunos.Tests.Applications.CadastrarAluno;
public class CadastrarAlunoCommandHandlerTests
{
    private readonly Mock<IAlunoRepository> _repo = new();
    private readonly Mock<IMediatorHandler> _mediator = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CadastrarAlunoCommandHandler _sut;

    public CadastrarAlunoCommandHandlerTests()
    {
        _uow.Setup(u => u.Commit()).ReturnsAsync(true);
        _repo.Setup(r => r.UnitOfWork).Returns(_uow.Object);
        _sut = new CadastrarAlunoCommandHandler(_repo.Object, _mediator.Object);
    }

    private static CadastrarAlunoCommand Valido() => new(
        id: Guid.NewGuid(),
        nome: "Fulano da Silva",
        email: "fulano@exemplo.com",
        cpf: "12345678909",
        dataNascimento: DateTime.Today.AddYears(-20),
        telefone: "11999999999",
        genero: "M",
        cidade: "SP",
        estado: "SP",
        cep: "01001000",
        foto: "https://cdn.exemplo.com/foto.png"
    );

    [Fact]
    public async Task Deve_retornar_invalido_quando_command_invalido()
    {
        var cmd = new CadastrarAlunoCommand(Guid.Empty, "", "x", "", DateTime.Today.AddDays(1), new string('x', 30),
                                            new string('x', 30), new string('x', 60), "XXX", new string('x', 10), new string('x', 2000));

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeFalse();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.AtLeastOnce);
        _repo.Verify(r => r.AdicionarAsync(It.IsAny<Aluno>()), Times.Never);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_ja_existir_aluno_por_Id()
    {
        var cmd = Valido();
        _repo.Setup(r => r.ObterPorIdAsync(cmd.Id, true)).ReturnsAsync(new Aluno(cmd.Id, cmd.Nome, cmd.Email, cmd.Cpf, cmd.DataNascimento, cmd.Genero, cmd.Cidade, cmd.Estado, cmd.Cep, cmd.Foto!));

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task Deve_notificar_quando_ja_existir_aluno_por_Email()
    {
        var cmd = Valido();
        _repo.Setup(r => r.ObterPorIdAsync(cmd.Id, true)).ReturnsAsync((Aluno?)null);
        _repo.Setup(r => r.ObterPorEmailAsync(cmd.Email, true)).ReturnsAsync(new Aluno(Guid.NewGuid(), cmd.Nome, cmd.Email, cmd.Cpf, cmd.DataNascimento, cmd.Genero, cmd.Cidade, cmd.Estado, cmd.Cep, cmd.Foto!));

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeTrue();
        res.Data.Should().BeNull();
        _mediator.Verify(m => m.PublicarNotificacaoDominio(It.IsAny<DomainNotificacaoRaiz>()), Times.Once);
        _uow.Verify(u => u.Commit(), Times.Never);
    }

    [Fact]
    public async Task HappyPath_deve_cadastrar_commit_e_retornar_Id()
    {
        var cmd = Valido();
        _repo.Setup(r => r.ObterPorIdAsync(cmd.Id, true)).ReturnsAsync((Aluno?)null);
        _repo.Setup(r => r.ObterPorEmailAsync(cmd.Email, true)).ReturnsAsync((Aluno?)null);

        Aluno? criado = null;
        _repo.Setup(r => r.AdicionarAsync(It.IsAny<Aluno>()))
             .Callback<Aluno>(a => criado = a)
             .Returns(Task.CompletedTask);

        var res = await _sut.Handle(cmd, CancellationToken.None);

        criado.Should().NotBeNull();
        criado!.Email.Should().Be(cmd.Email);
        _uow.Verify(u => u.Commit(), Times.Once);

        res.IsValid.Should().BeTrue();
        res.Data.Should().Be(criado.Id);
    }

    [Fact]
    public async Task Excecao_deve_retornar_invalido_e_nao_commit()
    {
        var cmd = Valido();
        _repo.Setup(r => r.ObterPorIdAsync(cmd.Id, true)).ReturnsAsync((Aluno?)null);
        _repo.Setup(r => r.ObterPorEmailAsync(cmd.Email, true)).ReturnsAsync((Aluno?)null);
        _repo.Setup(r => r.AdicionarAsync(It.IsAny<Aluno>())).ThrowsAsync(new InvalidOperationException("boom"));

        var res = await _sut.Handle(cmd, CancellationToken.None);

        res.IsValid.Should().BeFalse();
        res.ObterErros().Any(e => e.Contains("Erro ao registrar aluno: boom")).Should().BeTrue();
        _uow.Verify(u => u.Commit(), Times.Never);
    }
}
