using Core.Communication;
using Core.Messages;
using MediatR;

namespace Conteudo.Application.Commands.AtualizarAula
{
    public class AtualizarAulaCommand : RaizCommand, IRequest<CommandResult>
    {
        public Guid Id { get; private set; }
        public Guid CursoId { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public int Numero { get; private set; }
        public int DuracaoMinutos { get; private set; }
        public string VideoUrl { get; private set; }
        public string TipoAula { get; private set; }
        public bool IsObrigatoria { get; private set; }
        public string Observacoes { get; private set; }

        public AtualizarAulaCommand(
            Guid id,
            Guid cursoId,
            string nome,
            string descricao,
            int numero,
            int duracaoMinutos,
            string videoUrl,
            string tipoAula,
            bool isObrigatoria,
            string observacoes)
        {
            Id = id;
            CursoId = cursoId;
            Nome = nome;
            Descricao = descricao;
            Numero = numero;
            DuracaoMinutos = duracaoMinutos;
            VideoUrl = videoUrl;
            TipoAula = tipoAula;
            IsObrigatoria = isObrigatoria;
            Observacoes = observacoes;

            DefinirRaizAgregacao(Id);
        }
    }
}
