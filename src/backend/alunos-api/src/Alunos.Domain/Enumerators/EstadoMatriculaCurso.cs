using System.ComponentModel;

namespace Alunos.Domain.Enumerators
{
    public enum EstadoMatriculaCurso
    {
        [Description("Pendente de pagamento")]
        PendentePagamento = 1,

        [Description("Pagamento realizado")]
        PagamentoRealizado = 2,

        [Description("Pagamento não concluído")]
        Abandonado = 3,

        [Description("Curso concluído")]
        Concluido = 4
    }
}
