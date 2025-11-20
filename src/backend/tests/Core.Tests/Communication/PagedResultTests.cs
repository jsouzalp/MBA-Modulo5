using Core.Communication;

namespace Core.Tests.Communication;
public class PagedResultTests
{
    [Fact]
    public void Defaults_deve_ter_Items_vazio()
    {
        var pr = new PagedResult<string>();
        pr.Items.Should().NotBeNull();
        pr.Items.Should().BeEmpty();
    }

    [Fact]
    public void Deve_preencher_campos_de_paginacao_e_itens()
    {
        var pr = new PagedResult<object>
        {
            PageIndex = 2,
            PageSize = 10,
            TotalResults = 42,
            Query = "abc",
            Items = new object[] { 7, 8, 9 }
        };

        pr.PageIndex.Should().Be(2);
        pr.PageSize.Should().Be(10);
        pr.TotalResults.Should().Be(42);
        pr.Query.Should().Be("abc");
        pr.Items.Should().BeEquivalentTo(new[] { 7, 8, 9 });
    }
}
