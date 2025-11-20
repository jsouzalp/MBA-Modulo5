using FluentAssertions;
using Plataforma.Educacao.Core.Exceptions;

namespace Alunos.Tests.Domains;
public class CertificadoTests
{
    // ---------- Happy path ----------
    [Fact]
    public void Deve_criar_certificado_valido()
    {
        var c = new CertificadoBuilder().Build();

        c.Id.Should().NotBeEmpty();
        c.MatriculaCursoId.Should().NotBeEmpty();
        c.NomeCurso.Should().Be("Curso de DDD");
        c.CargaHoraria.Should().Be(40);
        c.NotaFinal.Should().Be(8m);
        c.PathCertificado.Should().Be("certificados/abc123.pdf");
        c.NomeInstrutor.Should().Be("Martin Fowler");
        c.DataEmissao.Should().BeNull(); // não emitido por padrão
    }

    // ---------- Matrícula (Guid) ----------
    [Fact]
    public void Deve_falhar_quando_matricula_guid_for_vazio()
    {
        var builder = new CertificadoBuilder().ComMatriculaId(Guid.Empty);

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Matrícula do curso deve ser informada"));
    }

    // ---------- Nome do curso ----------
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Deve_falhar_quando_nome_curso_vazio(string nome)
    {
        var builder = new CertificadoBuilder().ComNomeCurso(nome);

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Nome do curso não pode ser nulo ou vazio"));
    }

    [Fact]
    public void Deve_falhar_quando_nome_curso_ultrapassar_200()
    {
        var nome = new string('x', 201);
        var builder = new CertificadoBuilder().ComNomeCurso(nome);

        Action act = () => builder.Build();

        // Observação: a entidade hoje usa a mensagem de "Path..." ao validar o NomeCurso (provável typo).
        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Path do certificado deve ter no máximo 200 caracteres"));
    }

    // ---------- Data de solicitação ----------
    [Fact]
    public void Deve_falhar_quando_data_solicitacao_for_futura()
    {
        var amanha = DateTime.Now.AddDays(1);
        var builder = new CertificadoBuilder().ComDataSolicitacao(amanha);

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Data de solicitação não pode ser superior à data atual"));
    }

    [Theory]
    [InlineData("0001-01-01")]
    [InlineData("9999-12-31")]
    public void Deve_falhar_quando_data_solicitacao_invalida(string data)
    {
        var builder = new CertificadoBuilder().ComDataSolicitacao(DateTime.Parse(data));

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Data de solicitação"));
    }

    // ---------- Path do certificado ----------
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Deve_falhar_quando_path_vazio(string path)
    {
        var builder = new CertificadoBuilder().ComPath(path);

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Path do certificado não pode ser nulo ou vazio"));
    }

    [Fact]
    public void Deve_falhar_quando_path_ultrapassar_1024()
    {
        var path = new string('a', 1025);
        var builder = new CertificadoBuilder().ComPath(path);

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Path do certificado deve ter no máximo 1024 caracteres"));
    }

    // ---------- Nome do instrutor ----------
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Deve_falhar_quando_nome_instrutor_vazio(string nome)
    {
        var builder = new CertificadoBuilder().ComInstrutor(nome);

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Nome do instrutor não pode ser nulo ou vazio"));
    }

    [Fact]
    public void Deve_falhar_quando_nome_instrutor_ultrapassar_100()
    {
        var nome = new string('x', 101);
        var builder = new CertificadoBuilder().ComInstrutor(nome);

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Nome do instrutor deve ter no máximo 100 caracteres"));
    }

    // ---------- Carga horária ----------
    [Theory]
    [InlineData((short)0)]
    [InlineData((short)-1)]
    public void Deve_falhar_quando_carga_horaria_nao_positiva(short horas)
    {
        var builder = new CertificadoBuilder().ComCargaHoraria(horas);

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Carga horária deve ser maior que zero"));
    }

    [Theory]
    [InlineData((short)1)]
    [InlineData((short)10000)]
    public void Deve_aceitar_limites_validos_de_carga_horaria(short horas)
    {
        var c = new CertificadoBuilder().ComCargaHoraria(horas).Build();
        c.CargaHoraria.Should().Be(horas);
    }

    [Theory]
    [InlineData((short)10001)]
    public void Deve_falhar_quando_carga_horaria_ultrapassar_limite(short horas)
    {
        var builder = new CertificadoBuilder().ComCargaHoraria(horas);

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Carga horária deve estar entre 1 e 10.000 horas"));
    }

