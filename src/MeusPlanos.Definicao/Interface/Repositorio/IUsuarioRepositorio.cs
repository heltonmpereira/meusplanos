using System;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio.Base;
using MeusPlanos.Definicao.Modelo.Usuario;

namespace MeusPlanos.Definicao.Interface.Repositorio
{
    public interface IUsuarioRepositorio : IRepositorio<Usuario, Guid>
    {
        Task<UsuarioRedefinirSenha> RedefinirSenhaAsync(Usuario model);
        Task<bool> AlterarSenhaComCodigoRedefinicaoAsync(Usuario model);
        Task<bool> AlterarSenhaAsync(Usuario model);
    }
}