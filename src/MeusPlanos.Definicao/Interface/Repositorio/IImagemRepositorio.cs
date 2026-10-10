using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio.Base;

namespace MeusPlanos.Definicao.Interface.Repositorio;

public interface IImagemRepositorio : IRepositorio<Imagem, Guid>
{
    Task<Imagem> UploadAsync(Imagem imagem, byte[] conteudo, string pastaRaiz);
    Task<bool> ExcluirArquivoFisicoAsync(Guid imagemId, string pastaRaiz);
    Task<IList<Imagem>> ObterPorIdsAsync(Guid[] ids);
}
