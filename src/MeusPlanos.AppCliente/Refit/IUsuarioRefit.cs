using System;
using System.Threading.Tasks;
using MeusPlanos.AppCliente.ViewModel;
using MeusPlanos.Definicao.Modelo;
using Refit;

namespace MeusPlanos.AppCliente.Refit
{
    public interface IUsuarioRefit
    {
        [Get("/")]
        Task<RespostaPaginadaServico<UsuarioViewModel>> Navegar(string criterioJson, bool exibirRegistrosDeletados);

        [Get("/{id}")]
        Task<RespostaServico<UsuarioViewModel>> ObterPorId(Guid id);

        [Put("/")]
        Task<RespostaServico<UsuarioViewModel>> Alterar(UsuarioViewModel item);

        [Post("/")]
        Task<RespostaServico<UsuarioViewModel>> Incluir(UsuarioViewModel item);

        [Delete("/{id}")]
        Task<RespostaServico<int>> Deletar(Guid id);
    }
}