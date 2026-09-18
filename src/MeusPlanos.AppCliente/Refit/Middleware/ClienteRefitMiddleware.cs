using System;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace MeusPlanos.AppCliente.Refit.Middleware;

public static class ClienteRefitMiddleware
{
    public static IServiceCollection AdicionarConexoesRefit(
        this IServiceCollection services, string baseUrlApi)
    {
        if (baseUrlApi.EndsWith('/'))
            baseUrlApi = baseUrlApi.Remove(baseUrlApi.Length - 1);

        //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<AuthorizationMessageHandler>();

        //var primaryHttpMessageHandler = new HttpClientHandler
        //{
        //    ServerCertificateCustomValidationCallback =
        //        (message, cert, chain, sslErrors) => true
        //};

        services
            .AddRefitClient<ILoginRefit>()
            .AddHttpMessageHandler<AuthorizationMessageHandler>()
            .ConfigureHttpClient(c => c.BaseAddress =
                new Uri($"{baseUrlApi}/login"));

        services
            .AddRefitClient<IUsuarioRefit>()
            .AddHttpMessageHandler<AuthorizationMessageHandler>()
            .ConfigureHttpClient(c => c.BaseAddress =
                new Uri($"{baseUrlApi}/Usuario"));

        services
            .AddRefitClient<IPapelRefit>()
            .AddHttpMessageHandler<AuthorizationMessageHandler>()
            .ConfigureHttpClient(c => c.BaseAddress =
                new Uri($"{baseUrlApi}/Papel"));

        return services;
    }
}