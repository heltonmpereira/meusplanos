using System;

namespace MeusPlanos.Definicao.Interface.Entidade;

public interface IEntidade<TPK>
{
    TPK Id { get; set; }
    DateTimeOffset DataCriacao { get; set; }
    DateTimeOffset? DataAlteracao { get; set; }
    DateTimeOffset? DataDelecao { get; set; }
}