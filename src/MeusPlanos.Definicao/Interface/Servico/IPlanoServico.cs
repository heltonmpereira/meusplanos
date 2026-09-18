using System;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico.Base;

namespace MeusPlanos.Definicao.Interface.Servico;

public interface IPlanoServico : IServico<Plano, Guid, IPlanoRepositorio>
{
}
