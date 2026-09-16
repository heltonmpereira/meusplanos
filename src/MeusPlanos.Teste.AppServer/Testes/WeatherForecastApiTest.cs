using System;
using System.Net.Http;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Teste.AppServer.Base;
using MeusPlanos.Teste.AppServer.Helper;
using Xunit;

namespace MeusPlanos.Teste.AppServer.Testes
{
    [Collection("TestesDeIntegracao")]
    public class WeatherForecastApiTest(TestFixture fixture)
    {
        private readonly HttpClient _client = fixture.Client;
        public static readonly object[][] DadosInclusao =
        [
            [new DateTime(2017,3,1), 20, "Summary 1"],
            [new DateTime(2017,4,1), 10, "Summary 2"],
        ];

        [Theory, MemberData(nameof(DadosInclusao))]
        public async Task TestarCadastro(DateTime data, int celsius, string summary)
        {
            var weather = new WeatherForecast()
            {
                Date = data,
                TemperatureC = celsius,
                Summary = summary
            };

            var rota = "/api/weatherforecast";
            var token = await _client.ObterTokenLogin("admin", "admin");
            var item = await _client.EnviarPostAsync<WeatherForecast>(rota, weather, token);
            Assert.Equal("Registro cadastrado com sucesso.", item.Mensagem);
            Assert.NotEqual(Guid.NewGuid(), item.Dados.Id);

            var itens = await _client.EnviarGetAsync<WeatherForecast>(rota);
            Assert.True(itens.TotalRegistros > 0);

            rota += $"/{item.Dados.Id}";
            var removido = await _client.EnviarDeleteAsync<int>(rota, token);
            Assert.Equal("1 registro removido com sucesso.", removido.Mensagem);
        }
    }
}