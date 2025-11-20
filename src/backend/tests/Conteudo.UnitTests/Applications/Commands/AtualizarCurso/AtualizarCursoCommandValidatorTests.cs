using Conteudo.Application.Commands.AtualizarCurso;

namespace Conteudo.UnitTests.Applications.Commands.AtualizarCurso;
public class AtualizarCursoCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_campos_obrigatorios()
    {
        var cmd = new AtualizarCursoCommand
        {
            Id = Guid.Empty,
            Nome = "",
            Valor = -1,
            DuracaoHoras = 0,
            Nivel = "",
            Instrutor = "",
            VagasMaximas = 0,
            Resumo = "",
            Descricao = "",
            Objetivos = ""
        };

        var result = new AtualizarCursoCommandValidator().Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_campos_ok()
    {
        var cmd = new AtualizarCursoCommand
        {
            Id = Guid.NewGuid(),
            Nome = "DDD",
            Valor = 10,
            DuracaoHoras = 8,
            Nivel = "Básico",
            Instrutor = "I",
            VagasMaximas = 10,
            Resumo = "r",
            Descricao = "d",
            Objetivos = "o"
        };

        new AtualizarCursoCommandValidator().Validate(cmd).IsValid.Should().BeTrue();
    }
}
