using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using MeusPlanos.Definicao.Interface.Servico.Resposta;
using MeusPlanos.Definicao.Modelo;
using MeusPlanos.Servico.Servico.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace MeusPlanos.Servico.Servico;

public class ImagemServico(
    IImagemRepositorio repositorio,
    IWebHostEnvironment webHostEnvironment)
    : BaseServico<Imagem, Guid, IImagemRepositorio>(repositorio), IImagemServico
{
    private const string PastaImagens = "uploads\\imagens";
    private static readonly string[] ExtensoesPermitidas = { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };
    private const long TamanhoMaximoBytes = 10 * 1024 * 1024; // 10MB

    public Task<IRespostaServico<string>> ObterCaminhoFisicoRaizAsync()
    {
        var caminho = Path.Combine(webHostEnvironment.WebRootPath, PastaImagens);
        var resp = new RespostaServico<string>(caminho)
        {
            BemSucedido = true,
            Mensagem = "Caminho localizado."
        };
        return Task.FromResult<IRespostaServico<string>>(resp);
    }

    public async Task<IRespostaServico<Imagem>> UploadAsync(
        Stream conteudo,
        string nomeArquivoOriginal,
        string contentType,
        long tamanhoBytes,
        string descricao = null)
    {
        if (conteudo == null)
            return new RespostaServico<Imagem>(null, "Conteúdo não informado.") { BemSucedido = false };
        if (tamanhoBytes <= 0 && conteudo.CanSeek)
            tamanhoBytes = conteudo.Length;
        if (tamanhoBytes <= 0)
            return new RespostaServico<Imagem>(null, "Arquivo vazio.") { BemSucedido = false };
        if (tamanhoBytes > TamanhoMaximoBytes)
            return new RespostaServico<Imagem>(null,
                $"Tamanho máximo permitido é {TamanhoMaximoBytes / 1024 / 1024}MB.") { BemSucedido = false };

        var extensao = Path.GetExtension(nomeArquivoOriginal)?.ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(extensao) || !ExtensoesPermitidas.Contains(extensao))
            return new RespostaServico<Imagem>(null,
                $"Extensão não permitida. Extensões aceitas: {string.Join(", ", ExtensoesPermitidas)}.")
            { BemSucedido = false };

        var pastaRaiz = (await ObterCaminhoFisicoRaizAsync()).Dados;
        var subPasta = $"{DateTime.Now:yyyy}\\{DateTime.Now:MM}";
        var nomeArquivo = $"{Guid.NewGuid():N}{extensao}";
        var caminhoRelativo = $"{subPasta.Replace('\\', '/')}/{nomeArquivo}";
        var caminhoCompletoPasta = Path.Combine(pastaRaiz, subPasta);

        if (!Directory.Exists(caminhoCompletoPasta))
            Directory.CreateDirectory(caminhoCompletoPasta);

        byte[] bytes;
        if (conteudo is MemoryStream ms && ms.TryGetBuffer(out var seg))
            bytes = seg.Array == null ? ms.ToArray() : seg.ToArray();
        else
        {
            using var copia = new MemoryStream();
            await conteudo.CopyToAsync(copia).ConfigureAwait(false);
            bytes = copia.ToArray();
        }

        var imagem = new Imagem
        {
            NomeOriginal = nomeArquivoOriginal,
            NomeArquivo = nomeArquivo,
            CaminhoRelativo = caminhoRelativo,
            ContentType = contentType,
            TamanhoBytes = tamanhoBytes,
            Descricao = descricao
        };

        var salva = await Repositorio.UploadAsync(imagem, bytes, pastaRaiz).ConfigureAwait(false);
        var sucesso = salva != null;

        return new RespostaServico<Imagem>(salva)
        {
            BemSucedido = sucesso,
            Mensagem = sucesso ? "Upload realizado com sucesso." : "Falha ao realizar upload."
        };
    }

    public async Task<IRespostaServico<Imagem>> UploadAsync(IFormFile arquivo, string descricao = null)
    {
        if (arquivo == null || arquivo.Length == 0)
            return new RespostaServico<Imagem>(null, "Arquivo não informado.") { BemSucedido = false };
        using var stream = arquivo.OpenReadStream();
        return await UploadAsync(stream, arquivo.FileName, arquivo.ContentType, arquivo.Length, descricao).ConfigureAwait(false);
    }

    public async Task<IRespostaServico<int>> ExcluirComArquivoAsync(Guid imagemId)
    {
        var pastaRaiz = (await ObterCaminhoFisicoRaizAsync()).Dados;
        await Repositorio.ExcluirArquivoFisicoAsync(imagemId, pastaRaiz).ConfigureAwait(false);
        return await DeletarAsync(imagemId).ConfigureAwait(false);
    }

    public async Task<IRespostaServico<IList<Imagem>>> ObterPorIdsAsync(Guid[] ids)
    {
        var itens = await Repositorio.ObterPorIdsAsync(ids).ConfigureAwait(false);
        return new RespostaServico<IList<Imagem>>(itens)
        {
            BemSucedido = true,
            Mensagem = itens?.Count > 0 ? "Itens localizados." : "Nenhum item localizado."
        };
    }
}
