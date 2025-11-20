using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BFF.API.Models.Request;

[ExcludeFromCodeCoverage]
public class AulaAtualizarRequest : AulaCriarRequest
{
    [Required(ErrorMessage = "ID da aula é obrigatório")]
    public Guid Id { get; set; }
}
