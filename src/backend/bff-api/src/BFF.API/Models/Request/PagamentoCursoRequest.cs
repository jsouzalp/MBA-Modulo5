using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BFF.API.Models.Request;

[ExcludeFromCodeCoverage]
public class PagamentoCursoInputModel
{
    [Required(ErrorMessage = "O Id do Matricula é obrigatório.")]
    public Guid MatriculaId { get; set; }

    [Required(ErrorMessage = "O Id do cliente é obrigatório.")]
    public Guid AlunoId { get; set; }

    [Required(ErrorMessage = "O Id do curso é obrigatório.")]
    public Guid CursoId { get; set; }

    [Required(ErrorMessage = "O valor total é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor total deve ser maior que zero.")]
    public decimal Total { get; set; }

    [Required(ErrorMessage = "O nome do cartão é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome do cartão deve ter no máximo 100 caracteres.")]
    public string NomeCartao { get; set; }

    [Required(ErrorMessage = "O número do cartão é obrigatório.")]
    [CreditCard(ErrorMessage = "O número do cartão de crédito não é válido.")]
    public string NumeroCartao { get; set; }

    [Required(ErrorMessage = "A data de expiração do cartão é obrigatória.")]
    [RegularExpression(@"^(0[1-9]|1[0-2])\/([0-9]{2})$", ErrorMessage = "A data de expiração deve estar no formato MM/AA.")]
    public string ExpiracaoCartao { get; set; }

    [Required(ErrorMessage = "O código de segurança do cartão é obrigatório.")]
    [StringLength(4, MinimumLength = 3, ErrorMessage = "O código de segurança (CVV) deve ter 3 ou 4 dígitos.")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "O código de segurança deve conter apenas números.")]
    public string CvvCartao { get; set; }
}
