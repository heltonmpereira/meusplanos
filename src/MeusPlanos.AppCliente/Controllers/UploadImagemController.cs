using System;
using System.IO;
using System.Threading.Tasks;
using MeusPlanos.AppCliente.Controllers.Base;
using MeusPlanos.AppCliente.Refit;
using MeusPlanos.Definicao.Modelo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace MeusPlanos.AppCliente.Controllers;

[Microsoft.AspNetCore.Authorization.Authorize]
public class UploadImagemController : PadraoController
{
    [HttpPost]
    public async Task<JsonResult> UploadImagem(IFormFile arquivo, string descricao = null)
    {
        try
        {
            if (arquivo == null || arquivo.Length == 0)
                return Json(new { sucesso = false, mensagem = "Arquivo não enviado." });

            var imgRefit = HttpContext.RequestServices.GetRequiredService<IImagemRefit>();
            using var stream = arquivo.OpenReadStream();
            var streamPart = new StreamPart(stream, arquivo.FileName, arquivo.ContentType);

            RespostaServico<ViewModel.ImagemViewModel> resposta;
            try
            {
                resposta = await imgRefit.Upload(streamPart, descricao).ConfigureAwait(false);
            }
            finally
            {
                stream.Dispose();
            }

            if (resposta == null || !resposta.BemSucedido)
                return Json(new { sucesso = false, mensagem = resposta?.Mensagem ?? "Falha ao enviar imagem." });

            var img = resposta.Dados;
            var baseUrl = HttpContext.RequestServices.GetRequiredService<IConfiguration>()["BaseUrlApi"]?.TrimEnd('/');
            var url = !string.IsNullOrWhiteSpace(img?.CaminhoRelativo) && !string.IsNullOrWhiteSpace(baseUrl)
                ? $"{baseUrl}/uploads/imagens/{img.CaminhoRelativo.Replace('\\', '/')}"
                : null;

            return Json(new
            {
                sucesso = true,
                id = img?.Id,
                nomeOriginal = img?.NomeOriginal,
                tamanho = img?.TamanhoFormatado,
                url
            });
        }
        catch (ApiException ex)
        {
            return Json(new { sucesso = false, mensagem = ex.Content ?? ex.Message });
        }
        catch (Exception ex)
        {
            return Json(new { sucesso = false, mensagem = ex.Message });
        }
    }
}
