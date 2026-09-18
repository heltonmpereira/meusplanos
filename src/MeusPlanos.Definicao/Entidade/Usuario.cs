using System;
using System.Collections.Generic;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.Definicao.Entidade;

public class Usuario : IEntidade<Guid>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string CodigoRedefinicaoSenha { get; set; }

    public DateTimeOffset DataCriacao { get; set; }
    public DateTimeOffset? DataAlteracao { get; set; }
    public DateTimeOffset? DataDelecao { get; set; }

    public ICollection<UsuarioPapel> Papeis { get; set; }
}