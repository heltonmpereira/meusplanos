using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dhani.Utilitarios.Filtro;
using MeusPlanos.Definicao.Interface.Entidade;

namespace MeusPlanos.Definicao.Interface.Repositorio.Base;

public interface IRepositorio<T, TPK> : IDisposable
    where T : class, IEntidade<TPK>
{
    Task<int> DeletarRangeAsync(TPK[] ids);
    Task<int> DeletarAsync(TPK id);
    Task<T> AlterarAsync(T item);
    Task<Tuple<IList<T>, int>> NavegarAsync(ICriterio criterio);
    Task<T> IncluirAsync(T item);
    Task<T> ObterPorIdAsync(TPK id, bool adicionarIncludes = true, params object[] args);
}