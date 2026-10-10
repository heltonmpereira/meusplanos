using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico.Base;
using MeusPlanos.Definicao.Interface.Servico.Resposta;

namespace MeusPlanos.Definicao.Interface.Servico;

public interface IPlanoItemOpcaoLinkImagemServico : IServico<PlanoItemOpcaoLinkImagem, Guid, IPlanoItemOpcaoLinkImagemRepositorio>
{
    Task<IRespostaServico<IList<PlanoItemOpcaoLinkImagem>>> ObterPorLinkIdAsync(Guid planoItemOpcaoLinkId);
    Task<IRespostaServico<PlanoItemOpcaoLinkImagem>> VincularImagemAsync(Guid planoItemOpcaoLinkId, Guid imagemId, int? ordem = null, bool ehCapa = false);
    Task<IRespostaServico<int>> DesvincularImagemAsync(Guid linkImagemId, bool excluirArquivo = false);
    Task<IRespostaServico<int>> DefinirComoCapaAsync(Guid linkImagemId);
    Task<IRespostaServico<int>> ReordenarAsync(Guid linkId, Guid[] idsOrdenados);
}
