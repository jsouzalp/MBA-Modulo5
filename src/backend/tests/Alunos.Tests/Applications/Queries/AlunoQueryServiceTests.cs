using Alunos.Application.Queries;
using Alunos.Domain.Entities;
using Alunos.Domain.Interfaces;
using Alunos.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Alunos.Tests.Applications.Queries;

public class AlunoQueryServiceTests
{
    private static Aluno NovoAlunoAtivo()
    {
        var a = new Aluno(
            Guid.NewGuid(),
            "Fulano",
            "f@e.com",
            "12345678909",
            new DateTime(1990, 1, 1),
            "M",
            "SP",
            "SP",
            "01001000",
            "foto.png");
        a.AtivarAluno();
        return a;
    }

    [Fact]
    public async Task ObterAlunoPorIdAsync_deve_retornar_null_quando_nao_encontrado()
    {
        var repo = new Mock<IAlunoRepository>();
        repo.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>(), true)).ReturnsAsync((Aluno?)null);

        var sut = new AlunoQueryService(repo.Object);

        var dto = await sut.ObterAlunoPorIdAsync(Guid.NewGuid());
        dto.Should().BeNull();
    }

    [Fact]
    public async Task ObterAlunoPorIdAsync_deve_mapear_props_basicas_e_matriculas()
    {
        var repo = new Mock<IAlunoRepository>();
        var aluno = NovoAlunoAtivo();
        var matricula = aluno.MatricularAlunoEmCurso(Guid.NewGuid(), "Curso avançado de DDD", 100m, "obs");

        repo.Setup(r => r.ObterPorIdAsync(aluno.Id, true)).ReturnsAsync(aluno);

        var sut = new AlunoQueryService(repo.Object);

        var dto = await sut.ObterAlunoPorIdAsync(aluno.Id);

        dto.Should().NotBeNull();
        dto!.Id.Should().Be(aluno.Id);
        dto.Nome.Should().Be("Fulano");
        dto.Email.Should().Be("f@e.com");
        dto.Cpf.Should().Be("12345678909");
        dto.MatriculasCursos.Should().HaveCount(1);
        dto.MatriculasCursos!.Single().CursoId.Should().Be(matricula.CursoId);
        dto.MatriculasCursos.Single().NomeCurso.Should().Be("Curso avançado de DDD");
        // NotaFinal/Estado/Certificado dependem de regras internas; aqui checamos apenas que não explode e que há valores coerentes
    }

    [Fact]
    public async Task ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync_deve_retornar_null_quando_nao_encontrado()
    {
        var repo = new Mock<IAlunoRepository>();
        repo.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>(), true)).ReturnsAsync((Aluno?)null);

        var sut = new AlunoQueryService(repo.Object);

        (await sut.ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(Guid.NewGuid())).Should().BeNull();
    }

    [Fact]
    public async Task ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync_deve_mapear_e_contar_zero_sem_historico()
    {
        var repo = new Mock<IAlunoRepository>();
        var aluno = NovoAlunoAtivo();
        aluno.MatricularAlunoEmCurso(Guid.NewGuid(), "Curso avançado de DDD", 200m, "obs");
        repo.Setup(r => r.ObterPorIdAsync(aluno.Id, true)).ReturnsAsync(aluno);

        var sut = new AlunoQueryService(repo.Object);

        var evo = await sut.ObterEvolucaoMatriculasCursoDoAlunoPorIdAsync(aluno.Id);

        evo.Should().NotBeNull();
        evo!.MatriculasCursos.Should().HaveCount(1);
        var m = evo.MatriculasCursos!.Single();
        m.QuantidadeAulasRealizadas.Should().Be(0);
        m.QuantidadeAulasEmAndamento.Should().Be(0);
    }

    [Fact]
    public async Task ObterMatriculasPorAlunoIdAsync_deve_retornar_vazio_quando_aluno_nao_encontrado()
    {
        var repo = new Mock<IAlunoRepository>();
        repo.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>(), true)).ReturnsAsync((Aluno?)null);

        var sut = new AlunoQueryService(repo.Object);

        var lista = await sut.ObterMatriculasPorAlunoIdAsync(Guid.NewGuid());
        lista.Should().BeEmpty();
    }

    [Fact]
    public async Task ObterMatriculasPorAlunoIdAsync_deve_retornar_lista_mapeada()
    {
        var repo = new Mock<IAlunoRepository>();
        var aluno = NovoAlunoAtivo();
        var matricula = aluno.MatricularAlunoEmCurso(Guid.NewGuid(), "Algoritmos", 300m, "obs");
        repo.Setup(r => r.ObterPorIdAsync(aluno.Id, true)).ReturnsAsync(aluno);

        var sut = new AlunoQueryService(repo.Object);

        var lista = (await sut.ObterMatriculasPorAlunoIdAsync(aluno.Id)).ToList();

        lista.Should().HaveCount(1);
        lista[0].NomeCurso.Should().Be("Algoritmos");
        lista[0].CursoId.Should().Be(matricula.CursoId);
    }

    [Fact]
    public async Task ObterInformacaoMatriculaCursoAsync_deve_retornar_null_quando_matricula_nao_encontrada()
    {
        var repo = new Mock<IAlunoRepository>();
        repo.Setup(r => r.ObterMatriculaPorIdAsync(It.IsAny<Guid>(), true)).ReturnsAsync((MatriculaCurso?)null);

        var sut = new AlunoQueryService(repo.Object);

        (await sut.ObterInformacaoMatriculaCursoAsync(Guid.NewGuid())).Should().BeNull();
    }

    [Fact]
    public async Task ObterInformacaoMatriculaCursoAsync_deve_mapear_campos_basicos()
    {
        var repo = new Mock<IAlunoRepository>();
        var aluno = NovoAlunoAtivo();
        var matricula = aluno.MatricularAlunoEmCurso(Guid.NewGuid(), "Curso avançado de DDD", 150m, "obs");

        repo.Setup(r => r.ObterMatriculaPorIdAsync(matricula.Id, true)).ReturnsAsync(matricula);

        var sut = new AlunoQueryService(repo.Object);
        var dto = await sut.ObterInformacaoMatriculaCursoAsync(matricula.Id);

        dto.Should().NotBeNull();
        dto!.Id.Should().Be(matricula.Id);
        dto.NomeCurso.Should().Be("Curso avançado de DDD");
        dto.AlunoId.Should().Be(aluno.Id);
        dto.Certificado.Should().BeNull(); // sem certificado solicitado
    }

    [Fact]
    public async Task ObterCertificadoPorMatriculaIdAsync_deve_retornar_null_quando_sem_matricula_ou_sem_certificado()
    {
        var repo = new Mock<IAlunoRepository>();
        var sut = new AlunoQueryService(repo.Object);

        // sem matrícula
        repo.Setup(r => r.ObterMatriculaPorIdAsync(It.IsAny<Guid>(), true)).ReturnsAsync((MatriculaCurso?)null);
        (await sut.ObterCertificadoPorMatriculaIdAsync(Guid.NewGuid())).Should().BeNull();

        // com matrícula mas sem certificado
        var aluno = NovoAlunoAtivo();
        var m = aluno.MatricularAlunoEmCurso(Guid.NewGuid(), "Curso avançado de DDD", 100, "obs");
        repo.Setup(r => r.ObterMatriculaPorIdAsync(m.Id, true)).ReturnsAsync(m);

        (await sut.ObterCertificadoPorMatriculaIdAsync(m.Id)).Should().BeNull();
    }

    [Fact]
    public async Task ObterAulasPorMatriculaIdAsync_deve_retornar_null_quando_matricula_nao_encontrada()
    {
        var repo = new Mock<IAlunoRepository>();
        repo.Setup(r => r.ObterMatriculaPorIdAsync(It.IsAny<Guid>(), true)).ReturnsAsync((MatriculaCurso?)null);

        var sut = new AlunoQueryService(repo.Object);

        (await sut.ObterAulasPorMatriculaIdAsync(Guid.NewGuid())).Should().BeNull();
    }

    [Fact]
    public async Task ObterMatriculaPorIdAsync_deve_retornar_null_quando_aluno_nao_encontrado_ou_quando_matricula_nao_pertence_ao_aluno()
    {
        var repo = new Mock<IAlunoRepository>();
        var sut = new AlunoQueryService(repo.Object);

        // aluno não encontrado
        repo.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>(), true)).ReturnsAsync((Aluno?)null);
        (await sut.ObterMatriculaPorIdAsync(Guid.NewGuid(), Guid.NewGuid())).Should().BeNull();

        // aluno encontrado mas matrícula de outro aluno
        var aluno = NovoAlunoAtivo();
        var outroAluno = NovoAlunoAtivo();
        var matOutro = outroAluno.MatricularAlunoEmCurso(Guid.NewGuid(), "Curso avançado de Algoritmo", 1, "o");
        repo.Setup(r => r.ObterPorIdAsync(aluno.Id, true)).ReturnsAsync(aluno);
        repo.Setup(r => r.ObterMatriculaPorIdAsync(matOutro.Id, true)).ReturnsAsync(matOutro);

        (await sut.ObterMatriculaPorIdAsync(matOutro.Id, aluno.Id)).Should().BeNull();
    }

    [Fact]
    public async Task ObterMatriculaPorIdAsync_deve_retornar_dto_quando_ok()
    {
        var repo = new Mock<IAlunoRepository>();
        var aluno = NovoAlunoAtivo();
        var m = aluno.MatricularAlunoEmCurso(Guid.NewGuid(), "Curso avançado de XUnit", 500m, "obs");

        repo.Setup(r => r.ObterPorIdAsync(aluno.Id, true)).ReturnsAsync(aluno);
        repo.Setup(r => r.ObterMatriculaPorIdAsync(m.Id, true)).ReturnsAsync(m);

        var sut = new AlunoQueryService(repo.Object);

        var dto = await sut.ObterMatriculaPorIdAsync(m.Id, aluno.Id);

        dto.Should().NotBeNull();
        dto!.Id.Should().Be(m.Id);
        dto.NomeCurso.Should().Be("Curso avançado de XUnit");
        dto.AlunoId.Should().Be(aluno.Id);
    }

    [Fact]
    public async Task ObterCertificadoPorMatriculaIdAsync_deve_retornar_dto_quando_certificado_existe()
    {
        var repo = new Mock<IAlunoRepository>();
        var sut = new AlunoQueryService(repo.Object);

        var aluno = NovoAlunoAtivo();
        var matricula = aluno.MatricularAlunoEmCurso(Guid.NewGuid(), "Curso oficial de DDD", 100m, "obs");

        matricula.RegistrarPagamentoMatricula();
        matricula.RegistrarHistoricoAprendizado(Guid.NewGuid(), "Módulo 1", 10, DateTime.UtcNow.Date);
        matricula.ConcluirCurso();
        matricula.RequisitarCertificadoConclusao(matricula.CalcularMediaFinalCurso(), "/certificados/abc.pdf", "Instrutor X");
        var certificado = matricula.Certificado;

        repo.Setup(r => r.ObterMatriculaPorIdAsync(matricula.Id, true))
            .ReturnsAsync(matricula);

        var dto = await sut.ObterCertificadoPorMatriculaIdAsync(matricula.Id);

        dto.Should().NotBeNull();
        dto!.Id.Should().Be(certificado.Id);
        dto.MatriculaCursoId.Should().Be(certificado.MatriculaCursoId);
        dto.NomeCurso.Should().Be("Curso oficial de DDD");
        dto.PathCertificado.Should().Be("/certificados/abc.pdf");
        dto.NomeInstrutor.Should().Be("Instrutor X");
    }

    [Fact]
    public async Task ObterAulasPorMatriculaIdAsync_deve_retornar_lista_quando_historico_existe()
    {
        var repo = new Mock<IAlunoRepository>();
        var sut = new AlunoQueryService(repo.Object);

        var aluno = NovoAlunoAtivo();
        var matricula = aluno.MatricularAlunoEmCurso(Guid.NewGuid(), "Algoritmos Avançados", 100m, "obs");
        matricula.RegistrarPagamentoMatricula();
        matricula.RegistrarHistoricoAprendizado(Guid.NewGuid(), "Módulo 1", 10, DateTime.UtcNow.Date);
        matricula.RegistrarHistoricoAprendizado(Guid.NewGuid(), "Módulo 2", 10, DateTime.UtcNow.Date);
        matricula.RegistrarHistoricoAprendizado(Guid.NewGuid(), "Módulo 3", 10, DateTime.UtcNow.Date);

        repo.Setup(r => r.ObterMatriculaPorIdAsync(matricula.Id, true))
            .ReturnsAsync(matricula);

        var aulas = (await sut.ObterAulasPorMatriculaIdAsync(matricula.Id))!.ToList();

        aulas.Should().HaveCount(3);
        aulas.Select(a => a.NomeAula).Should().Contain(new[] { "Módulo 1", "Módulo 2", "Módulo 3" });
        aulas.All(a => a.CursoId == matricula.CursoId).Should().BeTrue();
    }

    [Fact]
    public async Task ObterCertificadosPorAlunoIdAsync_deve_filtrar_por_path_e_ordenar_por_data_emissao_desc()
    {
        var repo = new Mock<IAlunoRepository>();
        var sut = new AlunoQueryService(repo.Object);

        var aluno = NovoAlunoAtivo();

        var m1 = aluno.MatricularAlunoEmCurso(Guid.NewGuid(), "Curso Algoritimos", 100m, "obs");
        m1.RegistrarPagamentoMatricula();
        m1.RegistrarHistoricoAprendizado(Guid.NewGuid(), "Módulo 1", 10, DateTime.UtcNow.Date);
        m1.RegistrarHistoricoAprendizado(Guid.NewGuid(), "Módulo 2", 10, DateTime.UtcNow.Date);
        m1.ConcluirCurso();
        m1.RequisitarCertificadoConclusao(m1.CalcularMediaFinalCurso(), "/certificados/abc.pdf", "Instrutor X");

        var m2 = aluno.MatricularAlunoEmCurso(Guid.NewGuid(), "Curso Binários", 100m, "obs");
        m2.RegistrarPagamentoMatricula();
        m2.RegistrarHistoricoAprendizado(Guid.NewGuid(), "Módulo 1", 10, DateTime.UtcNow);
        m2.RegistrarHistoricoAprendizado(Guid.NewGuid(), "Módulo 2", 10, DateTime.UtcNow);
        m2.ConcluirCurso();
        m2.RequisitarCertificadoConclusao(m2.CalcularMediaFinalCurso(), "/certificados/def.pdf", "Instrutor X");

        var m3 = aluno.MatricularAlunoEmCurso(Guid.NewGuid(), "Curso Curvas", 100m, "obs");


        repo.Setup(r => r.ObterPorIdAsync(aluno.Id, true))
            .ReturnsAsync(aluno);

        var lista = (await sut.ObterCertificadosPorAlunoIdAsync(aluno.Id)).ToList();

        // só 2 entram (path obrigatório)
        lista.Should().HaveCount(2);

        // ordenado desc por DataEmissao
        lista[0].NomeCurso.Should().Be("Curso Binários");
        lista[1].NomeCurso.Should().Be("Curso Algoritimos");

        // url mapeada do path
        lista[0].Url.Should().Be("/certificados/def.pdf");

        // código gerado (8 chars)
        lista[0].Codigo.Should().NotBeNullOrWhiteSpace();
        lista[0].Codigo.Length.Should().Be(8);
    }

}
