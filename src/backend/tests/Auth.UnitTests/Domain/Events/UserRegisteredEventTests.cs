using Auth.Domain.Events;

namespace Auth.UnitTests.Domain.Events;
public class UserRegisteredEventTests
{
    [Fact]
    public void Defaults_deve_ter_strings_vazias_foto_null_e_bools_false()
    {
        var e = new UserRegisteredEvent();

        e.UserId.Should().Be(string.Empty);
        e.Email.Should().Be(string.Empty);
        e.Nome.Should().Be(string.Empty);
        e.CPF.Should().Be(string.Empty);
        e.Telefone.Should().Be(string.Empty);
        e.Genero.Should().Be(string.Empty);
        e.Cidade.Should().Be(string.Empty);
        e.Estado.Should().Be(string.Empty);
        e.CEP.Should().Be(string.Empty);

        e.Foto.Should().BeNull();
        e.EhAdministrador.Should().BeFalse();

        // datas não têm valor padrão definido; apenas checamos que são atribuíveis
        e.DataCadastro.Should().Be(default);
        e.DataNascimento.Should().Be(default);
    }

    [Fact]
    public void Setters_devem_mapear_todos_os_campos()
    {
        var e = new UserRegisteredEvent
        {
            UserId = "u1",
            Email = "a@b.com",
            Nome = "Fulano",
            DataNascimento = new DateTime(1990, 1, 1),
            CPF = "12345678909",
            Telefone = "11",
            Genero = "M",
            Cidade = "SP",
            Estado = "SP",
            CEP = "01001000",
            Foto = "f.png",
            DataCadastro = new DateTime(2024, 1, 1),
            EhAdministrador = true
        };

        e.UserId.Should().Be("u1");
        e.Email.Should().Be("a@b.com");
        e.Nome.Should().Be("Fulano");
        e.DataNascimento.Should().Be(new DateTime(1990, 1, 1));
        e.CPF.Should().Be("12345678909");
        e.Telefone.Should().Be("11");
        e.Genero.Should().Be("M");
        e.Cidade.Should().Be("SP");
        e.Estado.Should().Be("SP");
        e.CEP.Should().Be("01001000");
        e.Foto.Should().Be("f.png");
        e.DataCadastro.Should().Be(new DateTime(2024, 1, 1));
        e.EhAdministrador.Should().BeTrue();
    }
}
