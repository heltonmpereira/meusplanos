using System;
using MeusPlanos.AppServer.Controllers.Base;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dhani.Utilitarios.Filtro;
using Newtonsoft.Json;

namespace MeusPlanos.AppServer.Controllers;

public class PlanoController(IPlanoServico servico, IHttpContextAccessor httpContextAccessor) 
    : BaseApiController<Plano, Guid, IPlanoServico, IPlanoRepositorio>(servico, httpContextAccessor)
{

    protected override ICriterio ObterFiltroInicial(string criterioJson)
    {
        var objFiltro = string.IsNullOrWhiteSpace(criterioJson)
            ? null
            : JsonConvert.DeserializeObject<FiltroBase>(criterioJson);

        objFiltro?.RenderizarValor();
        var criterio = objFiltro ?? new FiltroBase();

        return criterio;
    }

}
