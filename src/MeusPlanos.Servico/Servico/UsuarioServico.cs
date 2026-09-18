using System;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using MeusPlanos.Servico.Servico.Base;

namespace MeusPlanos.Servico.Servico;

public class UsuarioServico(IUsuarioRepositorio repositorio) : BaseServico<Usuario, Guid, IUsuarioRepositorio>(repositorio), IUsuarioServico
{
}