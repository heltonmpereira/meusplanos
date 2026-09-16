using System.Diagnostics;
using MeusPlanos.AppCliente.Controllers.Base;
using MeusPlanos.AppCliente.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MeusPlanos.AppCliente.Controllers
{
    [AllowAnonymous]
    public class HomeController(ILogger<HomeController> logger) : PadraoController
    {
        private readonly ILogger<HomeController> _logger = logger;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionHandle = HttpContext.Features
                .Get<IExceptionHandlerPathFeature>();

            var viewModel = new ErrorViewModel(exceptionHandle)
            {
                Caminho = exceptionHandle.Path ?? HttpContext.Request.Path,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            _logger.LogError(
                exception: exceptionHandle.Error,
                message: "Mensagem de erro: {Mensagem}",
                viewModel.Mensagem);
            return View(viewModel);
        }
    }
}