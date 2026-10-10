using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.Definicao.Entidade;

public class PlanoItemOpcaoLink : IEntidade<Guid>
{
    public Guid Id { get; set; }

    [Required]
    public Guid PlanoItemOpcaoId { get; set; }
    public PlanoItemOpcao PlanoItemOpcao { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Url { get; set; }

    [MaxLength(200)]
    public string NomeSite { get; set; }

    public decimal? Valor { get; set; }

    [MaxLength(2000)]
    public string CondicoesPagamento { get; set; }

    [MaxLength(4000)]
    public string Observacoes { get; set; }

    public DateTime? DataCotacao { get; set; }

    public DateTime DataCriacao { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public DateTime? DataDelecao { get; set; }

    public ICollection<PlanoItemOpcaoLinkImagem> ImagensLinks { get; set; }
}
