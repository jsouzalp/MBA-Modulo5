using Conteudo.Domain.Entities;
using Conteudo.Domain.ValueObjects;

namespace Conteudo.UnitTests.Domains;
public class CursoBuilder
{
    private string _nome = "Arquitetura Limpa e DDD";
    private decimal _valor = 499.90m;
    private int _duracaoHoras = 40;
    private string _nivel = "Intermediário";
    private string _instrutor = "Tio Bob";
    private int _vagasMaximas = 30;
    private string _imagemUrl = "";
    private DateTime? _validoAte = null;
    private Guid? _categoriaId = null;
    private ConteudoProgramatico _conteudo = new ConteudoProgramatico(
        resumo: "Resumo do curso",
        descricao: "Descrição detalhada",
        objetivos: "Aprender DDD e Clean Architecture",
        preRequisitos: "C# básico",
        publicoAlvo: "Devs .NET",
        metodologia: "Aulas e labs",
        recursos: "Slides e repositório",
        avaliacao: "Projeto final",
        bibliografia: "Livros e docs"
    );

    public CursoBuilder ComNome(string v) { _nome = v; return this; }
    public CursoBuilder ComValor(decimal v) { _valor = v; return this; }
    public CursoBuilder ComDuracao(int horas) { _duracaoHoras = horas; return this; }
    public CursoBuilder ComNivel(string v) { _nivel = v; return this; }
    public CursoBuilder ComInstrutor(string v) { _instrutor = v; return this; }
    public CursoBuilder ComVagas(int v) { _vagasMaximas = v; return this; }
    public CursoBuilder ComImagem(string v) { _imagemUrl = v; return this; }
    public CursoBuilder ValidoAte(DateTime? v) { _validoAte = v; return this; }
    public CursoBuilder ComCategoria(Guid? v) { _categoriaId = v; return this; }
    public CursoBuilder ComConteudo(ConteudoProgramatico v) { _conteudo = v; return this; }

    public Curso Build() =>
        new Curso(
            nome: _nome,
            valor: _valor,
            conteudoProgramatico: _conteudo,
            duracaoHoras: _duracaoHoras,
            nivel: _nivel,
            instrutor: _instrutor,
            vagasMaximas: _vagasMaximas,
            imagemUrl: _imagemUrl,
            validoAte: _validoAte,
            categoriaId: _categoriaId
        );
}
