using System;
using MeusPlanos.AppCliente.ViewModel.Base;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.AppCliente.ViewModel;

public class UsuarioPapelViewModel : BaseViewModel, IEntidade<Guid>
{
    public Guid Id { get; set; }
    public Guid PapelId { get; set; }
    public Guid UsuarioId { get; set; }

    public DateTime DataCriacao { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public DateTime? DataDelecao { get; set; }

    public PapelViewModel Papel { get; set; }
    public UsuarioViewModel Usuario { get; set; }
}