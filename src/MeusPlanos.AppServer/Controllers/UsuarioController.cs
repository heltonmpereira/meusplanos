using System;
using System.Threading.Tasks;
using Dhani.Utilitarios.Filtro;
using MeusPlanos.AppServer.Controllers.Base;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using MeusPlanos.Servico.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MeusPlanos.AppServer.Controllers
{
    public class UsuarioController(IUsuarioServico servico, IHttpContextAccessor httpContextAccessor) : BaseApiController<Usuario, Guid, IUsuarioServico, IUsuarioRepositorio>(servico, httpContextAccessor)
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

        public override Task<IActionResult> IncluirAsync(Usuario item)
        {
            item.PasswordHash = item.PasswordHash.CriptografarTexto();
            return base.IncluirAsync(item);
        }
    }
}