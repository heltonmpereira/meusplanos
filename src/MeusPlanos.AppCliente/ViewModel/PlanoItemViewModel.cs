using System;
using System.Collections.Generic;
using MeusPlanos.AppCliente.ViewModel.Base;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.AppCliente.ViewModel;

public class PlanoItemViewModel : BaseViewModel, IEntidade<Guid>
{
    public Guid Id { get; set; }
    public Guid PlanoId { get; set; }
    public PlanoViewModel Plano { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal ValorOrcamentoEstimado { get; set; }
    public int Ordem { get; set; }

    public DateTime DataCriacao { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public DateTime? DataDelecao { get; set; }

    public ICollection<PlanoItemOpcaoViewModel> Opcoes { get; set; }
}
