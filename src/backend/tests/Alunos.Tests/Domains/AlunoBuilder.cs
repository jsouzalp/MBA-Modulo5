using Alunos.Domain.Entities;

namespace Alunos.Tests.Domains;
public class AlunoBuilder
{
    private Guid _codigoAuth = Guid.NewGuid();
    private string _nome = "Fulano de Tal";
    private string _email = "FULANO.TESTE@EXEMPLO.COM.BR"; // proposital: upper p/ validar lower
    private string _cpf = "12345678909";                   // 11 dígitos
    private DateTime _nasc = new DateTime(1995, 1, 1);
    private string _genero = "M";
    private string _cidade = "São Paulo";
    private string _estado = "SP";
    private string _cep = "12345-678";                     // será sanitizado -> 12345678
    private string _foto = "foto.png";

    public AlunoBuilder ComCodigoAuth(Guid v) { _codigoAuth = v; return this; }
    public AlunoBuilder ComNome(string v) { _nome = v; return this; }
    public AlunoBuilder ComEmail(string v) { _email = v; return this; }
    public AlunoBuilder ComCpf(string v) { _cpf = v; return this; }
    public AlunoBuilder NascidoEm(DateTime v) { _nasc = v; return this; }
    public AlunoBuilder ComGenero(string v) { _genero = v; return this; }
    public AlunoBuilder NaCidade(string v) { _cidade = v; return this; }
    public AlunoBuilder NoEstado(string v) { _estado = v; return this; }
    public AlunoBuilder ComCep(string v) { _cep = v; return this; }
    public AlunoBuilder ComFoto(string v) { _foto = v; return this; }

    public Aluno Build()
        => new Aluno(_codigoAuth, _nome, _email, _cpf, _nasc, _genero, _cidade, _estado, _cep, _foto);

    public Aluno BuildAtivo()
    {
        var a = Build();
        a.AtivarAluno();
        return a;
    }
}
