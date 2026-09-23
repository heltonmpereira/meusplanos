using System;
using MeusPlanos.AppServer.Controllers.Base;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using Microsoft.AspNetCore.Http;

namespace MeusPlanos.AppServer.Controllers;

public class PlanoController(IPlanoServico servico, IHttpContextAccessor httpContextAccessor) 
    : BaseApiController<Plano, Guid, IPlanoServico, IPlanoRepositorio>(servico, httpContextAccessor)
{

}
