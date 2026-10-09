using Refit;
using MeusPlanos.AppCliente.ViewModel;
using MeusPlanos.Definicao.Modelo;
using System;
using System.Threading.Tasks;

namespace MeusPlanos.AppCliente.Refit;

public interface IPlanoItemOpcaoLinkRefit
{
    [Get("/")]
    Task<RespostaPaginadaServico<PlanoItemOpcaoLinkViewModel>> Navegar(string criterioJson, bool exibirRegistrosDeletados);

    [Get("/{id}")]
    Task<RespostaServico<PlanoItemOpcaoLinkViewModel>> ObterPorId(Guid id);

    [Put("/")]
    Task<RespostaServico<PlanoItemOpcaoLinkViewModel>> Alterar(PlanoItemOpcaoLinkViewModel item);

    [Post("/")]
    Task<RespostaServico<PlanoItemOpcaoLinkViewModel>> Incluir(PlanoItemOpcaoLinkViewModel item);

    [Delete("/{id}")]
    Task<RespostaServico<int>> Deletar(Guid id);
}
