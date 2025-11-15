using Conteudo.Application.Commands.AtualizarAula;
using Conteudo.Application.Commands.CadastrarAula;
using Conteudo.Application.DTOs;
using Conteudo.Domain.Entities;
using Core.SharedDtos.Conteudo;
using Mapster;

namespace Conteudo.Application.Mappings.Profiles
{
    public class AulaMappingProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AulaDto, Aula>()
                .ConstructUsing(x => new Aula(
                    x.CursoId,
                    x.Nome,
                    x.Descricao,
                    x.Numero,
                    x.DuracaoMinutos,
                    x.VideoUrl,
                    x.TipoAula,
                    x.IsObrigatoria,
                    x.Observacoes));

            config.NewConfig<CadastroAulaDto, CadastrarAulaCommand>()
                .ConstructUsing(x => new CadastrarAulaCommand(
                    x.CursoId,
                    x.Nome,
                    x.Descricao,
                    x.Numero,
                    x.DuracaoMinutos,
                    x.VideoUrl,
                    x.TipoAula,
                    x.IsObrigatoria,
                    x.Observacoes));

            config.NewConfig<AtualizarAulaDto, AtualizarAulaCommand>()
                .ConstructUsing(x => new AtualizarAulaCommand(
                    x.Id,
                    x.CursoId,
                    x.Nome,
                    x.Descricao,
                    x.Numero,
                    x.DuracaoMinutos,
                    x.VideoUrl,
                    x.TipoAula,
                    x.IsObrigatoria,
                    x.Observacoes));

            config.NewConfig<Aula, AulaDto>()
                .PreserveReference(true);
        }
    }
}
