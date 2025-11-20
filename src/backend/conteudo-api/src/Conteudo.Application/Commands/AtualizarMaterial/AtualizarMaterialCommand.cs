using Core.Communication;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.AtualizarMaterial
{
    public class AtualizarMaterialCommand : RaizCommand, IRequest<CommandResult>
    {
        public Guid CursoId { get; private set; }
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public string TipoMaterial { get; private set; }
        public string Url { get; private set; }
        public bool IsObrigatorio { get; private set; }
        public long TamanhoBytes { get; private set; }
        public string Extensao { get; private set; }
        public int Ordem { get; private set; }

        public AtualizarMaterialCommand(
            Guid cursoId,
            Guid id,
            string nome,
            string descricao,
            string tipoMaterial,
            string url,
            bool isObrigatorio,
            long tamanhoBytes,
            string extensao,
            int ordem)
        {
            CursoId = cursoId;
            Id = id;
            Nome = nome;
            Descricao = descricao;
            TipoMaterial = tipoMaterial;
            Url = url;
            IsObrigatorio = isObrigatorio;
            TamanhoBytes = tamanhoBytes;
            Extensao = extensao;
            Ordem = ordem;

            DefinirRaizAgregacao(Id);
        }
    }
}
