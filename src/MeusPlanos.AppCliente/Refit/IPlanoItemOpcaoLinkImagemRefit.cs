using Refit;
using MeusPlanos.AppCliente.ViewModel;
using MeusPlanos.Definicao.Modelo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeusPlanos.AppCliente.Refit;

public interface IPlanoItemOpcaoLinkImagemRefit
{
    [Get("/")]
    Task<RespostaPaginadaServico<PlanoItemOpcaoLinkImagemViewModel>> Navegar(string criterioJson, bool exibirRegistrosDeletados);

    [Get("/{id}")]
    Task<RespostaServico<PlanoItemOpcaoLinkImagemViewModel>> ObterPorId(Guid id);

    [Get("/PorLink/{planoItemOpcaoLinkId}")]
    Task<RespostaServico<IList<PlanoItemOpcaoLinkImagemViewModel>>> ObterPorLinkId(Guid planoItemOpcaoLinkId);

    [Post("/Vincular")]
    Task<RespostaServico<PlanoItemOpcaoLinkImagemViewModel>> VincularImagem(
        [Query] Guid planoItemOpcaoLinkId,
        [Query] Guid imagemId,
        [Query] int? ordem = null,
        [Query] bool ehCapa = false);

    [Delete("/{id}/Desvincular")]
    Task<RespostaServico<int>> DesvincularImagem(Guid id, [Query] bool excluirArquivo = false);

    [Put("/{id}/DefinirCapa")]
    Task<RespostaServico<int>> DefinirComoCapa(Guid id);

    [Put("/Reordenar/{linkId}")]
    Task<RespostaServico<int>> Reordenar(Guid linkId, [Body] Guid[] idsOrdenados);
}
