using System;
using System.Threading.Tasks;
using Dhani.Utilitarios.Filtro;
using MeusPlanos.Definicao.Interface.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio.Base;
using MeusPlanos.Definicao.Interface.Servico.Resposta;

namespace MeusPlanos.Definicao.Interface.Servico.Base;

public interface IServico<T, TPK, TRepositorio> : IDisposable
    where T : class, IEntidade<TPK>
    where TRepositorio : IRepositorio<T, TPK>
{
    TRepositorio Repositorio { get; }

    Task<IRespostaServico<int>> DeletarRangeAsync(TPK[] ids);
    Task<IRespostaServico<int>> DeletarAsync(TPK id);
    Task<IRespostaServico<T>> AlterarAsync(T item);
    Task<IRespostaPaginadaServico<T>> NavegarAsync(ICriterio criterio);
    Task<IRespostaServico<T>> IncluirAsync(T item);
    Task<IRespostaServico<T>> ObterPorIdAsync(TPK id, bool adicionarIncludes = true);
}