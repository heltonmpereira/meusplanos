using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using MeusPlanos.Definicao.Interface.Servico.Resposta;
using MeusPlanos.Definicao.Modelo;
using MeusPlanos.Servico.Servico.Base;

namespace MeusPlanos.Servico.Servico;

public class PlanoItemOpcaoLinkImagemServico(
    IPlanoItemOpcaoLinkImagemRepositorio repositorio,
    IImagemServico imagemServico)
    : BaseServico<PlanoItemOpcaoLinkImagem, Guid, IPlanoItemOpcaoLinkImagemRepositorio>(repositorio),
        IPlanoItemOpcaoLinkImagemServico
{
    private readonly IImagemServico _imagemServico = imagemServico;

    public async Task<IRespostaServico<IList<PlanoItemOpcaoLinkImagem>>> ObterPorLinkIdAsync(Guid planoItemOpcaoLinkId)
    {
        var itens = await Repositorio.ObterPorLinkIdAsync(planoItemOpcaoLinkId).ConfigureAwait(false);
        return new RespostaServico<IList<PlanoItemOpcaoLinkImagem>>(itens)
        {
            BemSucedido = true,
            Mensagem = itens?.Count > 0 ? "Itens localizados." : "Nenhuma imagem vinculada."
        };
    }

    public async Task<IRespostaServico<PlanoItemOpcaoLinkImagem>> VincularImagemAsync(
        Guid planoItemOpcaoLinkId, Guid imagemId, int? ordem = null, bool ehCapa = false)
    {
        if (planoItemOpcaoLinkId == Guid.Empty)
            return new RespostaServico<PlanoItemOpcaoLinkImagem>(null, "Link não informado.") { BemSucedido = false };

        if (imagemId == Guid.Empty)
            return new RespostaServico<PlanoItemOpcaoLinkImagem>(null, "Imagem não informada.") { BemSucedido = false };

        var existentes = await Repositorio.ObterPorLinkIdAsync(planoItemOpcaoLinkId).ConfigureAwait(false);
        if (existentes.Any(a => a.ImagemId == imagemId))
            return new RespostaServico<PlanoItemOpcaoLinkImagem>(null, "Imagem já vinculada a este link.")
                { BemSucedido = false };

        var proximaOrdem = ordem ?? (existentes.Count + 1);
        var primeiro = existentes.Count == 0;

        var entidade = new PlanoItemOpcaoLinkImagem
        {
            PlanoItemOpcaoLinkId = planoItemOpcaoLinkId,
            ImagemId = imagemId,
            Ordem = proximaOrdem,
            EhCapa = ehCapa || primeiro
        };

        var resp = await IncluirAsync(entidade).ConfigureAwait(false);

        if (entidade.EhCapa && resp.BemSucedido)
            _ = await DefinirComoCapaAsync(resp.Dados.Id).ConfigureAwait(false);

        return resp;
    }

    public async Task<IRespostaServico<int>> DesvincularImagemAsync(Guid linkImagemId, bool excluirArquivo = false)
    {
        if (linkImagemId == Guid.Empty)
            return new RespostaServico<int>(0, "Registro não informado.") { BemSucedido = false };

        var atual = await Repositorio.ObterPorIdAsync(linkImagemId).ConfigureAwait(false);
        if (atual == null)
            return new RespostaServico<int>(0, "Registro não localizado.") { BemSucedido = false };

        var imagemId = atual.ImagemId;
        var resp = await DeletarAsync(linkImagemId).ConfigureAwait(false);

        if (excluirArquivo && resp.BemSucedido)
            _ = await _imagemServico.ExcluirComArquivoAsync(imagemId).ConfigureAwait(false);

        return resp;
    }

    public async Task<IRespostaServico<int>> DefinirComoCapaAsync(Guid linkImagemId)
    {
        var linhas = await Repositorio.DefinirComoCapaAsync(linkImagemId).ConfigureAwait(false);
        return new RespostaServico<int>(linhas)
        {
            BemSucedido = linhas > 0,
            Mensagem = linhas > 0 ? "Imagem definida como capa." : "Falha ao definir como capa."
        };
    }

    public async Task<IRespostaServico<int>> ReordenarAsync(Guid linkId, Guid[] idsOrdenados)
    {
        var linhas = await Repositorio.ReordenarAsync(linkId, idsOrdenados).ConfigureAwait(false);
        return new RespostaServico<int>(linhas)
        {
            BemSucedido = true,
            Mensagem = "Ordem atualizada."
        };
    }
}
