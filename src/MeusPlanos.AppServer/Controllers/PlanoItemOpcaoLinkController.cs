using System;
using Dhani.Utilitarios.Filtro;
using MeusPlanos.AppServer.Controllers.Base;
using MeusPlanos.AppServer.Modelo;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using Microsoft.AspNetCore.Http;

namespace MeusPlanos.AppServer.Controllers;

public class PlanoItemOpcaoLinkController(IPlanoItemOpcaoLinkServico servico, IHttpContextAccessor httpContextAccessor)
    : BaseApiController<PlanoItemOpcaoLink, Guid, IPlanoItemOpcaoLinkServico, IPlanoItemOpcaoLinkRepositorio>(servico, httpContextAccessor)
{
    protected override TipoFiltroRegistroEnum FiltroRegistros { get; set; } = TipoFiltroRegistroEnum.NaoFiltrarUsuario;

    protected override ICriterio ObterFiltroInicial(string criterioJson)
    {
        var criterio = base.ObterFiltroInicial(criterioJson);
        criterio ??= new FiltroBase();

        var filtroProprietario = new FiltroOpcao(
            "PlanoItemOpcao.PlanoItem.Plano.ProprietarioId",
            TipoOperadorBusca.Igual,
            IdUsuarioLogado,
            false,
            TipoOperadorLogico.And);

        var grupoSeguranca = new GrupoFiltro("SegurancaPlanoOpcaoLink", [filtroProprietario])
        {
            RelacaoOutrosGrupos = TipoOperadorLogico.And
        };

        criterio.AdicionarGrupo(grupoSeguranca);
        return criterio;
    }
}
