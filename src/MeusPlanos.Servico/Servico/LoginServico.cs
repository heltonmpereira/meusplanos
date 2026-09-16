using System;
using System.Linq;
using System.Threading.Tasks;
using Dhani.Utilitarios.Filtro;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using MeusPlanos.Definicao.Interface.Servico.Resposta;
using MeusPlanos.Definicao.Modelo;
using MeusPlanos.Definicao.Modelo.Usuario;
using MeusPlanos.Servico.Helper;
using MeusPlanos.Servico.Servico.Base;

namespace MeusPlanos.Servico.Servico
{
    public class LoginServico(IUsuarioRepositorio repositorio) : BaseServico<Usuario, Guid, IUsuarioRepositorio>(repositorio), ILoginServico
    {
        private static FiltroOpcao FiltroUsuario(string username) =>
            new("username", TipoOperadorBusca.Igual, username);
        private static FiltroOpcao FiltroEmail(string email) =>
            new("email", TipoOperadorBusca.Igual, email);

        private async Task<Usuario> ObterPorUsuarioAsync(string username)
        {
            var criterio = new FiltroBase();
            criterio.AdicionarFiltro(null, FiltroUsuario(username));

            return await BuscarUsuarioAsync(criterio);
        }
        private async Task<Usuario> ObterPorUsernameEEmailAsync(string username, string email)
        {
            var criterio = new FiltroBase();
            criterio.AdicionarFiltro(null, FiltroUsuario(username));
            criterio.AdicionarFiltro(null, FiltroEmail(email));

            return await BuscarUsuarioAsync(criterio);
        }
        private async Task<Usuario> ObterPorUsernameOuEmailAsync(string username, string email)
        {
            var criterio = new FiltroBase();
            criterio.AdicionarFiltro(null, FiltroUsuario(username));
            var filtroEmail = FiltroEmail(email);
            filtroEmail.RelacaoOutrosFiltros = TipoOperadorLogico.Or;
            criterio.AdicionarFiltro(null, filtroEmail);

            return await BuscarUsuarioAsync(criterio);
        }
        private async Task<Usuario> BuscarUsuarioAsync(ICriterio criterio)
        {
            var retorno = await Repositorio.NavegarAsync(criterio).ConfigureAwait(false);
            var usuario = retorno.Item1.SingleOrDefault();

            return usuario;
        }

        public async Task<IRespostaServico<UsuarioToken>> LoginAsync(UsuarioLogin userInfo)
        {
            var usuario = await ObterPorUsuarioAsync(userInfo.Username).ConfigureAwait(false);
            if (usuario == null)
            {
                return new RespostaServico<UsuarioToken>(null, "Usuário não localizado")
                {
                    BemSucedido = false,
                    StatusCode = System.Net.HttpStatusCode.NotAcceptable
                };
            }

            if (!usuario.PasswordHash.VerificarHashSenha(userInfo.Password))
            {
                return new RespostaServico<UsuarioToken>(null, "Usuário/Senha inválidos.")
                {
                    BemSucedido = false,
                    StatusCode = System.Net.HttpStatusCode.NotAcceptable
                };
            }

            var token = new UsuarioToken(usuario)
            {
                Roles = usuario.Papeis.Select(s => s.Papel.Nome).ToList(),
            };
            token.Roles.Add("UsuarioLogado");
            token.DataExpericaoToken = DateTime.Now.AddMonths(1);
            token.TokenAcesso = TokenServico.GerarToken(token);

            return new RespostaServico<UsuarioToken>(
                token,
                "Usuário localizado com sucesso.");
        }

