using System.Text.Json;
using Anino.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Anino.Services;

public class MockServerBuilder : IMockServerBuilder
{
    public IAninoWebApplication BuildServer(List<ApiEndpoint> endpoints, int latencyMs = DefaultValueOf.Latency)
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        foreach (var endpoint in endpoints)
        {
            var httpMethod = endpoint.Method.ToUpperInvariant();

            app.MapMethods(endpoint.Path, new[] { httpMethod }, async (HttpContext context) =>
            {
                // Simulate network latency if specified
                if (latencyMs > DefaultValueOf.Latency)
                {
                    await Task.Delay(latencyMs).ConfigureAwait(false);
                }

                context.Response.StatusCode = endpoint.StatusCode;

                if (endpoint.Response.ValueKind != JsonValueKind.Null && 
                    endpoint.Response.ValueKind != JsonValueKind.Undefined)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(endpoint.Response.GetRawText()).ConfigureAwait(false);
                }
            });
        }

        return new AninoWebApplication(app);
    }
}