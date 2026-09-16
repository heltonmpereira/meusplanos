using System;
using MeusPlanos.AppCliente.Controllers.Base;
using MeusPlanos.AppCliente.Filter;
using MeusPlanos.AppCliente.Refit;
using MeusPlanos.AppCliente.ViewModel;
using Microsoft.AspNetCore.Http;

namespace MeusPlanos.AppCliente.Controllers
{
    [BreadcrumbActionFilter]
    public class WeatherForecastController(IWeatherForecastRefit servico, IHttpContextAccessor httpContextAccessor) : BaseController<WeatherForecastViewModel, Guid, IWeatherForecastRefit>(servico, httpContextAccessor)
    {
    }
}