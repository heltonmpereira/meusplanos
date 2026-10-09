using System;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Modelo.Maps.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeusPlanos.Modelo.Maps;

public class PlanoItemMap : BaseMap<PlanoItem, Guid>
{
    public override void Configure(EntityTypeBuilder<PlanoItem> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Nome).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Descricao).HasMaxLength(2000);
        builder.Property(p => p.ValorOrcamentoEstimado).HasPrecision(18, 2).IsRequired();
        builder.Property(p => p.Ordem).IsRequired();
        builder.Property(p => p.DataCriacao).HasDefaultValueSql("getdate()");

        builder
            .HasOne(o => o.Plano)
            .WithMany(m => m.Itens)
            .HasForeignKey(fk => fk.PlanoId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasIndex(i => i.PlanoId);
    }
}