    // ---------- Nota final (0..10) ----------
    [Theory]
    [InlineData(-0.1)]
    [InlineData(10.1)]
    [InlineData(11.0)]
    public void Deve_falhar_quando_nota_final_fora_do_intervalo(double nota)
    {
        var builder = new CertificadoBuilder().ComNotaFinal((decimal)nota);

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Nota final deve estar entre 0 e 10"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(7.5)]
    public void Deve_aceitar_nota_final_limites_e_valor_medio(double nota)
    {
        var c = new CertificadoBuilder().ComNotaFinal((decimal)nota).Build();
        c.NotaFinal.Should().Be((decimal)nota);
    }

    // ---------- Atualizações e travas após emissão ----------
    [Fact]
    public void Deve_atualizar_data_emissao_quando_valida()
    {
        var c = new CertificadoBuilder().Build();
        var agora = DateTime.Now; // igual/antes de agora deve ser aceito pela regra vigente

        c.Invoking(x => x.AtualizarDataEmissao(agora))
         .Should().NotThrow();

        c.DataEmissao.Should().Be(agora);
    }

    [Fact]
    public void Deve_falhar_ao_atualizar_data_emissao_futura()
    {
        var c = new CertificadoBuilder().Build();

        var futuro = DateTime.Now.AddDays(1);
        c.Invoking(x => x.AtualizarDataEmissao(futuro))
         .Should().Throw<DomainException>()
         .Which.Errors.Should().Contain(e => e.Contains("Data de emissão deve ser igual ou superior à data atual"));
        // Nota: pela implementação atual, data futura dispara erro (mensagem pode soar contraditória).
    }

    [Fact]
    public void Nao_deve_permitir_modificacoes_apos_emissao()
    {
        var c = new CertificadoBuilder().Build();
        c.AtualizarDataEmissao(DateTime.Now);

        c.Invoking(x => x.AtualizarCargaHoraria(100))
         .Should().Throw<DomainException>()
         .WithMessage("*Certificado foi emitido e não pode sofrer alterações*");

        c.Invoking(x => x.AtualizarNotaFinal(9))
         .Should().Throw<DomainException>()
         .WithMessage("*Certificado foi emitido e não pode sofrer alterações*");

        c.Invoking(x => x.AtualizarPathCertificado("novo/path.pdf"))
         .Should().Throw<DomainException>()
         .WithMessage("*Certificado foi emitido e não pode sofrer alterações*");

        c.Invoking(x => x.AtualizarNomeInstrutor("Outro Instrutor"))
         .Should().Throw<DomainException>()
         .WithMessage("*Certificado foi emitido e não pode sofrer alterações*");
    }

    [Fact]
    public void Deve_validar_carga_horaria_ao_atualizar_quando_ainda_nao_emitido()
    {
        var c = new CertificadoBuilder().Build();

        c.Invoking(x => x.AtualizarCargaHoraria(0))
         .Should().Throw<DomainException>()
         .Which.Errors.Should().Contain(e => e.Contains("Carga horária deve ser maior que zero"));

        c.Invoking(x => x.AtualizarCargaHoraria(10001))
         .Should().Throw<DomainException>()
         .Which.Errors.Should().Contain(e => e.Contains("Carga horária deve estar entre 1 e 10.000 horas"));

        c.Invoking(x => x.AtualizarCargaHoraria(120))
         .Should().NotThrow();

        c.CargaHoraria.Should().Be(120);
    }

    [Fact]
    public void Deve_validar_nota_final_ao_atualizar_quando_ainda_nao_emitido()
    {
        var c = new CertificadoBuilder().Build();

        c.Invoking(x => x.AtualizarNotaFinal(255)) // > 10
         .Should().Throw<DomainException>()
         .Which.Errors.Should().Contain(e => e.Contains("Nota final deve estar entre 0 e 10"));

        c.Invoking(x => x.AtualizarNotaFinal(9))
         .Should().NotThrow();

        c.NotaFinal.Should().Be(9);
    }

    [Fact]
    public void Deve_validar_path_ao_atualizar_quando_ainda_nao_emitido()
    {
        var c = new CertificadoBuilder().Build();

        c.Invoking(x => x.AtualizarPathCertificado(""))
         .Should().Throw<DomainException>()
         .Which.Errors.Should().Contain(e => e.Contains("Path do certificado não pode ser nulo ou vazio"));

        var pathLongo = new string('p', 1025);
        c.Invoking(x => x.AtualizarPathCertificado(pathLongo))
         .Should().Throw<DomainException>()
         .Which.Errors.Should().Contain(e => e.Contains("Path do certificado deve ter no máximo 1024 caracteres"));

        c.Invoking(x => x.AtualizarPathCertificado("novo/cert.pdf"))
         .Should().NotThrow();

        c.PathCertificado.Should().Be("novo/cert.pdf");
    }

    [Fact]
    public void Deve_validar_instrutor_ao_atualizar_quando_ainda_nao_emitido()
    {
        var c = new CertificadoBuilder().Build();

        c.Invoking(x => x.AtualizarNomeInstrutor(""))
         .Should().Throw<DomainException>()
         .Which.Errors.Should().Contain(e => e.Contains("Nome do instrutor não pode ser nulo ou vazio"));

        var longonome = new string('x', 101);
        c.Invoking(x => x.AtualizarNomeInstrutor(longonome))
         .Should().Throw<DomainException>()
         .Which.Errors.Should().Contain(e => e.Contains("Nome do instrutor deve ter no máximo 100 caracteres"));

        c.Invoking(x => x.AtualizarNomeInstrutor("Tio Bob"))
         .Should().NotThrow();

        c.NomeInstrutor.Should().Be("Tio Bob");
    }

    // ---------- ToString ----------
    [Fact]
    public void ToString_deve_conter_campos_chave()
    {
        var c = new CertificadoBuilder()
            .ComMatriculaId(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"))
            .ComNomeCurso("Clean Architecture")
            .ComCargaHoraria(60)
            .ComNotaFinal(9.5m)
            .ComDataSolicitacao(new DateTime(2025, 03, 15))
            .Build();

        var s = c.ToString();
        s.Should().Contain("Clean Architecture");
        s.Should().Contain("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        s.Should().Contain("60");
        s.Should().Contain("9,5");
        s.Should().Contain("15/03/2025");
    }
}
