using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dhani.Utilitarios.Filtro;
using MeusPlanos.Definicao.Interface.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio.Base;
using MeusPlanos.Definicao.Interface.Servico.Base;
using MeusPlanos.Definicao.Interface.Servico.Resposta;
using MeusPlanos.Definicao.Modelo;

namespace MeusPlanos.Servico.Servico.Base;

public abstract class BaseServico<T, TPK, TRepositorio>(TRepositorio repositorio) : IServico<T, TPK, TRepositorio>
    where T : class, IEntidade<TPK>
    where TRepositorio : IRepositorio<T, TPK>
{
    public TRepositorio Repositorio { get; } = repositorio;

    public virtual async Task<IRespostaServico<int>> DeletarAsync(TPK id)
    {
        return await DeletarRangeAsync([id]).ConfigureAwait(false);
    }
    public virtual async Task<IRespostaServico<int>> DeletarRangeAsync(TPK[] ids)
    {
        var linhasAfetadas = await Repositorio.DeletarRangeAsync(ids).ConfigureAwait(false);
        var retorno = new RespostaServico<int>(linhasAfetadas)
        {
            BemSucedido = linhasAfetadas > 0,
            Mensagem = linhasAfetadas > 0
                ? $"{linhasAfetadas} registro removido com sucesso."
                : "Falha ao remover os registros solicitados."
        };

        return retorno;
    }
    public virtual async Task<IRespostaServico<T>> AlterarAsync(T item)
    {
        var itemDb = await Repositorio.AlterarAsync(item).ConfigureAwait(false);
        var retorno = new RespostaServico<T>(item)
        {
            BemSucedido = itemDb != null,
            Mensagem = itemDb != null
                ? "Registro alterado com sucesso."
                : "Falha ao alterar o registro solicitado."
        };

        return retorno;
    }
    public virtual async Task<IRespostaServico<T>> IncluirAsync(T item)
    {
        var itemDb = await Repositorio.IncluirAsync(item).ConfigureAwait(false);
        var retorno = new RespostaServico<T>(item)
        {
            BemSucedido = itemDb != null,
            Mensagem = itemDb != null
                ? "Registro cadastrado com sucesso."
                : "Falha ao cadastrar o registro solicitado."
        };

        return retorno;
    }
    public virtual async Task<IRespostaServico<T>> ObterPorIdAsync(TPK id, bool adicionarIncludes = true)
    {
        var item = await Repositorio.ObterPorIdAsync(id).ConfigureAwait(false);
        var retorno = new RespostaServico<T>(item)
        {
            BemSucedido = item != null,
            Mensagem = item != null
               ? "Registro localizado com sucesso."
               : "Falha ao localizar o registro solicitado."
        };

        return retorno;
    }

    protected IRespostaPaginadaServico<TP> MontarRespostaPaginada<TP>(Tuple<IList<TP>, int> dados, ICriterio criterio)
    {
        var bemSucedido = dados.Item1 != null;
        var retorno = new RespostaPaginadaServico<TP>(
            dados.Item1,
            criterio.Pagina,
            criterio.ItensPorPagina,
            dados.Item2)
        {
            BemSucedido = bemSucedido,
            Mensagem = bemSucedido
                ? "Itens localizados com sucesso."
                : "Nenhum registro localizado."
        };

        if (criterio.ItensPorPagina > 1)
            retorno.TotalPaginas = (retorno.TotalRegistros / retorno.ItensPorPagina) + 1;
        else
        {
            retorno.TotalPaginas = 1;
            retorno.NumeroPagina = 1;
            retorno.ItensPorPagina = retorno.TotalRegistros == 0 ? 1 : retorno.TotalRegistros;
        }

        return retorno;
    }

    public virtual async Task<IRespostaPaginadaServico<T>> NavegarAsync(ICriterio criterio)
    {
        var itens = await Repositorio.NavegarAsync(criterio).ConfigureAwait(false);
        return MontarRespostaPaginada(itens, criterio);
    }

    #region Dispose
    private bool disposed = false;
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                Repositorio.Dispose();
            }
        }
        disposed = true;
    }
    #endregion
}