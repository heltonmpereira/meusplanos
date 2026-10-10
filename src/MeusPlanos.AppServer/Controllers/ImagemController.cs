using System;
using System.IO;
using System.Threading.Tasks;
using Dhani.Utilitarios.Filtro;
using MeusPlanos.AppServer.Controllers.Base;
using MeusPlanos.AppServer.Helper;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using MeusPlanos.Definicao.Interface.Servico.Resposta;
using MeusPlanos.Definicao.Modelo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeusPlanos.AppServer.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ImagemController(
    IImagemServico servico,
    IHttpContextAccessor httpContextAccessor) : PadraoApiController<IImagemServico>(servico, httpContextAccessor)
{
    [HttpGet]
    public virtual async Task<IActionResult> NavegarAsync(string criterioJson, bool exibirRegistrosDeletados = false)
    {
        var criterio = ObterFiltroInicial(criterioJson);
        if (!exibirRegistrosDeletados)
        {
            criterio.AdicionarGrupo(new GrupoFiltro("Deletados", [
                new FiltroOpcao
                {
                    NomePropriedade = "DataDelecao",
                    Operador = TipoOperadorBusca.Igual,
                    Valor = null
                }
            ]) { RelacaoOutrosGrupos = TipoOperadorLogico.And });
        }
        return await Servico.RespostaPaginadaServicoAsync<Imagem>(criterio: criterio);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorIdAsync(Guid id) =>
        await Servico.RespostaServicoAsync<Imagem>(parameters: [id, true]);

    [HttpGet("ObterPorIds")]
    public async Task<IActionResult> ObterPorIdsAsync([FromQuery] Guid[] ids) =>
        await Servico.RespostaServicoAsync<System.Collections.Generic.IList<Imagem>>(parameters: [ids]);

    [HttpPost("Upload")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> UploadAsync(IFormFile arquivo, string descricao = null)
    {
        try
        {
            if (arquivo == null || arquivo.Length == 0)
                return BadRequest(new RespostaServico<Imagem>(null, "Arquivo não enviado.") { BemSucedido = false });
            using var stream = arquivo.OpenReadStream();
            var resultado = await Servico.UploadAsync(stream, arquivo.FileName, arquivo.ContentType, arquivo.Length, descricao).ConfigureAwait(false);
            return resultado == null || !resultado.BemSucedido
                ? BadRequest(resultado)
                : Ok(resultado);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new RespostaServico<Exception>(e, e.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarAsync(Guid id) =>
        await Servico.RespostaServicoAsync<int>(parameters: [id]);

    [HttpDelete("{id}/ExcluirComArquivo")]
    public async Task<IActionResult> ExcluirComArquivoAsync(Guid id) =>
        await Servico.RespostaServicoAsync<int>(nomeMetodo: nameof(IImagemServico.ExcluirComArquivoAsync), parameters: [id]);
}
