using System;
using System.Collections.Generic;
using System.Linq;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Modelo.Data;
using MeusPlanos.Modelo.Repositorio.Base;
using Microsoft.EntityFrameworkCore;

namespace MeusPlanos.Modelo.Repositorio;

public class PlanoItemOpcaoLinkRepositorio(IDbContext contexto)
    : BaseRepositorio<PlanoItemOpcaoLink, Guid>(contexto), IPlanoItemOpcaoLinkRepositorio
{
    protected override IQueryable<PlanoItemOpcaoLink> AdicionarIncludes(
        IQueryable<PlanoItemOpcaoLink> source,
        params object[] args)
    {
        return source
            .Include(i => i.PlanoItemOpcao);
    }
}
