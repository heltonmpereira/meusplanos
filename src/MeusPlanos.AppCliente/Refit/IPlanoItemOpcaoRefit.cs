using Refit;
using MeusPlanos.AppCliente.ViewModel;
using MeusPlanos.Definicao.Modelo;
using System;
using System.Threading.Tasks;

namespace MeusPlanos.AppCliente.Refit;

public interface IPlanoItemOpcaoRefit
{
    [Get("/")]
    Task<RespostaPaginadaServico<PlanoItemOpcaoViewModel>> Navegar(string criterioJson, bool exibirRegistrosDeletados);

    [Get("/{id}")]
    Task<RespostaServico<PlanoItemOpcaoViewModel>> ObterPorId(Guid id);

    [Put("/")]
    Task<RespostaServico<PlanoItemOpcaoViewModel>> Alterar(PlanoItemOpcaoViewModel item);

    [Post("/")]
    Task<RespostaServico<PlanoItemOpcaoViewModel>> Incluir(PlanoItemOpcaoViewModel item);

    [Delete("/{id}")]
    Task<RespostaServico<int>> Deletar(Guid id);
}
