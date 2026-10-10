using System;
using System.ComponentModel.DataAnnotations;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.Definicao.Entidade;

public class PlanoItemOpcaoLinkImagem : IEntidade<Guid>
{
    public Guid Id { get; set; }

    [Required]
    public Guid PlanoItemOpcaoLinkId { get; set; }
    public PlanoItemOpcaoLink PlanoItemOpcaoLink { get; set; }

    [Required]
    public Guid ImagemId { get; set; }
    public Imagem Imagem { get; set; }

    public int Ordem { get; set; }

    public bool EhCapa { get; set; }

    public DateTime DataCriacao { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public DateTime? DataDelecao { get; set; }
}
