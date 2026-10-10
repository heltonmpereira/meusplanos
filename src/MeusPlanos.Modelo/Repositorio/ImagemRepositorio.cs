using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Modelo.Data;
using MeusPlanos.Modelo.Repositorio.Base;
using Microsoft.EntityFrameworkCore;

namespace MeusPlanos.Modelo.Repositorio;

public class ImagemRepositorio(IDbContext contexto) : BaseRepositorio<Imagem, Guid>(contexto), IImagemRepositorio
{
    protected override IQueryable<Imagem> AdicionarIncludes(IQueryable<Imagem> source, params object[] args)
    {
        return source
            .Include(i => i.LinksImagens)
                .ThenInclude(li => li.PlanoItemOpcaoLink);
    }

    public async Task<IList<Imagem>> ObterPorIdsAsync(Guid[] ids)
    {
        if (ids == null || ids.Length == 0) return Array.Empty<Imagem>();

        return await Tabela
            .AsNoTracking()
            .Where(w => ids.Contains(w.Id) && w.DataDelecao == null)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<Imagem> UploadAsync(Imagem imagem, byte[] conteudo, string pastaRaiz)
    {
        if (imagem == null) throw new ArgumentNullException(nameof(imagem));
        if (conteudo == null || conteudo.Length == 0) throw new ArgumentException("Conteúdo vazio.", nameof(conteudo));
        if (string.IsNullOrWhiteSpace(pastaRaiz)) throw new ArgumentException("Pasta raiz inválida.", nameof(pastaRaiz));

        var caminhoCompleto = Path.Combine(pastaRaiz, imagem.CaminhoRelativo.TrimStart('\\', '/'));
        var diretorio = Path.GetDirectoryName(caminhoCompleto);

        if (!string.IsNullOrWhiteSpace(diretorio) && !Directory.Exists(diretorio))
            Directory.CreateDirectory(diretorio);

        await File.WriteAllBytesAsync(caminhoCompleto, conteudo).ConfigureAwait(false);

        return await IncluirAsync(imagem).ConfigureAwait(false);
    }

    public async Task<bool> ExcluirArquivoFisicoAsync(Guid imagemId, string pastaRaiz)
    {
        var imagem = await ObterPorIdAsync(imagemId).ConfigureAwait(false);
        if (imagem == null) return false;

        var caminhoCompleto = Path.Combine(pastaRaiz, imagem.CaminhoRelativo.TrimStart('\\', '/'));
        if (!File.Exists(caminhoCompleto)) return true;

        try
        {
            File.Delete(caminhoCompleto);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
