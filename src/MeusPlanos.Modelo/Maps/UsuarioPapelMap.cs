using System;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Modelo.Maps.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeusPlanos.Modelo.Maps;

public class UsuarioPapelMap : BaseMap<UsuarioPapel, Guid>
{
    public override void Configure(EntityTypeBuilder<UsuarioPapel> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.DataCriacao).HasDefaultValueSql("getdate()");

        builder
            .HasOne(o => o.Usuario)
            .WithMany(m => m.Papeis);

        builder
            .HasOne(o => o.Papel)
            .WithMany(m => m.Usuarios);

        builder.HasData(ObterDadosIniciais());
    }

    private static UsuarioPapel[] ObterDadosIniciais()
    {
        var usuarioPapeis = new[] {
            new UsuarioPapel{
                Id = Guid.Parse("7e0a9203-f672-11ed-9ae1-0fc4e648d876"),
                UsuarioId = Guid.Parse("7e0a9200-f672-11ed-9ae1-0fc4e648d876"),
                PapelId = Guid.Parse("7e0a9201-f672-11ed-9ae1-0fc4e648d876")
            },
            new UsuarioPapel{
                Id = Guid.Parse("7e0a9204-f672-11ed-9ae1-0fc4e648d876"),
                UsuarioId = Guid.Parse("7e0a9200-f672-11ed-9ae1-0fc4e648d876"),
                PapelId = Guid.Parse("7e0a9202-f672-11ed-9ae1-0fc4e648d876")
            }
        };

        return usuarioPapeis;
    }
}