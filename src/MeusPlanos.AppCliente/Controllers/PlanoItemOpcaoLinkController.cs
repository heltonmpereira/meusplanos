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
using MeusPlanos.Definicao.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

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

    [HttpPost]
    public override async Task<IActionResult> Incluir(PlanoItemOpcaoLinkViewModel item)
    {
        if (!ModelState.IsValid)
        {
            await InicializarObjeto(item);
            return View(item);
        }
        item = ExecutarAntesSalvar(item);
        var resposta = await Servico.Incluir(item).ConfigureAwait(false);
        if (resposta == null || !resposta.BemSucedido)
        {
            ModelState.AddModelError(string.Empty, resposta?.Mensagem ?? "Falha ao incluir.");
            await InicializarObjeto(item);
            return View(item);
        }
        var linkSalvo = resposta.Dados;
        if (linkSalvo != null)
            await SincronizarGaleria(linkSalvo.Id);
        return RedirecionarPosSalvar();
    }

    [HttpPost]
    public override async Task<IActionResult> Alterar(PlanoItemOpcaoLinkViewModel item)
    {
        if (!ModelState.IsValid)
        {
            await InicializarObjeto(item);
            return View(item);
        }
        item = ExecutarAntesSalvar(item);
        var resposta = await Servico.Alterar(item).ConfigureAwait(false);
        if (resposta == null || !resposta.BemSucedido)
        {
            ModelState.AddModelError(string.Empty, resposta?.Mensagem ?? "Falha ao alterar.");
            await InicializarObjeto(item);
            return View(item);
        }
        await SincronizarGaleria(item.Id);
        return RedirecionarPosSalvar();
    }

    private async Task SincronizarGaleria(Guid linkId)
    {
        if (linkId == Guid.Empty) return;

        // Parse de arrays do form
        var ids = Request.Form["GaleriaImagensIds"].ToArray()
            .Select(s => Guid.TryParse(s, out var g) ? (Guid?)g : null)
            .Where(w => w.HasValue)
            .Select(s => s.Value)
            .ToArray();
        var linkImagemIds = Request.Form["GaleriaLinkImagemIds"].ToArray()
            .Select(s => Guid.TryParse(s, out var g) ? (Guid?)g : null)
            .ToArray();
        var capas = Request.Form["GaleriaCapas"].ToArray();

        var svc = HttpContext.RequestServices.GetRequiredService<IPlanoItemOpcaoLinkImagemRefit>();
        var existentes = (await svc.ObterPorLinkId(linkId).ConfigureAwait(false))?.Dados
            ?? new List<PlanoItemOpcaoLinkImagemViewModel>();

        // 1) Remover os que não estão mais na galeria
        foreach (var ex in existentes)
        {
            var idxEx = Array.FindIndex(linkImagemIds, x => x == ex.Id);
            if (idxEx < 0)
            {
                // Não mais presente -> desvincular (sem excluir o arquivo por padrão)
                _ = await svc.DesvincularImagem(ex.Id, false).ConfigureAwait(false);
            }
        }

        // 2) Vincular novas imagens (as que não tem GaleriaLinkImagemIds) e definir ordem/capa
        Guid? idCapa = null;
        for (var i = 0; i < ids.Length; i++)
        {
            var imgId = ids[i];
            var liIdStr = linkImagemIds.Length > i ? linkImagemIds[i] : null;
            var ehCapaStr = capas.Length > i ? capas[i] : "false";
            var ehCapa = string.Equals(ehCapaStr, "true", StringComparison.OrdinalIgnoreCase);

            if (!liIdStr.HasValue || liIdStr.Value == Guid.Empty)
            {
                var r = await svc.VincularImagem(linkId, imgId, i, ehCapa).ConfigureAwait(false);
                if (r?.Dados != null && ehCapa) idCapa = r.Dados.Id;
            }
            else if (ehCapa)
            {
                idCapa = liIdStr.Value;
            }
        }

        // 3) Reordenar e definir capa
        if (ids.Length > 0)
        {
            // Vincular se necessário para ter linkImagemId
            var listaAtualizada = (await svc.ObterPorLinkId(linkId).ConfigureAwait(false))?.Dados
                ?? new List<PlanoItemOpcaoLinkImagemViewModel>();
            var ordemIds = ids
                .Select(imgId => listaAtualizada.FirstOrDefault(f => f.ImagemId == imgId)?.Id)
                .Where(w => w.HasValue)
                .Select(s => s.Value)
                .ToArray();
            if (ordemIds.Length > 0)
                _ = await svc.Reordenar(linkId, ordemIds).ConfigureAwait(false);
        }
        if (idCapa.HasValue)
            _ = await svc.DefinirComoCapa(idCapa.Value).ConfigureAwait(false);
    }

    private IActionResult RedirecionarPosSalvar()
    {
        var opcaoId = PlanoItemOpcaoIdCorrente;
        if (opcaoId != Guid.Empty)
            return RedirectToAction(nameof(Index), new { PlanoItemOpcaoId = opcaoId });
        return RedirectToAction(nameof(Index));
    }
}
