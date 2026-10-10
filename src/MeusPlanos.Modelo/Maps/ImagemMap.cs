using System;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Modelo.Maps.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeusPlanos.Modelo.Maps;

public class ImagemMap : BaseMap<Imagem, Guid>
{
    public override void Configure(EntityTypeBuilder<Imagem> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.DataCriacao).HasDefaultValueSql("getdate()");

        builder
            .HasMany(i => i.LinksImagens)
            .WithOne(li => li.Imagem)
            .HasForeignKey(li => li.ImagemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
