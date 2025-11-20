using Conteudo.Domain.Entities;

namespace Conteudo.UnitTests.Domains;
public class CategoriaBuilder
{
    private string _nome = "Arquitetura e Design";
    private string _descricao = "Cursos sobre DDD, SOLID, Clean Architecture, etc.";
    private string _cor = "#3366FF";
    private string _iconeUrl = "";
    private int _ordem = 0;

    public CategoriaBuilder ComNome(string v) { _nome = v; return this; }
    public CategoriaBuilder ComDescricao(string v) { _descricao = v; return this; }
    public CategoriaBuilder ComCor(string v) { _cor = v; return this; }
    public CategoriaBuilder ComIcone(string v) { _iconeUrl = v; return this; }
    public CategoriaBuilder ComOrdem(int v) { _ordem = v; return this; }

    public Categoria Build()
        => new Categoria(_nome, _descricao, _cor, _iconeUrl, _ordem);
}
