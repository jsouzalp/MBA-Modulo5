using Conteudo.Application.Commands.CadastrarCategoria;
using Conteudo.Application.DTOs;
using Conteudo.Domain.Entities;
using Mapster;

namespace Conteudo.Application.Mappings.Profiles
{
    public class CategoriaMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CadastroCategoriaDto, CadastrarCategoriaCommand>().TwoWays();

            config.NewConfig<Categoria, CategoriaDto>()
                .Map(dest => dest.TotalCursos, src => src.Cursos.Count)
                .Map(dest => dest.CursosAtivos, src => src.Cursos.Count(c => c.Ativo))
                .PreserveReference(true);
        }
    }
}
