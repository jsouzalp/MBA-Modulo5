using Core.Communication.Filters;

namespace Core.Tests.Communication;
public class CursoFilterTests
{
    [Fact]
    public void Defaults_devem_ser_PageSize_8_PageIndex_1_Ativos_true()
    {
        var f = new CursoFilter();

        f.PageSize.Should().Be(8);
        f.PageIndex.Should().Be(1);
        f.Ativos.Should().BeTrue();
        f.IncludeAulas.Should().BeFalse();
        f.Query.Should().BeNull();
    }

    [Fact]
    public void Setters_devem_atualizar_valores()
    {
        var f = new CursoFilter
        {
            PageSize = 20,
            PageIndex = 3,
            Ativos = false,
            IncludeAulas = true,
            Query = "c#"
        };

        f.PageSize.Should().Be(20);
        f.PageIndex.Should().Be(3);
        f.Ativos.Should().BeFalse();
        f.IncludeAulas.Should().BeTrue();
        f.Query.Should().Be("c#");
    }
}
