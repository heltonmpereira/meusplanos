using System;
using System.Collections.Generic;
using System.Linq;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Modelo.Data;
using MeusPlanos.Modelo.Repositorio.Base;
using Microsoft.EntityFrameworkCore;

namespace MeusPlanos.Modelo.Repositorio;

public class PlanoItemOpcaoRepositorio(IDbContext contexto)
    : BaseRepositorio<PlanoItemOpcao, Guid>(contexto), IPlanoItemOpcaoRepositorio
{
    protected override IQueryable<PlanoItemOpcao> AdicionarIncludes(
        IQueryable<PlanoItemOpcao> source,
        params object[] args)
    {
        return source
            .Include(i => i.PlanoItem)
                .ThenInclude(pi => pi.Plano)
                    .ThenInclude(p => p.ImagemCapa)
            .Include(i => i.PlanoItem)
                .ThenInclude(pi => pi.ImagemCapa)
            .Include(i => i.ImagemCapa)
            .Include(i => i.Links)
                .ThenInclude(l => l.ImagensLinks)
                    .ThenInclude(li => li.Imagem);
    }
}
