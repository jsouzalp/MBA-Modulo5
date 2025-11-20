using Alunos.Domain.Entities;

namespace Alunos.Tests.Domains;
public class MatriculaCursoBuilder
{
    private Guid _alunoId = Guid.NewGuid();
    private Guid _cursoId = Guid.NewGuid();
    private string _nomeCurso = "Curso Completo de DDD"; // >= 10 chars
    private decimal _valor = 1000.50m;
    private string _observacao = "Observação inicial";

    public MatriculaCursoBuilder ComAlunoId(Guid id) { _alunoId = id; return this; }
    public MatriculaCursoBuilder ComCursoId(Guid id) { _cursoId = id; return this; }
    public MatriculaCursoBuilder ComNomeCurso(string nome) { _nomeCurso = nome; return this; }
    public MatriculaCursoBuilder ComValor(decimal valor) { _valor = valor; return this; }
    public MatriculaCursoBuilder ComObservacao(string obs) { _observacao = obs; return this; }

    public MatriculaCurso Build()
        => new MatriculaCurso(_alunoId, _cursoId, _nomeCurso, _valor, _observacao);

    /// <summary>
    /// Constrói e já marca pagamento realizado (estado disponível p/ registrar histórico).
    /// </summary>
    public MatriculaCurso BuildPago()
    {
        var m = Build();
        m.RegistrarPagamentoMatricula();
        return m;
    }
}
