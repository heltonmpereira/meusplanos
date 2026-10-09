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
public class PlanoItemOpcaoLinkController(IPlanoItemOpcaoLinkRefit servico, IHttpContextAccessor httpContextAccessor)
    : BaseController<PlanoItemOpcaoLinkViewModel, Guid, IPlanoItemOpcaoLinkRefit>(servico, httpContextAccessor)
{
    protected override string OrdenacaoPadrao { get; set; } = "dataCriacao desc";
    protected Guid PlanoItemOpcaoIdCorrente
    {
        get
        {
            var valor = Request.Query["PlanoItemOpcaoId"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(valor) && Guid.TryParse(valor, out var id))
                return id;
            var valorRota = RouteData.Values["PlanoItemOpcaoId"]?.ToString();
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

        var opcaoId = PlanoItemOpcaoIdCorrente;
        if (opcaoId != Guid.Empty)
        {
            criterio.AdicionarFiltro("Tela", new FiltroOpcao
            {
                NomePropriedade = "PlanoItemOpcaoId",
                Operador = TipoOperadorBusca.Igual,
                Valor = opcaoId
            });
            ViewBag.PlanoItemOpcaoId = opcaoId;
        }

        return criterio;
    }

    public override async Task<IActionResult> Incluir()
    {
        var model = Activator.CreateInstance<PlanoItemOpcaoLinkViewModel>();
        var opcaoId = PlanoItemOpcaoIdCorrente;
        if (opcaoId != Guid.Empty)
            model.PlanoItemOpcaoId = opcaoId;

        model.DataCotacao = DateTime.Now;
        await InicializarObjeto(model);
        return View(await Task.FromResult(model));
    }

    protected override PlanoItemOpcaoLinkViewModel ExecutarAntesSalvar(PlanoItemOpcaoLinkViewModel item)
    {
        if (item.PlanoItemOpcaoId == Guid.Empty)
        {
            var opcaoId = PlanoItemOpcaoIdCorrente;
            if (opcaoId != Guid.Empty)
                item.PlanoItemOpcaoId = opcaoId;
        }
        return item;
    }
}
