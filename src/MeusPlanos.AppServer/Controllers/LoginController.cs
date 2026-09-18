using System;
using System.Threading.Tasks;
using Dhani.Utilitarios.Helper;
using MeusPlanos.AppServer.Helper;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Servico;
using MeusPlanos.Definicao.Modelo;
using MeusPlanos.Definicao.Modelo.Usuario;
using MeusPlanos.Servico.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeusPlanos.AppServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController(ILoginServico servico, IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    public string IdUsuarioLogado { get; } = httpContextAccessor.HttpContext
            .User
            .FindFirst("UsuarioId")?
            .Value;
    private ILoginServico Servico { get; } = servico;

    [AllowAnonymous]
    [HttpPost()]
    public async Task<IActionResult> LoginAsync(UsuarioLogin loginInfo)
    {
        var retorno = await Servico.RespostaServicoAsync<UsuarioToken>(
             parameters: [loginInfo]);

        return retorno;
    }

    [AllowAnonymous]
    [HttpPost("registrar")]
    public async Task<IActionResult> RegistrarAsync(UsuarioRegistro registro)
    {
        var user = new Usuario().Mapear(registro);
        user.PasswordHash = registro.Password.CriptografarTexto();

        var retorno = await Servico.RespostaServicoAsync<Usuario>(
             parameters: [user],
             nomeMetodo: "IncluirAsync");

        return retorno;
    }

    [AllowAnonymous]
    [HttpPost("redefinirSenha")]
    public async Task<IActionResult> RedefinirSenhaAsync(UsuarioSolicitacaoRedefinicaoSenha model)
    {
        var retorno = await Servico.RespostaServicoAsync<UsuarioRedefinirSenha>(
             parameters: [model]);

        return retorno;
    }

    [AllowAnonymous]
    [HttpPost("alterarSenhaComCodigoRedefinicao")]
    public async Task<IActionResult> AlterarSenhaComCodigoRedefinicaoAsync(UsuarioRedefinirSenha model)
    {
        model.Password = model.Password.CriptografarTexto();
        var retorno = await Servico.RespostaServicoAsync<bool>(
             parameters: [model]);

        return retorno;
    }

    [HttpPut("alterarSenha")]
    public async Task<IActionResult> AlterarSenhaAsync(AlterarSenha model) =>
        await Servico.RespostaServicoAsync<bool>(
             parameters: [model]);

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorIdAsync(Guid id) =>
        await Servico.RespostaServicoAsync<Usuario>(
            parameters: [id, true]);

    [HttpPut]
    public async Task<IActionResult> AlterarAsync(Usuario item)
    {
        if (item.Id.ToString() != IdUsuarioLogado)
        {
            var erro = new Exception("Dados inválidos");
            var retorno = new RespostaServico<Exception>(erro, erro.Message);
            return BadRequest(retorno);
        }

        return await Servico.RespostaServicoAsync<Usuario>(
            parameters: [item]);
    }

    [HttpGet("acessar-como/{id}")]
    public async Task<IActionResult> AcessarComoAsync(Guid id) =>
        await Servico.RespostaServicoAsync<UsuarioToken>(
            parameters: [id]);
}