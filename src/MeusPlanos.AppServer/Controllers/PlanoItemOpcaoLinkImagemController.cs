using System;
using System.Threading.Tasks;
using Dhani.Utilitarios.Filtro;
using MeusPlanos.AppServer.Controllers.Base;
using MeusPlanos.AppServer.Helper;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeusPlanos.AppServer.Controllers;

public class PlanoItemOpcaoLinkImagemController(
    IPlanoItemOpcaoLinkImagemServico servico,
    IHttpContextAccessor httpContextAccessor)
    : BaseApiController<PlanoItemOpcaoLinkImagem, Guid, IPlanoItemOpcaoLinkImagemServico, IPlanoItemOpcaoLinkImagemRepositorio>(
        servico, httpContextAccessor)
{
    [HttpGet("PorLink/{planoItemOpcaoLinkId}")]
    public async Task<IActionResult> ObterPorLinkIdAsync(Guid planoItemOpcaoLinkId) =>
        await Servico.RespostaServicoAsync<System.Collections.Generic.IList<PlanoItemOpcaoLinkImagem>>(
            nomeMetodo: nameof(IPlanoItemOpcaoLinkImagemServico.ObterPorLinkIdAsync),
            parameters: [planoItemOpcaoLinkId]);

    [HttpPost("Vincular")]
    public async Task<IActionResult> VincularImagemAsync(
        [FromQuery] Guid planoItemOpcaoLinkId,
        [FromQuery] Guid imagemId,
        [FromQuery] int? ordem = null,
        [FromQuery] bool ehCapa = false) =>
        await Servico.RespostaServicoAsync<PlanoItemOpcaoLinkImagem>(
            nomeMetodo: nameof(IPlanoItemOpcaoLinkImagemServico.VincularImagemAsync),
            parameters: [planoItemOpcaoLinkId, imagemId, ordem, ehCapa]);

    [HttpDelete("{id}/Desvincular")]
    public async Task<IActionResult> DesvincularImagemAsync(Guid id, [FromQuery] bool excluirArquivo = false) =>
        await Servico.RespostaServicoAsync<int>(
            nomeMetodo: nameof(IPlanoItemOpcaoLinkImagemServico.DesvincularImagemAsync),
            parameters: [id, excluirArquivo]);

    [HttpPut("{id}/DefinirCapa")]
    public async Task<IActionResult> DefinirComoCapaAsync(Guid id) =>
        await Servico.RespostaServicoAsync<int>(
            nomeMetodo: nameof(IPlanoItemOpcaoLinkImagemServico.DefinirComoCapaAsync),
            parameters: [id]);

    [HttpPut("Reordenar/{linkId}")]
    public async Task<IActionResult> ReordenarAsync(Guid linkId, [FromBody] Guid[] idsOrdenados) =>
        await Servico.RespostaServicoAsync<int>(
            nomeMetodo: nameof(IPlanoItemOpcaoLinkImagemServico.ReordenarAsync),
            parameters: [linkId, idsOrdenados]);
}
