using System;
using System.Linq;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Modelo.Data;
using MeusPlanos.Modelo.Repositorio.Base;
using Microsoft.EntityFrameworkCore;

namespace MeusPlanos.Modelo.Repositorio;

public class PlanoRepositorio(IDbContext contexto)
    : BaseRepositorio<Plano, Guid>(contexto), IPlanoRepositorio
{
    protected override IQueryable<Plano> AdicionarIncludes(
        IQueryable<Plano> source,
        params object[] args)
    {
        return source
            .Include(i => i.Proprietario)
            .Include(i => i.ImagemCapa)
            .Include(i => i.Itens)
                .ThenInclude(it => it.ImagemCapa)
            .Include(i => i.Itens)
                .ThenInclude(it => it.Opcoes)
                    .ThenInclude(o => o.ImagemCapa)
            .Include(i => i.Itens)
                .ThenInclude(it => it.Opcoes)
                    .ThenInclude(o => o.Links)
                        .ThenInclude(l => l.ImagensLinks)
                            .ThenInclude(li => li.Imagem);
    }
}
