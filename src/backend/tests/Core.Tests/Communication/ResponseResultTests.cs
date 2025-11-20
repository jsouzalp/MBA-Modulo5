using Core.Communication;

namespace Core.Tests.Communication;
public class ResponseResultTests
{
    [Fact]
    public void Ctor_deve_inicializar_Errors_com_lista_vazia()
    {
        var rr = new ResponseResult<string>();

        rr.Errors.Should().NotBeNull();
        rr.Errors.Mensagens.Should().NotBeNull();
        rr.Errors.Mensagens.Should().BeEmpty();
    }

    [Fact]
    public void Deve_setar_Title_Status_Data_e_adicionar_erros()
    {
        var rr = new ResponseResult<object>
        {
            Title = "Bad Request",
            Status = 400,
            Data = new { ok = false }
        };
        rr.Errors.Mensagens.Add("campo X é obrigatório");

        rr.Title.Should().Be("Bad Request");
        rr.Status.Should().Be(400);
        rr.Data.Should().NotBeNull();
        rr.Errors.Mensagens.Should().ContainSingle().Which.Should().Contain("obrigatório");
    }
}
