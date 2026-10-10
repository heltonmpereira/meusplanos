using Refit;
using MeusPlanos.AppCliente.ViewModel;
using MeusPlanos.Definicao.Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MeusPlanos.AppCliente.Refit;

public interface IImagemRefit
{
    [Get("/")]
    Task<RespostaPaginadaServico<ImagemViewModel>> Navegar(string criterioJson, bool exibirRegistrosDeletados);

    [Get("/{id}")]
    Task<RespostaServico<ImagemViewModel>> ObterPorId(Guid id);

    [Get("/ObterPorIds")]
    Task<RespostaServico<IList<ImagemViewModel>>> ObterPorIds([Query(CollectionFormat.Multi)] Guid[] ids);

    [Multipart]
    [Post("/Upload")]
    Task<RespostaServico<ImagemViewModel>> Upload(
        [AliasAs("arquivo")] StreamPart arquivo,
        [AliasAs("descricao")] string descricao = null);

    [Delete("/{id}")]
    Task<RespostaServico<int>> Deletar(Guid id);

    [Delete("/{id}/ExcluirComArquivo")]
    Task<RespostaServico<int>> ExcluirComArquivo(Guid id);
}
