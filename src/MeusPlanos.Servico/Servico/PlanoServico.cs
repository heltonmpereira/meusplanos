using System;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using MeusPlanos.Servico.Servico.Base;

namespace MeusPlanos.Servico.Servico;

public class PlanoServico(IPlanoRepositorio repositorio) 
    : BaseServico<Plano, Guid, IPlanoRepositorio>(repositorio), IPlanoServico;
