using Core.Communication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Plataforma.Educacao.Core.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Alunos.API.Filters;

[ExcludeFromCodeCoverage]
public class DomainExceptionFilter(IActionResultExecutor<ObjectResult> executor, ILogger<ExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is DomainException ex)
        {
            context.ExceptionHandled = true;
            string erro = $"Erro de DOMINIO: {context?.Exception?.Message ?? context.Exception?.ToString()}";
            logger.LogError(context?.Exception, erro);

            var outputResponse = new
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Ocorreu um ou mais erros durante a validação das informações",
                Errors = new ResponseErrorMessages { Mensagens = [erro] }
            };

            ObjectResult output = new(outputResponse)
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Value = outputResponse
            };

            executor.ExecuteAsync(new ActionContext(context.HttpContext, context.RouteData, context.ActionDescriptor), output)
                .GetAwaiter()
                .GetResult();
        }
    }
}
