using System;
using MeusPlanos.AppCliente.ViewModel.Base;
using MeusPlanos.Definicao.Interface.Entidade;
using MeusPlanos.Definicao.Modelo.Enum;

namespace MeusPlanos.AppCliente.ViewModel;

public class PlanoViewModel : BaseViewModel, IEntidade<Guid>
{
    public Guid Id {get; set; }
    public string Nome { get; set; }
    public TipoPlano Tipo { get; set; }               // Imovel, Viagem, Ferias, Veiculo, Livre
    public decimal? ValorEstimadoManual { get; set; } // teto/chute inicial
    public DateOnly DataAlvo { get; set; }
    public StatusPlano Status { get; set; }
    public string Moeda { get; set; } = "BRL";

    public DateTime DataCriacao { get; set; }
    public DateTime DataAlteracao { get; set; }
    public DateTime DataDelecao { get; set; }

    //public ICollection<GrupoPlano> Grupos { get; set; }
    //public ICollection<Aporte> Aportes { get; set; }
}
