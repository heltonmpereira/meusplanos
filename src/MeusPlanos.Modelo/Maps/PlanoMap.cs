using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Modelo.Maps.Base;

namespace MeusPlanos.Modelo.Maps;

public class PlanoMap : BaseMap<Plano, Guid>
{
    public override void Configure(EntityTypeBuilder<Plano> builder)
    {
        base.Configure(builder);
    }
}
