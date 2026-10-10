using System;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Modelo.Maps.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeusPlanos.Modelo.Maps;

public class PlanoItemOpcaoLinkImagemMap : BaseMap<PlanoItemOpcaoLinkImagem, Guid>
{
    public override void Configure(EntityTypeBuilder<PlanoItemOpcaoLinkImagem> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.DataCriacao).HasDefaultValueSql("getdate()");

        builder
            .HasOne(li => li.PlanoItemOpcaoLink)
            .WithMany(l => l.ImagensLinks)
            .HasForeignKey(li => li.PlanoItemOpcaoLinkId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(li => li.Imagem)
            .WithMany(i => i.LinksImagens)
            .HasForeignKey(li => li.ImagemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(li => new { li.PlanoItemOpcaoLinkId, li.ImagemId })
            .HasFilter("[DataDelecao] IS NULL")
            .IsUnique();
    }
}
