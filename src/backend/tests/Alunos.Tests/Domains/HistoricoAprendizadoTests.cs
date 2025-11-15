using FluentAssertions;
using Plataforma.Educacao.Core.Exceptions;

namespace Alunos.Tests.Domains;
public class HistoricoAprendizadoTests
{
    // ---------- Happy path ----------
    [Fact]
    public void Deve_criar_historico_valido()
    {
        var h = new HistoricoAprendizadoBuilder().Build();

        h.Id.Should().NotBeEmpty();
        h.NomeAula.Should().Be("Introdução ao DDD");
        h.CargaHoraria.Should().Be(10);
        h.DataInicio.Should().Be(new DateTime(2025, 01, 10));
        h.DataTermino.Should().BeNull();
    }

    // ---------- Guid inválido ----------
    [Theory]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(false, false, true)]
    public void Deve_falhar_quando_algum_guid_for_vazio(bool zeraMatricula, bool zeraCurso, bool zeraAula)
    {
        var builder = new HistoricoAprendizadoBuilder()
            .ComMatriculaId(zeraMatricula ? Guid.Empty : Guid.NewGuid())
            .ComCursoId(zeraCurso ? Guid.Empty : Guid.NewGuid())
            .ComAulaId(zeraAula ? Guid.Empty : Guid.NewGuid());

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().NotBeEmpty();
    }

    // ---------- NomeAula ----------
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Deve_falhar_quando_nome_aula_vazio(string nome)
    {
        var builder = new HistoricoAprendizadoBuilder().ComNomeAula(nome);

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Nome da aula não pode ser vazio"));
    }

    [Theory]
    [InlineData("abcd")] // < 5
    [InlineData("a")]
    public void Deve_falhar_quando_nome_aula_curto_demais(string nome)
    {
        var builder = new HistoricoAprendizadoBuilder().ComNomeAula(nome);

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Nome da aula deve ter entre 5 e 100 caracteres"));
    }

    [Fact]
    public void Deve_falhar_quando_nome_aula_maior_que_100()
    {
        var builder = new HistoricoAprendizadoBuilder().ComNomeAula(new string('x', 101));

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Nome da aula deve ter entre 5 e 100 caracteres"));
    }

    // ---------- Carga horária ----------
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Deve_falhar_quando_carga_horaria_invalida(int carga)
    {
        var builder = new HistoricoAprendizadoBuilder().ComCargaHoraria(carga);

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Carga horária deve ser maior que zero"));
    }

    [Theory]
    [InlineData(201)]
    [InlineData(999)]
    public void Deve_falhar_quando_carga_horaria_fora_do_limite(int carga)
    {
        var builder = new HistoricoAprendizadoBuilder().ComCargaHoraria(carga);

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Carga horária deve estar entre 1 e 200 horas"));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(200)]
    public void Deve_aceitar_limites_validos_de_carga_horaria(int carga)
    {
        var h = new HistoricoAprendizadoBuilder().ComCargaHoraria(carga).Build();
        h.CargaHoraria.Should().Be(carga);
    }

    // ---------- Data de início ----------
    [Fact]
    public void Deve_falhar_quando_data_inicio_for_futura()
    {
        var builder = new HistoricoAprendizadoBuilder().ComDataInicio(DateTime.Now.AddDays(1));

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Data de início não pode ser superior à data atual"));
    }

    [Theory]
    [InlineData("0001-01-01")]
    [InlineData("9999-12-31")]
    public void Deve_falhar_quando_data_inicio_invalida(string data)
    {
        var builder = new HistoricoAprendizadoBuilder().ComDataInicio(DateTime.Parse(data));

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Data de início"));
    }

    // ---------- Data de término ----------
    [Fact]
    public void Deve_falhar_quando_data_termino_menor_que_inicio()
    {
        var inicio = new DateTime(2025, 01, 10);
        var termino = inicio.AddDays(-1);

        var builder = new HistoricoAprendizadoBuilder()
            .ComDataInicio(inicio)
            .ComDataTermino(termino);

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Data de término não pode ser menor que a data de início"));
    }

    [Fact]
    public void Deve_falhar_quando_data_termino_futura()
    {
        var builder = new HistoricoAprendizadoBuilder().ComDataTermino(DateTime.Now.AddDays(1));

        Action act = () => builder.Build();

        act.Should().Throw<DomainException>()
           .Which.Errors.Should().Contain(e => e.Contains("Data de término não pode ser superior à data atual"));
    }

    [Fact]
    public void Deve_aceitar_data_termino_igual_ao_inicio()
    {
        var dia = new DateTime(2025, 01, 10);
        var h = new HistoricoAprendizadoBuilder().ComDataInicio(dia).ComDataTermino(dia).Build();

        h.DataTermino.Should().Be(dia);
    }

    // ---------- ToString ----------
    [Fact]
    public void ToString_deve_indicar_em_andamento()
    {
        var h = new HistoricoAprendizadoBuilder().Build();
        h.ToString().Should().Contain("Em andamento");
    }

    [Fact]
    public void ToString_deve_incluir_data_termino_quando_existir()
    {
        var dia = new DateTime(2025, 02, 05);
        var h = new HistoricoAprendizadoBuilder().ComDataTermino(dia).Build();

        h.ToString().Should().Contain("05/02/2025");
    }
}
