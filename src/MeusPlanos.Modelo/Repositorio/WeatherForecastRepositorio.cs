using System;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Modelo.Data;
using MeusPlanos.Modelo.Repositorio.Base;

namespace MeusPlanos.Modelo.Repositorio
{
    public class WeatherForecastRepositorio(IDbContext contexto) : BaseRepositorio<WeatherForecast, Guid>(contexto), IWeatherForecastRepositorio
    {
    }
}