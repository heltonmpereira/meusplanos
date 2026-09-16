using System;
using System.Threading.Tasks;
using MeusPlanos.AppCliente.ViewModel;
using MeusPlanos.Definicao.Modelo;
using Refit;

namespace MeusPlanos.AppCliente.Refit
{
    public interface IWeatherForecastRefit
    {
        [Get("/")]
        Task<RespostaPaginadaServico<WeatherForecastViewModel>> Navegar(string criterioJson, bool exibirRegistrosDeletados);

        [Get("/{id}")]
        Task<RespostaServico<WeatherForecastViewModel>> ObterPorId(Guid id);

        [Put("/")]
        Task<RespostaServico<WeatherForecastViewModel>> Alterar(WeatherForecastViewModel item);

        [Post("/")]
        Task<RespostaServico<WeatherForecastViewModel>> Incluir(WeatherForecastViewModel item);

        [Delete("/{id}")]
        Task<RespostaServico<int>> Deletar(Guid id);
    }
}