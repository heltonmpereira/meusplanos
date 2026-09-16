using System;
using System.Net;
using Dhani.Utilitarios.Filtro;
using Dhani.Utilitarios.Helper;
using MeusPlanos.AppServer.Modelo;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Helper;
using MeusPlanos.Definicao.Interface.Servico.Resposta;
using MeusPlanos.Definicao.Modelo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MeusPlanos.AppServer.Controllers.Base
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PadraoApiController<TServico> : ControllerBase
    {
        protected TServico Servico { get; }
        protected string IdUsuarioLogado { get; }
        protected virtual TipoFiltroRegistroEnum FiltroRegistros { get; set; } = TipoFiltroRegistroEnum.FiltrarPorUsuarioLogado;
        public IHttpContextAccessor HttpContextAccessor { get; }

        private PadraoApiController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
            IdUsuarioLogado = httpContextAccessor.HttpContext?
                .User
                .FindFirst("UsuarioId")?
                .Value;
        }
        protected PadraoApiController(TServico servico, IHttpContextAccessor httpContextAccessor) : this(httpContextAccessor)
        {
            Servico = servico;
        }

        protected virtual ICriterio ObterFiltroInicial(string criterioJson)
        {
            var criterio = string.IsNullOrWhiteSpace(criterioJson)
                ? new FiltroBase()
                : JsonConvert.DeserializeObject<FiltroBase>(criterioJson);

            criterio?.RenderizarValor();

            if (FiltroRegistros == TipoFiltroRegistroEnum.FiltrarPorUsuarioLogado)
                criterio.AdicionarFiltroProprietario(IdUsuarioLogado);
            else if (FiltroRegistros == TipoFiltroRegistroEnum.FiltrarPorUsuarioLogadoOuSemUsuarioEspecificado)
                criterio.AdicionarFiltroProprietarioOpcional(IdUsuarioLogado);

            return criterio;
        }

        protected void DefinirUsuarioLogado<TEntidade>(TEntidade item)
        {
            const string nomePropriedade = "ProprietarioId";
            var prop = item.GetType().GetProperty(nomePropriedade);
            if (prop == null) return;

            if (Nullable.GetUnderlyingType(prop.PropertyType) == null)
                item.SetValue(nomePropriedade, IdUsuarioLogado);
        }

        protected IActionResult ConfrontarUsuarioLogadoEProprietario<TEntidade>(
            IActionResult respostaServico,
            string nomePropriedade = "ProprietarioId")
        {
            if (respostaServico is OkObjectResult retornoOk)
            {
                var resp = retornoOk.Value as IRespostaServico<TEntidade>;
                var propProprietarioId = resp.Dados.GetValue(nomePropriedade);

                if (propProprietarioId != null)
                {
                    var proprietarioId = (Guid)propProprietarioId;
                    if (proprietarioId != new Guid(IdUsuarioLogado))
                    {
                        var retorno = new RespostaServico<Usuario>(null, "Falha ao validar informações do usuário")
                        {
                            BemSucedido = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            MensagemErro = "Falha ao validar informações do usuário"
                        };

                        return BadRequest(retorno);
                    }
                }
            }

            return respostaServico;
        }
    }
}
