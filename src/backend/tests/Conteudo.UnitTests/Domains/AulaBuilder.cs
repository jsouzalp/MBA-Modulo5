using Conteudo.Domain.Entities;

namespace Conteudo.UnitTests.Domains;
public class AulaBuilder
{
    private Guid _cursoId = Guid.NewGuid();
    private string _nome = "Aula 01 - Introdução ao DDD";
    private string _descricao = "Visão geral, termos e blocos táticos.";
    private int _numero = 1;
    private int _duracaoMinutos = 75;
    private string _videoUrl = "https://cdn.exemplo.com/videos/aula01.mp4";
    private string _tipoAula = "Vídeo";
    private bool _isObrigatoria = true;
    private string _observacoes = "Sem pré-requisitos";

    public AulaBuilder ComCursoId(Guid v) { _cursoId = v; return this; }
    public AulaBuilder ComNome(string v) { _nome = v; return this; }
    public AulaBuilder ComDescricao(string v) { _descricao = v; return this; }
    public AulaBuilder ComNumero(int v) { _numero = v; return this; }
    public AulaBuilder ComDuracao(int v) { _duracaoMinutos = v; return this; }
    public AulaBuilder ComVideoUrl(string v) { _videoUrl = v; return this; }
    public AulaBuilder ComTipo(string v) { _tipoAula = v; return this; }
    public AulaBuilder Obrigatoria(bool v = true) { _isObrigatoria = v; return this; }
    public AulaBuilder ComObservacoes(string v) { _observacoes = v; return this; }

    public Aula Build() =>
        new Aula(
            _cursoId,
            _nome,
            _descricao,
            _numero,
            _duracaoMinutos,
            _videoUrl,
            _tipoAula,
            _isObrigatoria,
            _observacoes
        );
}
