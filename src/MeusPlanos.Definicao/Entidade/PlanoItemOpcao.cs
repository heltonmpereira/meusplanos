using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.Definicao.Entidade;

public class PlanoItemOpcao : IEntidade<Guid>
{
    public Guid Id { get; set; }

    [Required]
    public Guid PlanoItemId { get; set; }
    public PlanoItem PlanoItem { get; set; }

    [Required]
    [MaxLength(200)]
    public string Nome { get; set; }

    [MaxLength(2000)]
    public string Descricao { get; set; }

    [MaxLength(4000)]
    public string Caracteristicas { get; set; }

    public int Ordem { get; set; }

    public DateTime DataCriacao { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public DateTime? DataDelecao { get; set; }

    public ICollection<PlanoItemOpcaoLink> Links { get; set; }
}
