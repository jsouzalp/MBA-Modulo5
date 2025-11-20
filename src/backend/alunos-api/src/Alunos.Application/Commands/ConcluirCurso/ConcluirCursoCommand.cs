using Core.Messages;
using Core.SharedDtos.Conteudo;

namespace Alunos.Application.Commands.ConcluirCurso;

public class ConcluirCursoCommand : RaizCommand
{
    public Guid AlunoId { get; init; }
    public Guid MatriculaCursoId { get; init; }
    public CursoDto CursoDto { get; init; }

    public ConcluirCursoCommand(Guid alunoId, Guid matriculaCursoId, CursoDto cursoDto)
    {
        DefinirRaizAgregacao(alunoId);

        AlunoId = alunoId;
        MatriculaCursoId = matriculaCursoId;
        CursoDto = cursoDto;
    }
}
