using System;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.Definicao.Entidade
{
    public class WeatherForecast : IEntidade<Guid>
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        public string Summary { get; set; }
        public bool Deletado { get; set; }

        public Guid ProprietarioId { get; set; }
        public Usuario Proprietario { get; set; }
    }
}