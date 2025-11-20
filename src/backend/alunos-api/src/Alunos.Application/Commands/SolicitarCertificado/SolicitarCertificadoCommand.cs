using Core.Messages;

namespace Alunos.Application.Commands.SolicitarCertificado;

public class SolicitarCertificadoCommand : RaizCommand
{
    public Guid AlunoId { get; private set; }
    public Guid MatriculaCursoId { get; private set; }

    public SolicitarCertificadoCommand(Guid alunoId, Guid matriculaCursoId)
    {
        DefinirRaizAgregacao(alunoId);

        AlunoId = alunoId;
        MatriculaCursoId = matriculaCursoId;
    }
}
