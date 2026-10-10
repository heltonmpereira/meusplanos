using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.Definicao.Entidade;

public class PlanoItem : IEntidade<Guid>
{
    public Guid Id { get; set; }

    [Required]
    public Guid PlanoId { get; set; }
    public Plano Plano { get; set; }

    [Required]
    [MaxLength(200)]
    public string Nome { get; set; }

    [MaxLength(2000)]
    public string Descricao { get; set; }

    public decimal ValorOrcamentoEstimado { get; set; }

    public int Ordem { get; set; }

    public Guid? ImagemCapaId { get; set; }
    public Imagem ImagemCapa { get; set; }

    public DateTime DataCriacao { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public DateTime? DataDelecao { get; set; }

    public ICollection<PlanoItemOpcao> Opcoes { get; set; }
}
