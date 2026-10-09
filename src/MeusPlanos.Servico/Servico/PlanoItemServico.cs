using System;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using MeusPlanos.Servico.Servico.Base;

namespace MeusPlanos.Servico.Servico;

public class PlanoItemServico(IPlanoItemRepositorio repositorio)
    : BaseServico<PlanoItem, Guid, IPlanoItemRepositorio>(repositorio), IPlanoItemServico;
