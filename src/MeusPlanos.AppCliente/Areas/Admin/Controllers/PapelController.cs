using System;
using System.Linq;
using System.Threading.Tasks;
using Dhani.Utilitarios.Filtro;
using Dhani.Utilitarios.Helper;
using MeusPlanos.AppCliente.Helper;
using MeusPlanos.AppCliente.Refit;
using MeusPlanos.AppCliente.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeusPlanos.AppCliente.Areas.Admin.Controllers;

public class PapelController(IPapelRefit servico, IUsuarioRefit usuarioServico, IHttpContextAccessor httpContextAccessor) : Base.BaseAdminController<PapelViewModel, Guid, IPapelRefit>(servico, httpContextAccessor)
{
    private readonly IUsuarioRefit _usuarioServico = usuarioServico;

    public async Task<IActionResult> Usuarios(Guid id) =>
        await this.ChamarServicoView<PapelViewModel, Guid, IPapelRefit>(id, "ObterPorId");

    [HttpPost]
    public async Task<IActionResult> Usuarios(string id, string idUsuarios)
    {
        Guid[] guidUsuarios = null;
        if (!string.IsNullOrWhiteSpace(idUsuarios))
            guidUsuarios = idUsuarios
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .Select(s => new Guid(s))
                .ToArray();

        var view = await this.ChamarServicoProsseguirIndex<PapelViewModel, Guid, IPapelRefit>(
            model: new object[] { new Guid(id), guidUsuarios },
            apiMetodo: nameof(IPapelRefit.AtualizarUsuarios));

        return view;
    }

    [HttpPost]
    public async Task<JsonResult> ObterUsuariosDisponiveis(string nomeOuEmail = null)
    {
        var criterio = new FiltroBase();
        if (!string.IsNullOrWhiteSpace(nomeOuEmail))
        {
            var filtroNome = new FiltroOpcao("nome", TipoOperadorBusca.Contem, nomeOuEmail, false);
            var filtroEmail = new FiltroOpcao("email", TipoOperadorBusca.Contem, nomeOuEmail, false);
            filtroNome.RelacaoOutrosFiltros = TipoOperadorLogico.Or;

            var filtros = new GrupoFiltro(
                "padrao",
                [filtroNome, filtroEmail]);

            criterio.AdicionarGrupo(filtros);
        }

        var usuarios = await _usuarioServico.Navegar(criterio.ToJson(), false);

        return Json(usuarios.Dados);
    }
}