using System;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.Definicao.Entidade;

public class UsuarioPapel : IEntidade<Guid>
{
    public Guid Id { get; set; }
    public Guid PapelId { get; set; }
    public Guid UsuarioId { get; set; }

    public DateTime DataCriacao { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public DateTime? DataDelecao { get; set; }

    public Papel Papel { get; set; }
    public Usuario Usuario { get; set; }
}