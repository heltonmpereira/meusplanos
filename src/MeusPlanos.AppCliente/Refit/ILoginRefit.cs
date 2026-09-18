using System;
using System.Threading.Tasks;
using MeusPlanos.AppCliente.ViewModel;
using MeusPlanos.AppCliente.ViewModel.Login;
using MeusPlanos.Definicao.Modelo;
using MeusPlanos.Definicao.Modelo.Usuario;
using Refit;

namespace MeusPlanos.AppCliente.Refit;

public interface ILoginRefit
{
    [Get("/{id}")]
    Task<RespostaServico<UsuarioViewModel>> ObterPorId(Guid id);

    [Put("/")]
    Task<RespostaServico<UsuarioViewModel>> Alterar(UsuarioViewModel item);

    [Post("/")]
    Task<RespostaServico<UsuarioToken>> Logar(LoginViewModel item);

    [Post("/registrar")]
    Task<RespostaServico<UsuarioViewModel>> Registrar(RegistrarViewModel item);

    [Post("/RedefinirSenha")]
    Task<RespostaServico<RedefinirSenhaViewModel>> RedefinirSenha(SolicitarRedefinicaoSenhaViewModel model);

    [Post("/AlterarSenhaComCodigoRedefinicao")]
    Task<RespostaServico<bool>> AlterarSenhaComCodigoRedefinicao(RedefinirSenhaViewModel model);

    [Put("/AlterarSenha")]
    Task<RespostaServico<bool>> AlterarSenha(AlterarSenhaViewModel item);

    [Get("/acessar-como/{id}")]
    Task<RespostaServico<UsuarioToken>> AcessarComo(Guid id);
}