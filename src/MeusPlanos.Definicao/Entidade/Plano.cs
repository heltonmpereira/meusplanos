using System;
using MeusPlanos.Definicao.Interface.Entidade;
using MeusPlanos.Definicao.Modelo.Enum;

namespace MeusPlanos.Definicao.Entidade;

public class Plano : IEntidade<Guid>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public TipoPlano Tipo { get; set; }               // Imovel, Viagem, Ferias, Veiculo, Livre
    public decimal? ValorEstimadoManual { get; set; } // teto/chute inicial
    public DateOnly DataAlvo { get; set; }
    public StatusPlano Status { get; set; }
    public string Moeda { get; set; } = "BRL";

    public DateTime DataCriacao { get; set; }
    public DateTime? DataAlteracao { get; set; }
    public DateTime? DataDelecao { get; set; }

    //public ICollection<GrupoPlano> Grupos { get; set; }
    //public ICollection<Aporte> Aportes { get; set; }
}

//TODO: executar a atualização da base de dados após a criação do módulo, por exemplo:
//add-migration PlanoMigration -Project 02-Servidor\MeusPlanos.Modelo -StartupProject 02-Servidor\MeusPlanos.AppServer

