using System;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Modelo.Maps.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeusPlanos.Modelo.Maps;

public class PlanoItemOpcaoMap : BaseMap<PlanoItemOpcao, Guid>
{
    public override void Configure(EntityTypeBuilder<PlanoItemOpcao> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Nome).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Descricao).HasMaxLength(2000);
        builder.Property(p => p.Caracteristicas).HasMaxLength(4000);
        builder.Property(p => p.Ordem).IsRequired();
        builder.Property(p => p.DataCriacao).HasDefaultValueSql("getdate()");

        builder
            .HasOne(o => o.PlanoItem)
            .WithMany(m => m.Opcoes)
            .HasForeignKey(fk => fk.PlanoItemId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasIndex(i => i.PlanoItemId);
    }
}
