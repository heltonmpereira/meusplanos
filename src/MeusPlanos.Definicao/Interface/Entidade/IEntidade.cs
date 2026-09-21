using System;

namespace MeusPlanos.Definicao.Interface.Entidade;

public interface IEntidade<TPK>
{
    TPK Id { get; set; }
    DateTime DataCriacao { get; set; }
    DateTime? DataAlteracao { get; set; }
    DateTime? DataDelecao { get; set; }
}