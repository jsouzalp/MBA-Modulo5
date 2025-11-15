using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

namespace BFF.API.Handlers;

[ExcludeFromCodeCoverage]
public class AuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authorization = httpContextAccessor.HttpContext?.Request?.Headers["Authorization"].FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(authorization))
        {
            if (request.Headers.Contains("Authorization"))
            {
                request.Headers.Remove("Authorization");
            }

            request.Headers.Add("Authorization", authorization);
        }

        if (!request.Headers.Accept.Any())
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        return base.SendAsync(request, cancellationToken);
    }
}
