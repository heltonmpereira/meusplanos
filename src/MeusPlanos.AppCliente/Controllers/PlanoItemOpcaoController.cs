using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dhani.Utilitarios.Filtro;
using MeusPlanos.AppCliente.Controllers.Base;
using MeusPlanos.AppCliente.Filter;
using MeusPlanos.AppCliente.Refit;
using MeusPlanos.AppCliente.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MeusPlanos.AppCliente.Controllers;

[BreadcrumbActionFilter]
public class PlanoItemOpcaoController(IPlanoItemOpcaoRefit servico, IHttpContextAccessor httpContextAccessor)
    : BaseController<PlanoItemOpcaoViewModel, Guid, IPlanoItemOpcaoRefit>(servico, httpContextAccessor)
{
    protected override string OrdenacaoPadrao { get; set; } = "ordem, nome";
    protected Guid PlanoItemIdCorrente
    {
        get
        {
            var valor = Request.Query["PlanoItemId"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(valor) && Guid.TryParse(valor, out var id))
                return id;
            var valorRota = RouteData.Values["PlanoItemId"]?.ToString();
            if (!string.IsNullOrWhiteSpace(valorRota) && Guid.TryParse(valorRota, out var idRota))
                return idRota;
            return Guid.Empty;
        }
    }

    protected override ICriterio ObterFiltroPadrao(int page = 1)
    {
        var criterio = new FiltroBase
        {
            Pagina = page,
            Ordenacao = OrdenacaoPadrao,
            ItensPorPagina = 20
        };

        var planoItemId = PlanoItemIdCorrente;
        if (planoItemId != Guid.Empty)
        {
            criterio.AdicionarFiltro("Tela", new FiltroOpcao
            {
                NomePropriedade = "PlanoItemId",
                Operador = TipoOperadorBusca.Igual,
                Valor = planoItemId
            });
            ViewBag.PlanoItemId = planoItemId;
        }

        return criterio;
    }

    public override async Task<IActionResult> Incluir()
    {
        var model = Activator.CreateInstance<PlanoItemOpcaoViewModel>();
        var planoItemId = PlanoItemIdCorrente;
        if (planoItemId != Guid.Empty)
            model.PlanoItemId = planoItemId;

        await InicializarObjeto(model);
        return View(await Task.FromResult(model));
    }

    protected override PlanoItemOpcaoViewModel ExecutarAntesSalvar(PlanoItemOpcaoViewModel item)
    {
        if (item.PlanoItemId == Guid.Empty)
        {
            var planoItemId = PlanoItemIdCorrente;
            if (planoItemId != Guid.Empty)
                item.PlanoItemId = planoItemId;
        }
        return item;
    }
}
