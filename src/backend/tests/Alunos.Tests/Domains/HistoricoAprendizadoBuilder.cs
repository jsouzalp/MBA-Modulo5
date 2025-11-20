using Alunos.Domain.ValueObjects;

namespace Alunos.Tests.Domains;
public class HistoricoAprendizadoBuilder
{
    private Guid _matriculaId = Guid.NewGuid();
    private Guid _cursoId = Guid.NewGuid();
    private Guid _aulaId = Guid.NewGuid();
    private string _nomeAula = "Introdução ao DDD";
    private int _cargaHoraria = 10;
    private DateTime _dataInicio = new DateTime(2025, 01, 10);
    private DateTime? _dataTermino = null;

    public HistoricoAprendizadoBuilder ComMatriculaId(Guid id) { _matriculaId = id; return this; }
    public HistoricoAprendizadoBuilder ComCursoId(Guid id) { _cursoId = id; return this; }
    public HistoricoAprendizadoBuilder ComAulaId(Guid id) { _aulaId = id; return this; }
    public HistoricoAprendizadoBuilder ComNomeAula(string nome) { _nomeAula = nome; return this; }
    public HistoricoAprendizadoBuilder ComCargaHoraria(int horas) { _cargaHoraria = horas; return this; }
    public HistoricoAprendizadoBuilder ComDataInicio(DateTime data) { _dataInicio = data; return this; }
    public HistoricoAprendizadoBuilder ComDataTermino(DateTime? data) { _dataTermino = data; return this; }

    public HistoricoAprendizado Build()
    {
        return new HistoricoAprendizado(
            _matriculaId,
            _cursoId,
            _aulaId,
            _nomeAula,
            _cargaHoraria,
            _dataInicio,
            _dataTermino
        );
    }
}
