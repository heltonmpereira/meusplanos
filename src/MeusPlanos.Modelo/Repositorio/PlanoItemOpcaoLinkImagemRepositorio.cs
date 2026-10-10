using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Modelo.Data;
using MeusPlanos.Modelo.Repositorio.Base;
using Microsoft.EntityFrameworkCore;

namespace MeusPlanos.Modelo.Repositorio;

public class PlanoItemOpcaoLinkImagemRepositorio(IDbContext contexto)
    : BaseRepositorio<PlanoItemOpcaoLinkImagem, Guid>(contexto), IPlanoItemOpcaoLinkImagemRepositorio
{
    protected override IQueryable<PlanoItemOpcaoLinkImagem> AdicionarIncludes(
        IQueryable<PlanoItemOpcaoLinkImagem> source, params object[] args)
    {
        return source
            .Include(li => li.Imagem)
            .Include(li => li.PlanoItemOpcaoLink);
    }

    public async Task<IList<PlanoItemOpcaoLinkImagem>> ObterPorLinkIdAsync(Guid planoItemOpcaoLinkId)
    {
        return await Tabela
            .AsNoTracking()
            .Where(w => w.PlanoItemOpcaoLinkId == planoItemOpcaoLinkId && w.DataDelecao == null)
            .Include(li => li.Imagem)
            .OrderBy(o => o.Ordem)
            .ThenByDescending(o => o.DataCriacao)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<int> DefinirComoCapaAsync(Guid linkImagemId)
    {
        var item = await ObterPorIdAsync(linkImagemId).ConfigureAwait(false);
        if (item == null) return 0;

        var todosDoLink = Tabela
            .Where(w => w.PlanoItemOpcaoLinkId == item.PlanoItemOpcaoLinkId && w.DataDelecao == null)
            .ToList();

        foreach (var li in todosDoLink)
            li.EhCapa = li.Id == linkImagemId;

        Contexto.Entry(item).State = EntityState.Unchanged;
        Tabela.UpdateRange(todosDoLink);

        return await Contexto.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<int> ReordenarAsync(Guid linkId, Guid[] idsOrdenados)
    {
        if (idsOrdenados == null || idsOrdenados.Length == 0) return 0;

        var itens = await Tabela
            .Where(w => w.PlanoItemOpcaoLinkId == linkId && w.DataDelecao == null)
            .ToListAsync()
            .ConfigureAwait(false);

        for (var i = 0; i < idsOrdenados.Length; i++)
        {
            var item = itens.FirstOrDefault(f => f.Id == idsOrdenados[i]);
            if (item != null) item.Ordem = i + 1;
        }

        Tabela.UpdateRange(itens);
        return await Contexto.SaveChangesAsync().ConfigureAwait(false);
    }
}