        public async Task<IRespostaServico<UsuarioToken>> AcessarComoAsync(Guid id)
        {
            var userDb = await ObterPorIdAsync(id).ConfigureAwait(false);
            if (userDb == null || userDb.Dados == null)
            {
                return new RespostaServico<UsuarioToken>(null, "Usuário não localizado")
                {
                    BemSucedido = false,
                    StatusCode = System.Net.HttpStatusCode.NotAcceptable
                };
            }

            var usuario = userDb.Dados;
            var token = new UsuarioToken(usuario)
            {
                Roles = usuario.Papeis.Select(s => s.Papel.Nome).ToList(),
            };
            token.Roles.Add("UsuarioLogadoComo");
            token.DataExpericaoToken = DateTime.Now.AddMonths(1);
            token.TokenAcesso = TokenServico.GerarToken(token);

            return new RespostaServico<UsuarioToken>(
                token,
                "Usuário localizado com sucesso.");
        }

        public async Task<IRespostaServico<UsuarioRedefinirSenha>> RedefinirSenhaAsync(UsuarioSolicitacaoRedefinicaoSenha model)
        {
            var usuario = await ObterPorUsernameEEmailAsync(model.Username, model.Email).ConfigureAwait(false);
            if (usuario == null)
            {
                return new RespostaServico<UsuarioRedefinirSenha>(null, "Usuário não localizado")
                {
                    BemSucedido = false,
                    StatusCode = System.Net.HttpStatusCode.NotAcceptable
                };
            }

            var item = await Repositorio.RedefinirSenhaAsync(usuario).ConfigureAwait(false);
            var retorno = new RespostaServico<UsuarioRedefinirSenha>(
                item,
                "Solicitação de redefinição de senha realizada com sucesso.");

            return retorno;
        }

        public async Task<IRespostaServico<bool>> AlterarSenhaComCodigoRedefinicaoAsync(UsuarioRedefinirSenha model)
        {
            var usuario = await Repositorio.ObterPorIdAsync(model.Id);
            if (usuario == null)
            {
                return new RespostaServico<bool>(false, "Usuário não localizado")
                {
                    BemSucedido = false,
                    StatusCode = System.Net.HttpStatusCode.NotAcceptable
                };
            }

            usuario.PasswordHash = model.Password;
            usuario.CodigoRedefinicaoSenha = null;
            _ = await Repositorio.AlterarSenhaComCodigoRedefinicaoAsync(usuario).ConfigureAwait(false);
            var retorno = new RespostaServico<bool>(
                true,
                "Solicitação de redefinição de senha realizada com sucesso.");

            return retorno;
        }

        public async Task<IRespostaServico<bool>> AlterarSenhaAsync(AlterarSenha model)
        {
            var usuario = await Repositorio.ObterPorIdAsync(model.Id);
            if (usuario == null)
            {
                return new RespostaServico<bool>(false, "Usuário não localizado")
                {
                    BemSucedido = false,
                    StatusCode = System.Net.HttpStatusCode.NotAcceptable
                };
            }

            if (!usuario.PasswordHash.VerificarHashSenha(model.SenhaAtual))
            {
                return new RespostaServico<bool>(false, "Usuário/Senha inválidos.")
                {
                    BemSucedido = false,
                    StatusCode = System.Net.HttpStatusCode.NotAcceptable
                };
            }

            usuario.CodigoRedefinicaoSenha = null;
            usuario.PasswordHash = model.NovaSenha.CriptografarTexto();
            usuario.CodigoRedefinicaoSenha = null;
            _ = await Repositorio.AlterarSenhaAsync(usuario).ConfigureAwait(false);
            var retorno = new RespostaServico<bool>(
                true,
                "Alteração de senha realizada com sucesso.");

            return retorno;
        }

        public override async Task<IRespostaServico<Usuario>> IncluirAsync(Usuario item)
        {
            var cadastrado = await ObterPorUsernameOuEmailAsync(item.Username, item.Email);
            if (cadastrado != null)
            {
                return new RespostaServico<Usuario>(null, "Este nome de usuário ou endereço de e-mail já está sendo utilizado.")
                {
                    BemSucedido = false,
                    StatusCode = System.Net.HttpStatusCode.Unauthorized
                };
            }

            var retorno = await base.IncluirAsync(item);
            retorno.Mensagem = !retorno.BemSucedido
                ? retorno.Mensagem
                : "Usuário registrado com sucesso!";

            return retorno;
        }
    }
}