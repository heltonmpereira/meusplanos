using System;
using MeusPlanos.AppCliente.ViewModel.Base;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.AppCliente.ViewModel;

public class PlanoItemOpcaoLinkViewModel : BaseViewModel, IEntidade<Guid>
{
    public Guid Id { get; set; }
    public Guid PlanoItemOpcaoId { get; set; }
    public PlanoItemOpcaoViewModel PlanoItemOpcao { get; set; }
    public string Url { get; set; }
    public string NomeSite { get; set; }
    public decimal? Valor { get; set; }
    public string CondicoesPagamento { get; set; }
    public string Observacoes { get; set; }
    public DateTime? DataCotacao { get; set; }

    public DateTime DataCriacao { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public DateTime? DataDelecao { get; set; }
}
