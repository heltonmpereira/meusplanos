using System;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Modelo.Maps.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeusPlanos.Modelo.Maps
{
    public class WeatherForecastMap : BaseMap<WeatherForecast, Guid>
    {
        public override void Configure(EntityTypeBuilder<WeatherForecast> builder)
        {
            base.Configure(builder);

            builder.HasOne(o => o.Proprietario);
        }
    }
}