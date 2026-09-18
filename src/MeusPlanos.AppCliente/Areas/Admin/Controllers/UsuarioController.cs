using System;
using MeusPlanos.AppCliente.Refit;
using MeusPlanos.AppCliente.ViewModel;
using Microsoft.AspNetCore.Http;

namespace MeusPlanos.AppCliente.Areas.Admin.Controllers;

public class UsuarioController(IUsuarioRefit servico, IHttpContextAccessor httpContextAccessor) : Base.BaseAdminController<UsuarioViewModel, Guid, IUsuarioRefit>(servico, httpContextAccessor)
{
}