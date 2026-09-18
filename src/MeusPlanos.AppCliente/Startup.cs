using System;
using System.Globalization;
using System.IO;
using System.Linq;
using MeusPlanos.AppCliente.Helper;
using MeusPlanos.AppCliente.Refit.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using WebEssentials.AspNetCore.Pwa;

namespace MeusPlanos.AppCliente;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;

    private static void ConfigureDataProtection(IServiceCollection services, IWebHostEnvironment environment)
    {
        var keysDirectoryName = "Keys";
        var keysDirectoryPath = Path.Combine(environment.ContentRootPath, keysDirectoryName);
        if (!Directory.Exists(keysDirectoryPath))
            Directory.CreateDirectory(keysDirectoryPath);

        services.AddDataProtection()
              .PersistKeysToFileSystem(new DirectoryInfo(keysDirectoryPath))
              .SetApplicationName("MeusPlanos");
    }

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        _configuration = configuration;
        _env = env;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        ConfigureDataProtection(services, _env);

        services
            .AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .AddRazorRuntimeCompilation();

        services.AddAntiforgery(opts =>
        {
            opts.HeaderName = "XSRF-TOKEN";
            opts.Cookie.Name = Constante.NOME_COOKIE_ANTIFORGERY;
            opts.Cookie.SecurePolicy = CookieSecurePolicy.Always;

        });

        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(opt =>
            {
                opt.Cookie.Name = Constante.NOME_COOKIE_ASPNET;
                opt.Cookie.SameSite = SameSiteMode.Strict;
                opt.ExpireTimeSpan = TimeSpan.FromDays(30);
                opt.SlidingExpiration = true;
                opt.LoginPath = new PathString("/login/index");
                opt.LogoutPath = new PathString("/login/logout");
                opt.AccessDeniedPath = new PathString("/login/naoautorizado");
            });

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        var options = new PwaOptions()
        {
            CacheId = "2.0.0",
            EnableCspNonce = true,
            Strategy = ServiceWorkerStrategy.NetworkFirst,
            //CustomServiceWorkerStrategyFileName = "js/service-worker.js",
            OfflineRoute = "/Offline.html",
            BaseRoute = _configuration.GetValue<string>("BaseRoutePwa"),
        };
        services.AddProgressiveWebApp(options);

        services.AdicionarConexoesRefit(_configuration["BaseUrlApi"]);

        // HSTS - configure in services and use in pipeline
        services.AddHsts(opts =>
        {
            opts.MaxAge = TimeSpan.FromDays(365);
            opts.IncludeSubDomains = true;
            opts.Preload = true;
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        var cultureInfo = new CultureInfo("pt-BR")
        {
            NumberFormat =
            {
                CurrencySymbol = "R$",
                CurrencyPositivePattern = 2
            }
        };

        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        app.UseCookiePolicy(new CookiePolicyOptions
        {
            HttpOnly = HttpOnlyPolicy.Always,
            Secure = CookieSecurePolicy.Always
        });

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        // Registered before static files to always set header
        app.UseHsts();
        app.UseXContentTypeOptions();
        app.UseReferrerPolicy(opts => opts.NoReferrer());

        app.UseHttpsRedirection();
        app.UseStaticFiles(new StaticFileOptions
        {
            //RequestPath = "/wwwroot",
            //FileProvider = new PhysicalFileProvider(
            //    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
        });

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        //Registered after static files, to set headers for dynamic content.
        app.UseNoCacheHttpHeaders();
        app.UseXfo(xfo => xfo.SameOrigin());
        app.UseXXssProtection(options => options.EnabledWithBlockMode());
        app.UseRedirectValidation(opts =>
        {
            opts.AllowSameHostRedirectsToHttps();
            opts.AllowSameHostRedirectsToHttps(10443); //Allow redirects to custom HTTPS port
        });

        var secaoWebSec = _configuration.GetSection("NWebSec");
        if (secaoWebSec != null)
        {
            var imageSources = secaoWebSec.GetValue<string>("ImageSources")?.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray() ?? Array.Empty<string>();
            var fontSources = secaoWebSec.GetValue<string>("FontSources")?.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray() ?? Array.Empty<string>();
            var connectSources = secaoWebSec.GetValue<string>("ConnectSources")?.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray() ?? Array.Empty<string>();
            var styleSources = secaoWebSec.GetValue<string>("StyleSources")?.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray() ?? Array.Empty<string>();
            var scriptSources = secaoWebSec.GetValue<string>("ScriptSources")?.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray() ?? Array.Empty<string>();

            app.UseCsp(options => options
                .DefaultSources(s => s.Self())
                .ObjectSources(s => s.None())
                .BaseUris(s => s.None())
                .ImageSources(s =>
                {
                    s.Self();
                    if (imageSources.Length > 0) s.CustomSources(imageSources);
                })
                .FontSources(s =>
                {
                    s.Self();
                    if (fontSources.Length > 0) s.CustomSources(fontSources);
                })
                .ConnectSources(s =>
                {
                    s.Self();
                    if (connectSources.Length > 0) s.CustomSources(connectSources);
                })
                .StyleSources(s =>
                {
                    if (styleSources.Length > 0) s.CustomSources(styleSources);
                })
                .ScriptSources(s =>
                {
                    if (scriptSources.Length > 0) s.CustomSources(scriptSources);
                })
             );
        }

        var securitytxt = $"Contact: seuemail@dominio.com\r\nExpires: Mon, 01 Aug 2050 00:00 +0300";
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapAreaControllerRoute(
              name: "admin",
              areaName: "Admin",
              pattern: "admin/{controller=Home}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            endpoints.MapGet("/.well-known/security.txt", () => securitytxt);
            endpoints.MapGet("/security.txt", () => securitytxt);
        });
    }
}