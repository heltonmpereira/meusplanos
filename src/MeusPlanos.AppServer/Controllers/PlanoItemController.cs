using System;
using Dhani.Utilitarios.Filtro;
using MeusPlanos.AppServer.Controllers.Base;
using MeusPlanos.AppServer.Modelo;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using Microsoft.AspNetCore.Http;

namespace MeusPlanos.AppServer.Controllers;

public class PlanoItemController(IPlanoItemServico servico, IHttpContextAccessor httpContextAccessor)
    : BaseApiController<PlanoItem, Guid, IPlanoItemServico, IPlanoItemRepositorio>(servico, httpContextAccessor)
{
    protected override TipoFiltroRegistroEnum FiltroRegistros { get; set; } = TipoFiltroRegistroEnum.NaoFiltrarUsuario;

    protected override ICriterio ObterFiltroInicial(string criterioJson)
    {
        var criterio = base.ObterFiltroInicial(criterioJson);
        criterio ??= new FiltroBase();

        var filtroProprietario = new FiltroOpcao(
            "Plano.ProprietarioId",
            TipoOperadorBusca.Igual,
            IdUsuarioLogado,
            false,
            TipoOperadorLogico.And);

        var grupoSeguranca = new GrupoFiltro("SegurancaPlano", [filtroProprietario])
        {
            RelacaoOutrosGrupos = TipoOperadorLogico.And
        };

        criterio.AdicionarGrupo(grupoSeguranca);
        return criterio;
    }
}
