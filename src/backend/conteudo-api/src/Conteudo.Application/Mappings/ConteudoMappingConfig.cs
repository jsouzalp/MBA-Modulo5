using Conteudo.Application.Mappings.Profiles;
using Mapster;

namespace Conteudo.Application.Mappings;

public class ConteudoMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        new AulaMappingProfile().Register(config);
        new MaterialMappingProfile().Register(config);
        new CursoMappingProfile().Register(config);
        new CategoriaMappingProfile().Register(config);
    }
}
