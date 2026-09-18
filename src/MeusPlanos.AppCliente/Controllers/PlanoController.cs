using Microsoft.AspNetCore.Http;
using System;
using MeusPlanos.AppCliente.Controllers.Base;
using MeusPlanos.AppCliente.Filter;
using MeusPlanos.AppCliente.Refit;
using MeusPlanos.AppCliente.ViewModel;


namespace MeusPlanos.AppCliente.Controllers;

[BreadcrumbActionFilter]
public class PlanoController(IPlanoRefit servico, IHttpContextAccessor httpContextAccessor) 
    : BaseController<PlanoViewModel, Guid, IPlanoRefit>(servico, httpContextAccessor);
