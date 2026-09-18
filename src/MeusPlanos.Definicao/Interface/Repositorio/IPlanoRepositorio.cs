using System;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio.Base;

namespace MeusPlanos.Definicao.Interface.Repositorio;

public interface IPlanoRepositorio : IRepositorio<Plano, Guid>
{
}
