using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio.Base;

namespace MeusPlanos.Definicao.Interface.Repositorio;

public interface IPlanoItemOpcaoLinkImagemRepositorio : IRepositorio<PlanoItemOpcaoLinkImagem, Guid>
{
    Task<IList<PlanoItemOpcaoLinkImagem>> ObterPorLinkIdAsync(Guid planoItemOpcaoLinkId);
    Task<int> DefinirComoCapaAsync(Guid linkImagemId);
    Task<int> ReordenarAsync(Guid linkId, Guid[] idsOrdenados);
}
