using System;
using System.ComponentModel;
using System.Globalization;
using MeusPlanos.AppCliente.ViewModel.Base;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.AppCliente.ViewModel
{
    public class WeatherForecastViewModel : BaseViewModel, IEntidade<Guid>
    {
        public Guid Id { get; set; }
        [DisplayName("Data")]
        public DateTime Date { get; set; } = DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy HH:mm:00"), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        [DisplayName("ºCelsius")]
        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        [DisplayName("Resumo")]
        public string Summary { get; set; }
        public bool Deletado { get; set; }
    }
}