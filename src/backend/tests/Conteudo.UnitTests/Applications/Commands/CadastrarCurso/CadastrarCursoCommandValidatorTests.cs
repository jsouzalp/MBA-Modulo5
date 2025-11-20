using Conteudo.Application.Commands.CadastrarCurso;

namespace Conteudo.UnitTests.Applications.Commands.CadastrarCurso;
public class CadastrarCursoCommandValidatorTests
{
    [Fact]
    public void Deve_invalidar_campos_obrigatorios_e_formatos()
    {
        var cmd = new CadastrarCursoCommand
        {
            Nome = "",
            Valor = -1,
            DuracaoHoras = 0,
            Nivel = "",
            Instrutor = "",
            VagasMaximas = 0,
            ImagemUrl = "", // inválida: precisa ser URL absoluta
            Resumo = "",
            Descricao = "",
            Objetivos = ""
        };

        var result = new CadastrarCursoCommandValidator().Validate(cmd);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Deve_ser_valido_quando_campos_ok()
    {
        var cmd = new CadastrarCursoCommand
        {
            Nome = "DDD",
            Valor = 100,
            DuracaoHoras = 8,
            Nivel = "Básico",
            Instrutor = "Instrutor",
            VagasMaximas = 10,
            ImagemUrl = "https://cdn.exemplo.com/img.png",
            Resumo = "r",
            Descricao = "d",
            Objetivos = "o"
        };

        new CadastrarCursoCommandValidator().Validate(cmd).IsValid.Should().BeTrue();
    }
}
