using System.Linq;
using Dhani.Utilitarios.Filtro;

namespace MeusPlanos.Definicao.Helper;

public static class Utils
{
    public static ICriterio AdicionarFiltroProprietario(
        this ICriterio criterio,
        string idUsuarioLogado,
        string nomePropriedade = "ProprietarioId")
    {
        var filtroUsuarioLogado = new FiltroOpcao(
            nomePropriedade,
            TipoOperadorBusca.Igual,
            idUsuarioLogado,
            false);

        criterio ??= new FiltroBase();
        var grupo = new GrupoFiltro("Sistema", new[] { filtroUsuarioLogado });
        criterio.AdicionarGrupo(grupo);

        return criterio;
    }

    public static ICriterio AdicionarFiltroProprietarioOpcional(
        this ICriterio criterio,
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

    public static ICriterio OrganizarIdFiltros(this ICriterio criterio)
    {
        var idxFiltro = 1;
        foreach (var grupoFiltroFiltro in criterio.GruposFiltro.SelectMany(grupoFiltro => grupoFiltro.Filtros))
            grupoFiltroFiltro.Id = idxFiltro++;

        return criterio;
    }

}