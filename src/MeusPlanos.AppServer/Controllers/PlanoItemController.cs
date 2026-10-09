using System;
using MeusPlanos.AppServer.Controllers.Base;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using Microsoft.AspNetCore.Http;

namespace MeusPlanos.AppServer.Controllers;

public class PlanoItemController(IPlanoItemServico servico, IHttpContextAccessor httpContextAccessor)
    : BaseApiController<PlanoItem, Guid, IPlanoItemServico, IPlanoItemRepositorio>(servico, httpContextAccessor);
