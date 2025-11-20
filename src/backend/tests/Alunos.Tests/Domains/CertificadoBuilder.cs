using Alunos.Domain.Entities;

namespace Alunos.Tests.Domains;
public class CertificadoBuilder
{
    private Guid _matriculaId = Guid.NewGuid();
    private string _nomeCurso = "Curso de DDD";
    private DateTime? _dataSolicitacao = new DateTime(2025, 01, 10);
    private DateTime? _dataEmissao = null;
    private short _cargaHoraria = 40;
    private decimal _notaFinal = 8m;
    private string _pathCertificado = "certificados/abc123.pdf";
    private string _nomeInstrutor = "Martin Fowler";

    public CertificadoBuilder ComMatriculaId(Guid id) { _matriculaId = id; return this; }
    public CertificadoBuilder ComNomeCurso(string nome) { _nomeCurso = nome; return this; }
    public CertificadoBuilder ComDataSolicitacao(DateTime? data) { _dataSolicitacao = data; return this; }
    public CertificadoBuilder ComDataEmissao(DateTime? data) { _dataEmissao = data; return this; }
    public CertificadoBuilder ComCargaHoraria(short horas) { _cargaHoraria = horas; return this; }
    public CertificadoBuilder ComNotaFinal(decimal nota) { _notaFinal = nota; return this; }
    public CertificadoBuilder ComPath(string path) { _pathCertificado = path; return this; }
    public CertificadoBuilder ComInstrutor(string nome) { _nomeInstrutor = nome; return this; }

    public Certificado Build()
    {
        return new Certificado(
            _matriculaId,
            _nomeCurso,
            _dataSolicitacao,
            _dataEmissao,
            _cargaHoraria,
            _notaFinal,
            _pathCertificado,
            _nomeInstrutor
        );
    }
}
