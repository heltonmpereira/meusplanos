using MeusPlanos.AppCliente.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace MeusPlanos.AppCliente.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : PadraoController
{
    // GET: HomeController
    public ActionResult Index()
    {
        return View();
    }
}