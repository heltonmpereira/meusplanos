using System;
using System.Collections.Generic;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.Definicao.Entidade;

public class Papel : IEntidade<Guid>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Observacao { get; set; }

    public DateTime DataCriacao { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public DateTime? DataDelecao { get; set; }

    public ICollection<UsuarioPapel> Usuarios { get; set; }
}