using System;
using System.Collections.Generic;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.Definicao.Entidade;

public class Papel : IEntidade<Guid>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Observacao { get; set; }

    public DateTimeOffset DataCriacao { get; set; }
    public DateTimeOffset? DataAlteracao { get; set; }
    public DateTimeOffset? DataDelecao { get; set; }

    public ICollection<UsuarioPapel> Usuarios { get; set; }
}