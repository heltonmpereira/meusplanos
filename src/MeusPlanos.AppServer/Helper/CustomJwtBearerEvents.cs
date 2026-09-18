using System;
using System.Linq;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Interface.Servico;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MeusPlanos.AppServer.Helper;

public class CustomJwtBearerEvents : JwtBearerEvents
{
    private readonly IUsuarioServico _usuarioServico;

    public CustomJwtBearerEvents(IUsuarioServico usuarioServico)
    {
        _usuarioServico = usuarioServico;
    }

    public override async Task TokenValidated(TokenValidatedContext context)
    {
        try
        {
            // Extrair a data de atualização do token
            var dataAtualizacaoClaim = context.Principal?.Claims
                .FirstOrDefault(c => c.Type == "DataAtualizacao");

            if (dataAtualizacaoClaim == null)
            {
                context.Fail("Token não contém data de atualização do usuário");
                return;
            }

            // Extrair o ID do usuário
            var usuarioIdClaim = context.Principal?.Claims
                .FirstOrDefault(c => c.Type == "UsuarioId");

            if (usuarioIdClaim == null || !Guid.TryParse(usuarioIdClaim.Value, out var usuarioId))
            {
                context.Fail("Token não contém ID válido do usuário");
                return;
            }

            // Buscar o usuário atual no banco
            var respostaUsuario = await _usuarioServico.ObterPorIdAsync(usuarioId);
            if (!respostaUsuario.BemSucedido || respostaUsuario.Dados == null)
            {
                context.Fail("Usuário não encontrado");
                return;
            }

            var usuarioAtual = respostaUsuario.Dados;

            // Comparar a data de atualização do token com a data atual do usuário
            if (DateTime.TryParse(dataAtualizacaoClaim.Value, out var dataToken) &&
                usuarioAtual.DataAlteracao.HasValue &&
                dataToken < usuarioAtual.DataAlteracao.Value)
            {
                context.Fail("Token expirado devido a alterações no usuário");
                return;
            }

            await base.TokenValidated(context);
        }
        catch (Exception ex)
        {
            context.Fail($"Erro na validação do token: {ex.Message}");
        }
    }

    public override Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        // Log do erro de autenticação
        return base.AuthenticationFailed(context);
    }
}
