using Conteudo.Domain.Entities;

namespace Conteudo.UnitTests.Domains;
public class MaterialBuilder
{
    private Guid _aulaId = Guid.NewGuid();
    private string _nome = "Slides da Aula 1";
    private string _descricao = "Conjunto de slides introdutórios";
    private string _tipo = "PDF";
    private string _url = "https://cdn.exemplo.com/materials/aula1.pdf";
    private bool _obrigatorio = false;
    private long _tamanhoBytes = 1024 * 1024; // 1 MB
    private string _extensao = ".pdf";
    private int _ordem = 1;

    public MaterialBuilder ComAulaId(Guid v) { _aulaId = v; return this; }
    public MaterialBuilder ComNome(string v) { _nome = v; return this; }
    public MaterialBuilder ComDescricao(string v) { _descricao = v; return this; }
    public MaterialBuilder ComTipo(string v) { _tipo = v; return this; }
    public MaterialBuilder ComUrl(string v) { _url = v; return this; }
    public MaterialBuilder ComoObrigatorio(bool v = true) { _obrigatorio = v; return this; }
    public MaterialBuilder ComTamanho(long v) { _tamanhoBytes = v; return this; }
    public MaterialBuilder ComExtensao(string v) { _extensao = v; return this; }
    public MaterialBuilder ComOrdem(int v) { _ordem = v; return this; }

    public Material Build() =>
        new Material(
            _aulaId,
            _nome,
            _descricao,
            _tipo,
            _url,
            _obrigatorio,
            _tamanhoBytes,
            _extensao,
            _ordem
        );
}
