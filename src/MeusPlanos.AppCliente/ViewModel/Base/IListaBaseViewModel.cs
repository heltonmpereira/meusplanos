using Dhani.Utilitarios.Filtro;
using MeusPlanos.AppCliente.Models.Interface;
using Newtonsoft.Json;

namespace MeusPlanos.AppCliente.ViewModel.Base
{
    public interface IListaBaseViewModel
    {
        public ICriterioViewModel FiltroViewModel { get; set; }
        public FiltroOpcao OpcaoAtual { get; set; }
        public ICriterio Filtro { get; set; }
        public string FiltroJson { get => JsonConvert.SerializeObject(Filtro); }
    }
}