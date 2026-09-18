using System.IO;
using System.Reflection;
using MeusPlanos.AppServer;
using MeusPlanos.Teste.AppServer.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace MeusPlanos.Teste.AppServer.Base;

public class MeusPlanosWebApplicationFactory : WebApplicationFactory<Startup>
{
    public MeusPlanosWebApplicationFactory()
    {
        LocalDb.ApagarECriarLocalDb("MeusPlanosTesteDB");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(cfg =>
        {
            cfg.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            cfg.AddJsonFile("appSettingsTest.json", false);
        });
    }
}