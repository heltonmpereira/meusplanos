using System;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Modelo.Maps.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeusPlanos.Modelo.Maps;

public class PlanoMap : BaseMap<Plano, Guid>
{
    public override void Configure(EntityTypeBuilder<Plano> builder)
    {
        base.Configure(builder);
        
        builder.Property(p => p.DataCriacao).HasDefaultValueSql("getdate()");
    }
}
