using System;
using System.Threading.Tasks;
using MeusPlanos.AppCliente.ViewModel;
using MeusPlanos.Definicao.Modelo;
using Refit;

namespace MeusPlanos.AppCliente.Refit;

public interface IPapelRefit
{
    [Get("/")]
    Task<RespostaPaginadaServico<PapelViewModel>> Navegar(string criterioJson, bool exibirRegistrosDeletados);

    [Get("/{id}")]
    Task<RespostaServico<PapelViewModel>> ObterPorId(Guid id);

    [Put("/")]
    Task<RespostaServico<PapelViewModel>> Alterar(PapelViewModel item);

    [Post("/")]
    Task<RespostaServico<PapelViewModel>> Incluir(PapelViewModel item);

    [Delete("/{id}")]
    Task<RespostaServico<int>> Deletar(Guid id);

    [Put("/atualizar-usuarios")]
    Task<RespostaServico<PapelViewModel>> AtualizarUsuarios(Guid id, Guid[] idsUsuario);
}