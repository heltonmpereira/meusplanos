using System;
using System.Threading.Tasks;
using Dhani.Utilitarios.Filtro;
using MeusPlanos.AppServer.Controllers.Base;
using MeusPlanos.AppServer.Helper;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MeusPlanos.AppServer.Controllers;

public class PapelController(IPapelServico servico, IHttpContextAccessor httpContextAccessor) : BaseApiController<Papel, Guid, IPapelServico, IPapelRepositorio>(servico, httpContextAccessor)
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

    [HttpPut("atualizar-usuarios")]
    public async Task<IActionResult> AtualizarUsuariosAsync(Guid id, Guid[] idsUsuario) =>
        await Servico.RespostaServicoAsync<Papel>(
            parameters: [id, idsUsuario]);
}