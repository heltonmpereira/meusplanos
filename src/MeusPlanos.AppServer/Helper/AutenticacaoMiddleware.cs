using System.Text;
using MeusPlanos.Definicao.Interface.Servico;
using MeusPlanos.Servico.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MeusPlanos.AppServer.Helper;

public static class AutenticacaoMiddleware
{
    public static IServiceCollection AdicionarAutenticacao(
        this IServiceCollection services)
    {
        var key = Encoding.ASCII.GetBytes(TokenServico.Segredo);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuers = ["MeusPlanos"],
                ValidateAudience = true,
                ValidAudiences = ["MeusPlanos"],
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            // Usar eventos customizados para validar versão do usuário
            var serviceProvider = services.BuildServiceProvider();
            var usuarioServico = serviceProvider.GetRequiredService<IUsuarioServico>();
            options.Events = new CustomJwtBearerEvents(usuarioServico);
        });

        services.AddCors();

        return services;
    }
}