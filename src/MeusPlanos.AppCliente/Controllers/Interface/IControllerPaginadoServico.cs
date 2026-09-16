using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MeusPlanos.AppCliente.Controllers.Interface
{
    public interface IControllerPaginadoServico<TSERVICO> : IControllerServico<TSERVICO>
    {
        List<SelectListItem> ObterColunasFiltro(Type tipo, string prefixo = null, int nivel = 0);
    }
}