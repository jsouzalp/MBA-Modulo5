using Alunos.Domain.Entities;
using Alunos.Domain.Interfaces;
using Alunos.Infrastructure.Data;
using Alunos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Alunos.IntegrationTests;

public class DatabaseIntegrationTests : IDisposable
{
    private readonly AlunoDbContext _context;
    private readonly IAlunoRepository _alunoRepository;

    public DatabaseIntegrationTests()
    {
        // Configuração do banco em memória para testes
        var services = new ServiceCollection();

        services.AddDbContext<AlunoDbContext>(options =>
        {
            options.UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}");
        });

        services.AddScoped<IAlunoRepository, AlunoRepository>();

        var serviceProvider = services.BuildServiceProvider();

        _context = serviceProvider.GetRequiredService<AlunoDbContext>();
        _alunoRepository = serviceProvider.GetRequiredService<IAlunoRepository>();

        // Garante que o banco seja criado
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task AdicionarAluno_DevePersistirNoBanco()
    {
        // Arrange
        var aluno = new Aluno(
            Guid.NewGuid(),
            "João Silva",
            "joao@teste.com",
            "12345678901",
            DateTime.Now.AddYears(-25),
            "Masculino",
            "São Paulo",
            "SP",
            "01234-567",
            "foto.jpg");

        // Act
        await _alunoRepository.AdicionarAsync(aluno);
        await _context.SaveChangesAsync();

        // Assert
        var alunoPersistido = await _context.Alunos.FirstOrDefaultAsync(a => a.Id == aluno.Id);
        alunoPersistido.Should().NotBeNull();
        alunoPersistido!.Nome.Should().Be("João Silva");
        alunoPersistido.Email.Should().Be("joao@teste.com");
    }

    [Fact]
    public async Task ObterAlunoPorCodigoUsuario_ComAlunoExistente_DeveRetornarAluno()
    {
        // Arrange
        var codigoUsuario = Guid.NewGuid();
        var aluno = new Aluno(
            codigoUsuario,
            "Maria Silva",
            "maria@teste.com",
            "98765432100",
            DateTime.Now.AddYears(-30),
            "Feminino",
            "Rio de Janeiro",
            "RJ",
            "20000-000",
            "foto-maria.jpg");

        await _alunoRepository.AdicionarAsync(aluno);
        await _context.SaveChangesAsync();

        // Act
        var alunoEncontrado = await _alunoRepository.ObterPorCodigoUsuarioAsync(codigoUsuario);

        // Assert
        alunoEncontrado.Should().NotBeNull();
        alunoEncontrado!.Nome.Should().Be("Maria Silva");
        alunoEncontrado.Email.Should().Be("maria@teste.com");
    }

    [Fact]
    public async Task ObterAlunoPorCodigoUsuario_ComAlunoNaoExistente_DeveRetornarNull()
    {
        // Arrange
        var codigoUsuario = Guid.NewGuid();

        // Act
        var alunoEncontrado = await _alunoRepository.ObterPorCodigoUsuarioAsync(codigoUsuario);

        // Assert
        alunoEncontrado.Should().BeNull();
    }

    [Fact]
    public async Task ObterAlunoPorEmail_ComEmailExistente_DeveRetornarAluno()
    {
        // Arrange
        var email = "pedro@teste.com";
        var aluno = new Aluno(
            Guid.NewGuid(),
            "Pedro Santos",
            email,
            "11122233344",
            DateTime.Now.AddYears(-28),
            "Masculino",
            "Belo Horizonte",
            "MG",
            "30000-000",
            "foto-pedro.jpg");

        await _alunoRepository.AdicionarAsync(aluno);
        await _context.SaveChangesAsync();

        // Act
        var alunoEncontrado = await _alunoRepository.ObterPorEmailAsync(email);

        // Assert
        alunoEncontrado.Should().NotBeNull();
        alunoEncontrado!.Nome.Should().Be("Pedro Santos");
        alunoEncontrado.Email.Should().Be(email);
    }

    [Fact]
    public async Task ObterAlunoPorEmail_ComEmailNaoExistente_DeveRetornarNull()
    {
        // Arrange
        var email = "inexistente@teste.com";

        // Act
        var alunoEncontrado = await _alunoRepository.ObterPorEmailAsync(email);

        // Assert
        alunoEncontrado.Should().BeNull();
    }

    [Fact]
    public async Task ObterAlunoPorCodigoUsuario_ComCodigoExistente_DeveRetornarAluno()
    {
        // Arrange
        var codigoUsuario = Guid.NewGuid();
        var aluno = new Aluno(
            codigoUsuario,
            "Ana Costa",
            "ana@teste.com",
            "55566677788",
            DateTime.Now.AddYears(-26),
            "Feminino",
            "Salvador",
            "BA",
            "40000-000",
            "foto-ana.jpg");

        await _alunoRepository.AdicionarAsync(aluno);
        await _context.SaveChangesAsync();

        // Act
        var alunoEncontrado = await _alunoRepository.ObterPorCodigoUsuarioAsync(codigoUsuario);

        // Assert
        alunoEncontrado.Should().NotBeNull();
        alunoEncontrado!.Nome.Should().Be("Ana Costa");
        alunoEncontrado.Cpf.Should().Be("55566677788");
    }

