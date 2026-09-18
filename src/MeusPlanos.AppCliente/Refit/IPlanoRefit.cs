using Refit;
using MeusPlanos.AppCliente.ViewModel;
using MeusPlanos.Definicao.Modelo;
using System;
using System.Threading.Tasks;

namespace MeusPlanos.AppCliente.Refit;

public interface IPlanoRefit
{
    [Get("/")]
    Task<RespostaPaginadaServico<PlanoViewModel>> Navegar(string criterioJson, bool exibirRegistrosDeletados);

    [Get("/{id}")]
    Task<RespostaServico<PlanoViewModel>> ObterPorId(Guid id);

    [Put("/")]
    Task<RespostaServico<PlanoViewModel>> Alterar(PlanoViewModel item);

    [Post("/")]
    Task<RespostaServico<PlanoViewModel>> Incluir(PlanoViewModel item);

    [Delete("/{id}")]
    Task<RespostaServico<int>> Deletar(Guid id);
}
