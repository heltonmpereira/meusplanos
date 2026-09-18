using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Modelo.Usuario;
using MeusPlanos.Modelo.Data;
using MeusPlanos.Modelo.Repositorio.Base;
using Microsoft.EntityFrameworkCore;

namespace MeusPlanos.Modelo.Repositorio;

public class UsuarioRepositorio(IDbContext contexto) : BaseRepositorio<Usuario, Guid>(contexto), IUsuarioRepositorio
{
    public async Task<bool> AlterarSenhaAsync(Usuario model)
    {
        Tabela.Attach(model);
        Contexto.Entry(model).Property(p => p.PasswordHash).IsModified = true;
        Contexto.Entry(model).Property(p => p.CodigoRedefinicaoSenha).IsModified = true;
        await Contexto.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AlterarSenhaComCodigoRedefinicaoAsync(Usuario model)
    {
        Tabela.Attach(model);
        Contexto.Entry(model).Property(p => p.PasswordHash).IsModified = true;
        Contexto.Entry(model).Property(p => p.CodigoRedefinicaoSenha).IsModified = true;
        await Contexto.SaveChangesAsync();

        return true;
    }

    public async Task<UsuarioRedefinirSenha> RedefinirSenhaAsync(Usuario model)
    {
        var codigo = $"{DateTime.Now:yyyyMMdd hhmmssffff}";
        model.CodigoRedefinicaoSenha = Convert.ToBase64String(
            Encoding.ASCII.GetBytes(codigo));

        Tabela.Attach(model);
        Contexto.Entry(model).Property(p => p.CodigoRedefinicaoSenha).IsModified = true;
        await Contexto.SaveChangesAsync();

        var retorno = new UsuarioRedefinirSenha()
        {
            Id = model.Id,
            CodigoRedefinicaoSenha = model.CodigoRedefinicaoSenha,
        };

        return retorno;
    }

    protected override IQueryable<Usuario> AdicionarIncludes(IQueryable<Usuario> source, params object[] args)
    {
        return source
            .Include(i => i.Papeis)
                .ThenInclude(ti => ti.Papel);
    }
}