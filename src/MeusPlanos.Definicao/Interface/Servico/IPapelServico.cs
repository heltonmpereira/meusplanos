using System;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico.Base;
using MeusPlanos.Definicao.Interface.Servico.Resposta;

namespace MeusPlanos.Definicao.Interface.Servico;

public interface IPapelServico : IServico<Papel, Guid, IPapelRepositorio>
{
    Task<IRespostaServico<Papel>> AtualizarUsuariosAsync(Guid id, Guid[] idsUsuario);
}