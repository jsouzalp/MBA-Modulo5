using Auth.API.Models.Requests;
using Core.Messages.Integration;
using Mapster;
using System.Diagnostics.CodeAnalysis;

namespace Auth.API.Configuration;

[ExcludeFromCodeCoverage]
public class AuthApiMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegistroRequest, AlunoRegistradoIntegrationEvent>()
            .ConstructUsing(src => new AlunoRegistradoIntegrationEvent(
                Guid.Empty,
                src.Nome,
                src.Email,
                src.CPF,
                src.DataNascimento,
                src.Telefone,
                src.Genero,
                src.Cidade,
                src.Estado,
                src.CEP,
                src.Foto
            ));
    }
}
