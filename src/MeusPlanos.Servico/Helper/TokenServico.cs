using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using MeusPlanos.Definicao.Modelo.Usuario;
using Microsoft.IdentityModel.Tokens;

namespace MeusPlanos.Servico.Helper;

public static class TokenServico
{
    public const string Segredo = "fedaf7d8863b48e197b9287d492b708e";

    public static string GerarToken(UsuarioToken dadosUsuario)
    {
        var claims = new List<Claim> {
            new("UsuarioId", dadosUsuario.Id.ToString()),
            new(ClaimTypes.Name, dadosUsuario.Nome),
            new(ClaimTypes.Email, dadosUsuario.Email),
                new("DataAtualizacao", dadosUsuario.DataAtualizacao?.ToString("O") ?? DateTime.MinValue.ToString("O")),
        };

        claims.AddRange(dadosUsuario.Roles
            .Select(s => new Claim(ClaimTypes.Role, s.Trim().ToLower())));

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Segredo);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = "MeusPlanos",
            Audience = "MeusPlanos",
            IssuedAt = DateTime.Now.AddMonths(1),
            Expires = dadosUsuario.DataExpericaoToken,
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
            TokenType = "Bearer"
        };

        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}