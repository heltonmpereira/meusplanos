using System.Threading.Tasks;
using MeusPlanos.AppCliente.Controllers.Base;
using MeusPlanos.AppCliente.ViewModel.Base;
using MeusPlanos.Definicao.Interface.Entidade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeusPlanos.AppCliente.Areas.Admin.Controllers.Base;

[Authorize(Roles = "admin,usuariologado")]
[Area("Admin")]
public abstract class BaseAdminController<T, TPK, TSERVICO>(TSERVICO servico, IHttpContextAccessor httpContextAccessor) : BaseController<T, TPK, TSERVICO>(servico, httpContextAccessor)
    where T : BaseViewModel, IEntidade<TPK>
{
    [Authorize(Roles = "usuariologado")]
    [HttpPost]
    public override Task<JsonResult> ObterTodos(string prefix)
    {
        return base.ObterTodos(prefix);
    }
}