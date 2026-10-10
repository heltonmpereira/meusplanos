using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico.Base;
using MeusPlanos.Definicao.Interface.Servico.Resposta;

namespace MeusPlanos.Definicao.Interface.Servico;

public interface IImagemServico : IServico<Imagem, Guid, IImagemRepositorio>
{
    Task<IRespostaServico<Imagem>> UploadAsync(
        Stream conteudo,
        string nomeArquivoOriginal,
        string contentType,
        long tamanhoBytes,
        string descricao = null);

    Task<IRespostaServico<int>> ExcluirComArquivoAsync(Guid imagemId);
    Task<IRespostaServico<string>> ObterCaminhoFisicoRaizAsync();
    Task<IRespostaServico<IList<Imagem>>> ObterPorIdsAsync(Guid[] ids);
}
