using MeusPlanos.Definicao.Interface.Entidade;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeusPlanos.Definicao.Entidade;

public class Imagem : IEntidade<Guid>
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(500)]
    public string NomeOriginal { get; set; }

    [Required]
    [MaxLength(500)]
    public string NomeArquivo { get; set; }

    [Required]
    [MaxLength(1000)]
    public string CaminhoRelativo { get; set; }

    [Required]
    [MaxLength(200)]
    public string ContentType { get; set; }

    public long TamanhoBytes { get; set; }

    [MaxLength(1000)]
    public string Descricao { get; set; }

    public DateTime DataCriacao { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public DateTime? DataDelecao { get; set; }

    public ICollection<PlanoItemOpcaoLinkImagem> LinksImagens { get; set; }
}
