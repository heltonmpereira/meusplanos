using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using MeusPlanos.Modelo.Data;
using MeusPlanos.Modelo.Repositorio;
using MeusPlanos.Servico.Servico;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MeusPlanos.AppServer.Helper;

public static class ServidorMiddleware
{
    public static IServiceCollection AdicionarDados(
        this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<IDbContext, MeusPlanosContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<ILoginServico, LoginServico>();

        //TODO Adicionar os mapeamentos das classes dos projetos
        //seguindo o modelo dos mapeamentos já adicionadas:

        services.AddScoped<IPapelRepositorio, PapelRepositorio>();
        services.AddScoped<IPapelServico, PapelServico>();

        services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
        services.AddScoped<IUsuarioServico, UsuarioServico>();

        return services;
    }
}