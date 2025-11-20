using Alunos.Application.Commands.ConcluirCurso;
using Core.SharedDtos.Conteudo;
using FluentAssertions;

namespace Alunos.Tests.Applications.ConcluirCurso;
public class ConcluirCursoCommandTests
{
    [Fact]
    public void Ctor_deve_definir_RaizAgregacao_com_AlunoId()
    {
        var alunoId = Guid.NewGuid();
        var cmd = new ConcluirCursoCommand(alunoId, Guid.NewGuid(), new CursoDto { Id = Guid.NewGuid(), Aulas = [] });

        cmd.RaizAgregacao.Should().Be(alunoId);
    }
}
