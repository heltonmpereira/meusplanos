using System;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Modelo.Data;
using MeusPlanos.Modelo.Repositorio.Base;

namespace MeusPlanos.Modelo.Repositorio;

public class PlanoRepositorio(IDbContext contexto) 
    : BaseRepositorio<Plano, Guid>(contexto), IPlanoRepositorio;
