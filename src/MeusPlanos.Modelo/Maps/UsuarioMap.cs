using System;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Modelo.Maps.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeusPlanos.Modelo.Maps;

public class UsuarioMap : BaseMap<Usuario, Guid>
{
    public override void Configure(EntityTypeBuilder<Usuario> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Nome).HasMaxLength(150).IsRequired();
        builder.Property(p => p.Sobrenome).HasMaxLength(150).IsRequired();
        builder.Property(p => p.Email).HasMaxLength(150).IsRequired();
        builder.Property(p => p.Username).HasMaxLength(50).IsRequired();
        builder.Property(p => p.PasswordHash).HasMaxLength(200).IsRequired();
        builder.Property(p => p.DataCriacao).HasDefaultValueSql("getdate()");

        builder.Property(p => p.CodigoRedefinicaoSenha).HasMaxLength(200);

        builder.HasIndex(i => i.Email).IsUnique();
        builder.HasIndex(i => i.Username).IsUnique();

        builder.HasData(ObterDadosIniciais());
    }

    private static Usuario[] ObterDadosIniciais()
    {
        var usuarios = new[] {
            new Usuario{
                Id = Guid.Parse("7e0a9200-f672-11ed-9ae1-0fc4e648d876"),
                Nome = "Administrador",
                Sobrenome = "do Sistema",
                Username = "admin",
                Email = "admin@admin.com",
                PasswordHash = "AITMdMEsqiixw35g6qbq+zbaYM2HttO8uXFdJSg96xVnUn2AatqTCDcKqSVPlbzulA=="
            }
        };

        return usuarios;
    }
}