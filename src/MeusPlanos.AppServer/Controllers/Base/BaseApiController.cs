using System.Threading.Tasks;
using Dhani.Utilitarios.Filtro;
using MeusPlanos.AppServer.Helper;
using MeusPlanos.Definicao.Interface.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio.Base;
using MeusPlanos.Definicao.Interface.Servico.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeusPlanos.AppServer.Controllers.Base
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController<TEntidade, TPK, TServico, TRepositorio>(
        TServico servico,
        IHttpContextAccessor httpContextAccessor) : PadraoApiController<TServico>(servico, httpContextAccessor)
        where TEntidade : class, IEntidade<TPK>
        where TRepositorio : IRepositorio<TEntidade, TPK>
        where TServico : IServico<TEntidade, TPK, TRepositorio>
    {
        [HttpGet]
        public virtual async Task<IActionResult> NavegarAsync(string criterioJson, bool exibirRegistrosDeletados = false)
        {
            var filtroInicial = ObterFiltroInicial(criterioJson);

            if (!exibirRegistrosDeletados)
            {
                var grupoRegistrosDeletados = new GrupoFiltro("Deletados", [
                    new FiltroOpcao(){
                        NomePropriedade = "Deletado",
                        Valor = false,
                        Operador = TipoOperadorBusca.Igual,
                        RelacaoOutrosFiltros = TipoOperadorLogico.And
                    }
                ])
                {
                    RelacaoOutrosGrupos = TipoOperadorLogico.And
                };

                filtroInicial.AdicionarGrupo(grupoRegistrosDeletados);
            }

            return await Servico.RespostaPaginadaServicoAsync<TEntidade>(
                criterio: filtroInicial);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorIdAsync(TPK id)
        {
            var resposta = await Servico.RespostaServicoAsync<TEntidade>(
                parameters: [id, true]);

            return ConfrontarUsuarioLogadoEProprietario<TEntidade>(resposta);
        }

        [HttpPost]
        public virtual async Task<IActionResult> IncluirAsync(TEntidade item)
        {
            DefinirUsuarioLogado(item);
            return await Servico.RespostaServicoAsync<TEntidade>(
                parameters: [item]);
        }

        [HttpPut]
        public async Task<IActionResult> AlterarAsync(TEntidade item)
        {
            DefinirUsuarioLogado(item);
            return await Servico.RespostaServicoAsync<TEntidade>(
                parameters: [item]);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarAsync(TPK id) =>
            await Servico.RespostaServicoAsync<int>(
                parameters: [id]);

        [HttpDelete("DeletarRange")]
        public async Task<IActionResult> DeletarRangeAsync(TPK[] ids) =>
            await Servico.RespostaServicoAsync<int>(
                parameters: [ids]);
    }
}