using Core.Messages;

namespace Alunos.Application.Commands.RegistrarHistoricoAprendizado;

public class RegistrarHistoricoAprendizadoCommand : RaizCommand
{
    public Guid AlunoId { get; private set; }
    public Guid MatriculaCursoId { get; private set; }
    public Guid AulaId { get; private set; }
    public DateTime? DataTermino { get; private set; }
    public string NomeAula { get; private set; }
    public byte DuracaoMinutos { get; private set; }

    public RegistrarHistoricoAprendizadoCommand(Guid alunoId, Guid matriculaCursoId, Guid aulaId, string nomeAula, byte duracaoEmMinutos, DateTime? dataTermino = null)
    {
        DefinirRaizAgregacao(alunoId);

        AlunoId = alunoId;
        MatriculaCursoId = matriculaCursoId;
        AulaId = aulaId;
        DataTermino = dataTermino;
        NomeAula = nomeAula;
        DuracaoMinutos = duracaoEmMinutos;
    }
}
