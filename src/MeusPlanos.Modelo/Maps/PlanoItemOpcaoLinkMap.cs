using System;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Modelo.Maps.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeusPlanos.Modelo.Maps;

public class PlanoItemOpcaoLinkMap : BaseMap<PlanoItemOpcaoLink, Guid>
{
    public override void Configure(EntityTypeBuilder<PlanoItemOpcaoLink> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Url).HasMaxLength(1000).IsRequired();
        builder.Property(p => p.NomeSite).HasMaxLength(200);
        builder.Property(p => p.Valor).HasPrecision(18, 2);
        builder.Property(p => p.CondicoesPagamento).HasMaxLength(2000);
        builder.Property(p => p.Observacoes).HasMaxLength(4000);
        builder.Property(p => p.DataCriacao).HasDefaultValueSql("getdate()");

        builder
            .HasOne(o => o.PlanoItemOpcao)
            .WithMany(m => m.Links)
            .HasForeignKey(fk => fk.PlanoItemOpcaoId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasIndex(i => i.PlanoItemOpcaoId);
    }
}
