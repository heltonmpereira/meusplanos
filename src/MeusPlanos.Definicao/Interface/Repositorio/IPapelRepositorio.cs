using System;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio.Base;

namespace MeusPlanos.Definicao.Interface.Repositorio;

public interface IPapelRepositorio : IRepositorio<Papel, Guid>
{
    Task<Papel> AtualizarUsuariosAsync(Guid id, Guid[] idsUsuario);
}