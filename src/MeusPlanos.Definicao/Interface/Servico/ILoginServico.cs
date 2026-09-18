using System;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico.Base;
using MeusPlanos.Definicao.Interface.Servico.Resposta;
using MeusPlanos.Definicao.Modelo.Usuario;

namespace MeusPlanos.Definicao.Interface.Servico;

public interface ILoginServico : IServico<Usuario, Guid, IUsuarioRepositorio>
{
    Task<IRespostaServico<UsuarioToken>> LoginAsync(UsuarioLogin userInfo);
    Task<IRespostaServico<UsuarioRedefinirSenha>> RedefinirSenhaAsync(UsuarioSolicitacaoRedefinicaoSenha model);
    Task<IRespostaServico<bool>> AlterarSenhaComCodigoRedefinicaoAsync(UsuarioRedefinirSenha model);
    Task<IRespostaServico<bool>> AlterarSenhaAsync(AlterarSenha model);
    Task<IRespostaServico<UsuarioToken>> AcessarComoAsync(Guid id);
}