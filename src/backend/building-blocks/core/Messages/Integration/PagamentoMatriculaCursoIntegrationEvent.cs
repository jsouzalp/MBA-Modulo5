namespace Core.Messages.Integration;

public class PagamentoMatriculaCursoIntegrationEvent : IntegrationEvent
{
    public Guid AlunoId { get; init; }
    public Guid CursoId { get; init; }

    public PagamentoMatriculaCursoIntegrationEvent(Guid alunoId, Guid cursoId)
    {
        DefinirRaizAgregacao(alunoId);

        AlunoId = alunoId;
        CursoId = cursoId;
    }
}
