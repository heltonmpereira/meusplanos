using System;
using MeusPlanos.AppCliente.ViewModel.Base;

namespace MeusPlanos.AppCliente.ViewModel;

public class PlanoItemOpcaoLinkImagemViewModel : BaseViewModel
{
    public Guid Id { get; set; }
    public Guid PlanoItemOpcaoLinkId { get; set; }
    public Guid ImagemId { get; set; }
    public int Ordem { get; set; }
    public bool EhCapa { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public DateTime? DataDelecao { get; set; }

    public ImagemViewModel Imagem { get; set; }
    public PlanoItemOpcaoLinkViewModel PlanoItemOpcaoLink { get; set; }
}
