using System;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using MeusPlanos.Definicao.Interface.Servico.Resposta;
using MeusPlanos.Definicao.Modelo;
using MeusPlanos.Servico.Servico.Base;

namespace MeusPlanos.Servico.Servico;

public class PapelServico(IPapelRepositorio repositorio) : BaseServico<Papel, Guid, IPapelRepositorio>(repositorio), IPapelServico
{
    public async Task<IRespostaServico<Papel>> AtualizarUsuariosAsync(Guid id, Guid[] idsUsuario)
    {
        var retorno = await Repositorio.AtualizarUsuariosAsync(id, idsUsuario);

        return await Task.FromResult<IRespostaServico<Papel>>(new RespostaServico<Papel>(retorno));
    }
}