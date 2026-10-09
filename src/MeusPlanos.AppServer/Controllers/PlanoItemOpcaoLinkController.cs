using System;
using MeusPlanos.AppServer.Controllers.Base;
using MeusPlanos.Definicao.Entidade;
using MeusPlanos.Definicao.Interface.Repositorio;
using MeusPlanos.Definicao.Interface.Servico;
using Microsoft.AspNetCore.Http;

namespace MeusPlanos.AppServer.Controllers;

public class PlanoItemOpcaoLinkController(IPlanoItemOpcaoLinkServico servico, IHttpContextAccessor httpContextAccessor)
    : BaseApiController<PlanoItemOpcaoLink, Guid, IPlanoItemOpcaoLinkServico, IPlanoItemOpcaoLinkRepositorio>(servico, httpContextAccessor);
