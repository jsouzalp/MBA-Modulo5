using Conteudo.Domain.ValueObjects;

namespace Conteudo.UnitTests.Domains;
public class ConteudoProgramaticoBuilder
{
    private string _resumo = "Resumo do curso";
    private string _descricao = "Descrição detalhada do conteúdo programático";
    private string _objetivos = "Aprender DDD, SOLID e TDD na prática";
    private string? _preRequisitos = "Noções básicas de C#";
    private string? _publicoAlvo = "Desenvolvedores backend";
    private string? _metodologia = "Aulas expositivas e laboratórios";
    private string? _recursos = "Slides, repositório Git, exercícios";
    private string? _avaliacao = "Prova + projeto final";
    private string? _bibliografia = "Livro A; Artigo B; Documentação oficial";

    public ConteudoProgramaticoBuilder ComResumo(string v) { _resumo = v; return this; }
    public ConteudoProgramaticoBuilder ComDescricao(string v) { _descricao = v; return this; }
    public ConteudoProgramaticoBuilder ComObjetivos(string v) { _objetivos = v; return this; }
    public ConteudoProgramaticoBuilder ComPreRequisitos(string? v) { _preRequisitos = v; return this; }
    public ConteudoProgramaticoBuilder ComPublicoAlvo(string? v) { _publicoAlvo = v; return this; }
    public ConteudoProgramaticoBuilder ComMetodologia(string? v) { _metodologia = v; return this; }
    public ConteudoProgramaticoBuilder ComRecursos(string? v) { _recursos = v; return this; }
    public ConteudoProgramaticoBuilder ComAvaliacao(string? v) { _avaliacao = v; return this; }
    public ConteudoProgramaticoBuilder ComBibliografia(string? v) { _bibliografia = v; return this; }

    public ConteudoProgramatico Build() =>
        new ConteudoProgramatico(
            _resumo,
            _descricao,
            _objetivos,
            _preRequisitos!,
            _publicoAlvo!,
            _metodologia!,
            _recursos!,
            _avaliacao!,
            _bibliografia!);
}
