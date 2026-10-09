using Refit;
using MeusPlanos.AppCliente.ViewModel;
using MeusPlanos.Definicao.Modelo;
using System;
using System.Threading.Tasks;

namespace MeusPlanos.AppCliente.Refit;

public interface IPlanoItemRefit
{
    [Get("/")]
    Task<RespostaPaginadaServico<PlanoItemViewModel>> Navegar(string criterioJson, bool exibirRegistrosDeletados);

    [Get("/{id}")]
    Task<RespostaServico<PlanoItemViewModel>> ObterPorId(Guid id);

    [Put("/")]
    Task<RespostaServico<PlanoItemViewModel>> Alterar(PlanoItemViewModel item);

    [Post("/")]
    Task<RespostaServico<PlanoItemViewModel>> Incluir(PlanoItemViewModel item);

    [Delete("/{id}")]
    Task<RespostaServico<int>> Deletar(Guid id);
}
