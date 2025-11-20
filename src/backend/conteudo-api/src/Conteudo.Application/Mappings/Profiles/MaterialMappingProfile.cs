using Conteudo.Application.Commands.AtualizarMaterial;
using Conteudo.Application.Commands.CadastrarMaterial;
using Conteudo.Application.DTOs;
using Conteudo.Domain.Entities;
using Core.SharedDtos.Conteudo;
using Mapster;

namespace Conteudo.Application.Mappings.Profiles
{
    public class MaterialMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CadastroMaterialDto, CadastrarMaterialCommand>()
                .ConstructUsing(x => new CadastrarMaterialCommand(
                    x.CursoId,
                    x.AulaId,
                    x.Nome,
                    x.Descricao,
                    x.TipoMaterial,
                    x.Url,
                    x.IsObrigatorio,
                    x.TamanhoBytes,
                    x.Extensao,
                    x.Ordem));

            config.NewConfig<AtualizarMaterialDto, AtualizarMaterialCommand>()
                .ConstructUsing(x => new AtualizarMaterialCommand(
                    x.CursoId,
                    x.Id,
                    x.Nome,
                    x.Descricao,
                    x.TipoMaterial,
                    x.Url,
                    x.IsObrigatorio,
                    x.TamanhoBytes,
                    x.Extensao,
                    x.Ordem));

            config.NewConfig<Material, MaterialDto>();
        }
    }
}
