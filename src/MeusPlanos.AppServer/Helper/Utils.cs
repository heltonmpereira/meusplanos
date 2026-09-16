using Dhani.Utilitarios.Filtro;
using MeusPlanos.Definicao.Interface.Servico.Resposta;
using Microsoft.AspNetCore.Mvc;

namespace MeusPlanos.AppServer.Helper
{
    public static class Utils
    {
        public static void DefinirCodigoStatus<T>(ref IActionResult resposta)
        {
            resposta.DefinirCodigoStatus<T>();
        }

        public static void DefinirCodigoStatus<T>(this IActionResult resposta)
        {
            var objResult = (ObjectResult)resposta;

            if (objResult != null && objResult.Value is IRespostaServico<T> resp)
            {
                resp.StatusCode ??= (System.Net.HttpStatusCode)objResult.StatusCode;

                if (resp.StatusCode >= System.Net.HttpStatusCode.BadRequest
                 || resp.Mensagem == "A busca não encontrou nenhum registro.")
                    resp.BemSucedido = false;
            }
        }

        public static ICriterio AdicionarFiltroProprietario(
            ICriterio criterio,
            string idUsuarioLogado,
            string nomePropriedade = "ProprietarioId")
        {
            var filtroUsuarioLogado = new FiltroOpcao(
                nomePropriedade,
                TipoOperadorBusca.Igual,
                idUsuarioLogado,
                false,
                TipoOperadorLogico.Or);

            criterio ??= new FiltroBase();
            var grupo = new GrupoFiltro("Sistema", new[] { filtroUsuarioLogado });
            criterio.AdicionarGrupo(grupo);

            return criterio;
        }

        public static ICriterio AdicionarFiltroProprietarioOpcional(
            ICriterio criterio,
            string idUsuarioLogado,
            string nomePropriedade = "ProprietarioId")
        {
            var filtroUsuarioLogado = new FiltroOpcao(
                nomePropriedade,
                TipoOperadorBusca.Igual,
                idUsuarioLogado,
                false,
                TipoOperadorLogico.Or);

            var filtroUsuarioNulo = new FiltroOpcao(
                nomePropriedade,
                TipoOperadorBusca.Igual,
                null,
                false,
                TipoOperadorLogico.Or);

            var grp = new GrupoFiltro("Proprietario")
            {
                RelacaoEntreFiltros = TipoOperadorLogico.Or
            };
            grp.AdicionarFiltro(filtroUsuarioNulo);
            grp.AdicionarFiltro(filtroUsuarioLogado);

            criterio ??= new FiltroBase();
            criterio.AdicionarGrupo(grp);

            return criterio;
        }
    }
}