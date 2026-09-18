using System;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.Definicao.Entidade;

public class UsuarioPapel : IEntidade<Guid>
{
    public Guid Id { get; set; }
    public Guid PapelId { get; set; }
    public Guid UsuarioId { get; set; }

    public DateTimeOffset DataCriacao { get; set; }
    public DateTimeOffset? DataAlteracao { get; set; }
    public DateTimeOffset? DataDelecao { get; set; }

    public Papel Papel { get; set; }
    public Usuario Usuario { get; set; }
}