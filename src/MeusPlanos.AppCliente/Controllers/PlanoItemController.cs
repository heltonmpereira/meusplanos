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
public class PlanoItemController(IPlanoItemRefit servico, IHttpContextAccessor httpContextAccessor)
    : BaseController<PlanoItemViewModel, Guid, IPlanoItemRefit>(servico, httpContextAccessor)
{
    protected override string OrdenacaoPadrao { get; set; } = "ordem, nome";
    protected Guid PlanoIdCorrente
    {
        get
        {
            var valor = Request.Query["PlanoId"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(valor) && Guid.TryParse(valor, out var id))
                return id;
            var valorRota = RouteData.Values["PlanoId"]?.ToString();
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

        var planoId = PlanoIdCorrente;
        if (planoId != Guid.Empty)
        {
            criterio.AdicionarFiltro("Tela", new FiltroOpcao
            {
                NomePropriedade = "PlanoId",
                Operador = TipoOperadorBusca.Igual,
                Valor = planoId
            });
            ViewBag.PlanoId = planoId;
        }

        return criterio;
    }

    public override async Task<IActionResult> Incluir()
    {
        var model = Activator.CreateInstance<PlanoItemViewModel>();
        var planoId = PlanoIdCorrente;
        if (planoId != Guid.Empty)
            model.PlanoId = planoId;

        await InicializarObjeto(model);
        return View(await Task.FromResult(model));
    }

    protected override PlanoItemViewModel ExecutarAntesSalvar(PlanoItemViewModel item)
    {
        if (item.PlanoId == Guid.Empty)
        {
            var planoId = PlanoIdCorrente;
            if (planoId != Guid.Empty)
                item.PlanoId = planoId;
        }
        return item;
    }
}
