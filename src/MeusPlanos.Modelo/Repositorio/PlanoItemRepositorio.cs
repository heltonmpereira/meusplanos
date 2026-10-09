using System;
using System.Collections.Generic;
using System.Linq;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Modelo.Data;
using MeusPlanos.Modelo.Repositorio.Base;
using Microsoft.EntityFrameworkCore;

namespace MeusPlanos.Modelo.Repositorio;

public class PlanoItemRepositorio(IDbContext contexto)
    : BaseRepositorio<PlanoItem, Guid>(contexto), IPlanoItemRepositorio
{
    protected override IQueryable<PlanoItem> AdicionarIncludes(
        IQueryable<PlanoItem> source,
        params object[] args)
    {
        return source
            .Include(i => i.Opcoes)
                .ThenInclude(o => o.Links);
    }
}
