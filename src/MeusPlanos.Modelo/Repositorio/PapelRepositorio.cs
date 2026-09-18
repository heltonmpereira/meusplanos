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

public class PapelRepositorio(IDbContext contexto) : BaseRepositorio<Papel, Guid>(contexto), IPapelRepositorio
{
    public async Task<Papel> AtualizarUsuariosAsync(Guid id, Guid[] idsUsuario)
    {
        var itemDb = await ObterPorIdAsync(id);

        var itensRemovidos = new List<UsuarioPapel>();
        var itensAdicionados = new List<UsuarioPapel>();

        foreach (var userPapel in itemDb.Usuarios)
            if (!idsUsuario.Contains(userPapel.UsuarioId))
                itensRemovidos.Add(userPapel);

        foreach (var idUsuario in idsUsuario.Distinct())
            if (!itemDb.Usuarios.Any(a => a.UsuarioId == idUsuario))
                itensAdicionados.Add(new UsuarioPapel()
                {
                    PapelId = id,
                    UsuarioId = idUsuario
                });

        var tabPapeis = Contexto.Set<UsuarioPapel>();
        tabPapeis.RemoveRange(itensRemovidos);
        await tabPapeis.AddRangeAsync(itensAdicionados);

        await Contexto.SaveChangesAsync();

        return itemDb;
    }

    protected override IQueryable<Papel> AdicionarIncludes(IQueryable<Papel> source, params object[] args)
    {
        return source
            .Include(i => i.Usuarios)
                .ThenInclude(ti => ti.Usuario);
    }
}