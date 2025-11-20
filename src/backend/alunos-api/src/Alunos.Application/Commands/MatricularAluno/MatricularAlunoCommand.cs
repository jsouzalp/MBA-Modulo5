using Core.Messages;

namespace Alunos.Application.Commands.MatricularAluno;

public class MatricularAlunoCommand : RaizCommand
{
    public Guid AlunoId { get; init; }
    public Guid CursoId { get; init; }
    public bool CursoDisponivel { get; init; }
    public string NomeCurso { get; init; }
    public decimal ValorCurso { get; init; }
    public string Observacao { get; init; }

    public MatricularAlunoCommand(Guid alunoId, Guid cursoId, bool cursoDisponivel, string nomeCurso, decimal valorCurso, string observacao)
    {
        DefinirRaizAgregacao(alunoId);

        AlunoId = alunoId;
        CursoId = cursoId;
        CursoDisponivel = cursoDisponivel;
        NomeCurso = nomeCurso;
        ValorCurso = valorCurso;
        Observacao = observacao;
    }
}