    [Fact]
    public async Task ObterAlunoPorCodigoUsuario_ComCodigoNaoExistente_DeveRetornarNull()
    {
        // Arrange
        var codigoUsuario = Guid.NewGuid();

        // Act
        var alunoEncontrado = await _alunoRepository.ObterPorCodigoUsuarioAsync(codigoUsuario);

        // Assert
        alunoEncontrado.Should().BeNull();
    }

    [Fact]
    public async Task ObterAlunosPorEmail_ComAlunosExistentes_DeveRetornarAlunos()
    {
        // Arrange
        var alunos = new List<Aluno>
        {
            new Aluno(
                Guid.NewGuid(),
                "Carlos Lima",
                "carlos@teste.com",
                "12312312312",
                DateTime.Now.AddYears(-27),
                "Masculino",
                "Fortaleza",
                "CE",
                "50000-000",
                "foto-carlos.jpg"),
            new Aluno(
                Guid.NewGuid(),
                "Fernanda Rocha",
                "fernanda@teste.com",
                "45645645645",
                DateTime.Now.AddYears(-29),
                "Feminino",
                "Recife",
                "PE",
                "60000-000",
                "foto-fernanda.jpg")
        };

        foreach (var aluno in alunos)
        {
            await _alunoRepository.AdicionarAsync(aluno);
        }
        await _context.SaveChangesAsync();

        // Act
        var aluno1 = await _alunoRepository.ObterPorEmailAsync("carlos@teste.com");
        var aluno2 = await _alunoRepository.ObterPorEmailAsync("fernanda@teste.com");

        // Assert
        aluno1.Should().NotBeNull();
        aluno2.Should().NotBeNull();
        aluno1!.Nome.Should().Be("Carlos Lima");
        aluno2!.Nome.Should().Be("Fernanda Rocha");
    }

    [Fact]
    public async Task AtualizarAluno_DevePersistirAlteracoes()
    {
        // Arrange
        var aluno = new Aluno(
            Guid.NewGuid(),
            "Roberto Alves",
            "roberto@teste.com",
            "78978978978",
            DateTime.Now.AddYears(-31),
            "Masculino",
            "Curitiba",
            "PR",
            "70000-000",
            "foto-roberto.jpg");

        await _alunoRepository.AdicionarAsync(aluno);
        await _context.SaveChangesAsync();

        // Act
        aluno.AtivarAluno();

        await _alunoRepository.AtualizarAsync(aluno);
        await _context.SaveChangesAsync();

        // Assert
        var alunoAtualizado = await _alunoRepository.ObterPorEmailAsync("roberto@teste.com");
        alunoAtualizado.Should().NotBeNull();
        alunoAtualizado!.Ativo.Should().BeTrue();
        alunoAtualizado.Nome.Should().Be("Roberto Alves");
        alunoAtualizado.Email.Should().Be("roberto@teste.com");
    }

    [Fact]
    public async Task InativarAluno_DeveAlterarStatusParaInativo()
    {
        // Arrange
        var aluno = new Aluno(
            Guid.NewGuid(),
            "Lucia Mendes",
            "lucia@teste.com",
            "32132132132",
            DateTime.Now.AddYears(-33),
            "Feminino",
            "Porto Alegre",
            "RS",
            "80000-000",
            "foto-lucia.jpg");

        await _alunoRepository.AdicionarAsync(aluno);
        await _context.SaveChangesAsync();

        // Act
        aluno.InativarAluno();
        await _alunoRepository.AtualizarAsync(aluno);
        await _context.SaveChangesAsync();

        // Assert
        var alunoInativado = await _alunoRepository.ObterPorEmailAsync("lucia@teste.com");
        alunoInativado.Should().NotBeNull();
        alunoInativado!.Ativo.Should().BeFalse();
    }

    [Fact]
    public async Task AtivarAlunos_DeveAlterarStatusDosAlunos()
    {
        // Arrange
        var alunos = new List<Aluno>
        {
            new Aluno(
                Guid.NewGuid(),
                "Ativo 1",
                "ativo1@teste.com",
                "11111111111",
                DateTime.Now.AddYears(-25),
                "Masculino",
                "São Paulo",
                "SP",
                "01000-000",
                "foto1.jpg"),
            new Aluno(
                Guid.NewGuid(),
                "Ativo 2",
                "ativo2@teste.com",
                "22222222222",
                DateTime.Now.AddYears(-26),
                "Feminino",
                "São Paulo",
                "SP",
                "02000-000",
                "foto2.jpg")
        };

        foreach (var aluno in alunos)
        {
            await _alunoRepository.AdicionarAsync(aluno);
        }
        await _context.SaveChangesAsync();

        // Act
        alunos[0].AtivarAluno();
        alunos[1].AtivarAluno();

        await _alunoRepository.AtualizarAsync(alunos[0]);
        await _alunoRepository.AtualizarAsync(alunos[1]);
        await _context.SaveChangesAsync();

        // Assert
        var aluno1Atualizado = await _alunoRepository.ObterPorEmailAsync("ativo1@teste.com");
        var aluno2Atualizado = await _alunoRepository.ObterPorEmailAsync("ativo2@teste.com");

        aluno1Atualizado.Should().NotBeNull();
        aluno2Atualizado.Should().NotBeNull();
        aluno1Atualizado!.Ativo.Should().BeTrue();
        aluno2Atualizado!.Ativo.Should().BeTrue();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
