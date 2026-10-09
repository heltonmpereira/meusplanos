# Módulo PlanoItem (Itens do Plano + Opções + Links) - Implementation Plan

## Repository Research

### Arquitetura Atual do Sistema
O sistema adota **Clean Architecture** em camadas, seguindo rigorosamente o padrão abaixo para cada entidade (ex: Plano, Usuario, Papel):

| Camada | Projeto | Responsabilidade |
|--------|---------|------------------|
| Domain | `MeusPlanos.Definicao` | Entidades, Interfaces (IRepositorio, IServico), Enums, Modelos |
| Infrastructure | `MeusPlanos.Modelo` | EF Core DbContext, Mapeamentos (Maps), Migrations, Implementações de Repositório |
| Application | `MeusPlanos.Servico` | Implementações de Serviços, herdando `BaseServico<>` |
| API | `MeusPlanos.AppServer` | Controllers da Web API, herdando `BaseApiController<>` |
| Client MVC | `MeusPlanos.AppCliente` | ViewModels, Controllers MVC, Refit (HTTP Client), Views Razor, EditorTemplates/DisplayTemplates |

### Padrões Identificados (baseado no módulo Plano)
- **Entidade**: Implementa `IEntidade<Guid>`, possui `DataCriacao`, `DataAlteracao`, `DataDelecao` (soft delete)
- **Repositório**: Interface `IPlanoRepositorio : IRepositorio<Plano, Guid>` + implementação `PlanoRepositorio : BaseRepositorio<>`
- **Serviço**: Interface `IPlanoServico : IServico<Plano, Guid, IPlanoRepositorio>` + implementação `PlanoServico : BaseServico<>`
- **API Controller**: `PlanoController : BaseApiController<Plano, Guid, IPlanoServico, IPlanoRepositorio>`
- **Client Refit**: `IPlanoRefit` com endpoints Navegar, ObterPorId, Alterar, Incluir, Deletar
- **Client ViewModel**: `PlanoViewModel : BaseViewModel, IEntidade<Guid>`
- **Client Controller**: `PlanoController : BaseController<PlanoViewModel, Guid, IPlanoRefit>`
- **DI**: Registrado via `ServicoHelper.AdicionarDados()` (API) e `RefitHelper.AdicionarConexoesRefit()` (Client)
- **Soft Delete**: Implementado no `BaseRepositorio.DeletarRangeAsync` — seta `DataDelecao` em vez de remover fisicamente

### Requisitos do Módulo PlanoItem
O usuário descreveu 3 níveis de composição:

1. **PlanoItem** → Itens que compõem um Plano
   - Exemplo 1 (Viagem/Férias): Hospedagem, Alimentação, Transporte → cada um com **orçamento estimado**
   - Exemplo 2 (Trocar carro): SUV, Sedã, Hatch → categorias/tipos de item

2. **PlanoItemOpcao** → Opções concorrentes dentro de cada PlanoItem
   - Ex: SUV → HR-V, CR-V, IX35 | Sedã → Nissan Sentra, Versa, City Sedan | Hatch → City, Fit, Argo

3. **PlanoItemOpcaoLink** → Múltiplas ofertas/URLs para cada Opção
   - Cada link contém: URL do site, valor da oferta, condições de pagamento, características, observações, data da cotação

---

## Files and Modules

### 1. Domain Layer — `MeusPlanos.Definicao`
| Arquivo | Ação |
|---------|------|
| `Entidade/PlanoItem.cs` | **Criar** — Entidade PlanoItem com PlanoId FK, Nome, Descricao, ValorOrcamentoEstimado, Ordem |
| `Entidade/PlanoItemOpcao.cs` | **Criar** — Entidade PlanoItemOpcao com PlanoItemId FK, Nome, Descricao, Caracteristicas, Ordem |
| `Entidade/PlanoItemOpcaoLink.cs` | **Criar** — Entidade PlanoItemOpcaoLink com PlanoItemOpcaoId FK, Url, NomeSite, Valor, CondicoesPagamento, Observacoes, DataCotacao |
| `Interface/Repositorio/IPlanoItemRepositorio.cs` | **Criar** |
| `Interface/Repositorio/IPlanoItemOpcaoRepositorio.cs` | **Criar** |
| `Interface/Repositorio/IPlanoItemOpcaoLinkRepositorio.cs` | **Criar** |
| `Interface/Servico/IPlanoItemServico.cs` | **Criar** |
| `Interface/Servico/IPlanoItemOpcaoServico.cs` | **Criar** |
| `Interface/Servico/IPlanoItemOpcaoLinkServico.cs` | **Criar** |
| `Entidade/Plano.cs` | **Editar** — Adicionar `ICollection<PlanoItem> Itens` (navigation property) |

### 2. Infrastructure Layer — `MeusPlanos.Modelo`
| Arquivo | Ação |
|---------|------|
| `Maps/PlanoItemMap.cs` | **Criar** — Herdar `BaseMap<PlanoItem, Guid>`, configurar FK e campos |
| `Maps/PlanoItemOpcaoMap.cs` | **Criar** |
| `Maps/PlanoItemOpcaoLinkMap.cs` | **Criar** |
| `Data/MeusPlanosContext.cs` | **Editar** — Adicionar `DbSet<PlanoItem>`, `DbSet<PlanoItemOpcao>`, `DbSet<PlanoItemOpcaoLink>` |
| `Repositorio/PlanoItemRepositorio.cs` | **Criar** |
| `Repositorio/PlanoItemOpcaoRepositorio.cs` | **Criar** |
| `Repositorio/PlanoItemOpcaoLinkRepositorio.cs` | **Criar** |
| `Helper/ServicoHelper.cs` (AppServer) | **Editar** — Registrar 3 repositórios + 3 serviços no DI |
| `Migrations/*_PlanoItemMigration.cs` | **Criar** — Via `add-migration` após models prontos |

### 3. Application Layer — `MeusPlanos.Servico`
| Arquivo | Ação |
|---------|------|
| `Servico/PlanoItemServico.cs` | **Criar** |
| `Servico/PlanoItemOpcaoServico.cs` | **Criar** |
| `Servico/PlanoItemOpcaoLinkServico.cs` | **Criar** |

### 4. API Layer — `MeusPlanos.AppServer`
| Arquivo | Ação |
|---------|------|
| `Controllers/PlanoItemController.cs` | **Criar** |
| `Controllers/PlanoItemOpcaoController.cs` | **Criar** |
| `Controllers/PlanoItemOpcaoLinkController.cs` | **Criar** |

### 5. Client Layer — `MeusPlanos.AppCliente`
| Arquivo | Ação |
|---------|------|
| `ViewModel/PlanoItemViewModel.cs` | **Criar** |
| `ViewModel/PlanoItemOpcaoViewModel.cs` | **Criar** |
| `ViewModel/PlanoItemOpcaoLinkViewModel.cs` | **Criar** |
| `ViewModel/PlanoViewModel.cs` | **Editar** — Adicionar `ICollection<PlanoItemViewModel> Itens` |
| `Refit/IPlanoItemRefit.cs` | **Criar** |
| `Refit/IPlanoItemOpcaoRefit.cs` | **Criar** |
| `Refit/IPlanoItemOpcaoLinkRefit.cs` | **Criar** |
| `Helper/RefitHelper.cs` (método `AdicionarConexoesRefit`) | **Editar** — Registrar as 3 interfaces Refit |
| `Controllers/PlanoItemController.cs` | **Criar** |
| `Controllers/PlanoItemOpcaoController.cs` | **Criar** |
| `Controllers/PlanoItemOpcaoLinkController.cs` | **Criar** |
| `Views/PlanoItem/Index.cshtml`, `Incluir.cshtml`, `Alterar.cshtml`, `Detalhes.cshtml`, `Deletar.cshtml` | **Criar** (similar a Views/Plano/) |
| `Views/PlanoItemOpcao/` (5 views) | **Criar** |
| `Views/PlanoItemOpcaoLink/` (5 views) | **Criar** |
| `Views/Shared/EditorTemplates/PlanoItemViewModel.cshtml` | **Criar** |
| `Views/Shared/EditorTemplates/PlanoItemOpcaoViewModel.cshtml` | **Criar** |
| `Views/Shared/EditorTemplates/PlanoItemOpcaoLinkViewModel.cshtml` | **Criar** |
| `Views/Shared/DisplayTemplates/PlanoItemViewModel.cshtml` | **Criar** |
| `Views/Shared/DisplayTemplates/PlanoItemOpcaoViewModel.cshtml` | **Criar** |
| `Views/Shared/DisplayTemplates/PlanoItemOpcaoLinkViewModel.cshtml` | **Criar** |
| `Views/Plano/Detalhes.cshtml` | **Editar** — Incluir seção listando os PlanoItems do plano, com botão "Gerenciar Itens" |

---

## Implementation Steps (ordem de dependência)

### Etapa 1 — Domain (Definicao)
1. Criar as 3 entidades (`PlanoItem`, `PlanoItemOpcao`, `PlanoItemOpcaoLink`) implementando `IEntidade<Guid>`
2. Adicionar navigation property `Itens` em `Plano.cs`
3. Criar as 3 interfaces de repositório
4. Criar as 3 interfaces de serviço

### Etapa 2 — Infrastructure (Modelo)
5. Criar os 3 Maps com configuração das FKs (DeleteBehavior.Restrict)
6. Adicionar os 3 DbSets em `MeusPlanosContext`
7. Criar os 3 Repositories
8. Sobrescrever `AdicionarIncludes` nos repositórios para carregar a árvore (Plano → Itens → Opções → Links)

### Etapa 3 — Application (Servico)
9. Criar os 3 Services herdando `BaseServico<>`
10. Registrar DI no `ServicoHelper.AdicionarDados()`

### Etapa 4 — API (AppServer)
11. Criar os 3 Controllers herdando `BaseApiController<>`
12. Criar migration (`add-migration PlanoItemMigration -Project MeusPlanos.Modelo -StartupProject MeusPlanos.AppServer`)
13. Aplicar migration (`update-database`)

### Etapa 5 — Client MVC (AppCliente)
14. Criar os 3 ViewModels
15. Criar as 3 interfaces Refit
16. Registrar Refit clients no `RefitHelper.AdicionarConexoesRefit()`
17. Criar os 3 Controllers MVC
18. Criar EditorTemplates e DisplayTemplates
19. Criar as Views (5 para cada entidade = 15 views)
20. Integrar: adicionar na view Detalhes do Plano um link/grid para gerenciar os Itens do Plano (passando `PlanoId` como filtro)

### Etapa 6 — Validação
21. Compilar a solução (`dotnet build`)
22. Verificar erros de diagnóstico
23. Verificar se as APIs estão funcionando via Swagger
24. Fazer smoke test: criar Plano → criar PlanoItem → criar Opção → criar Links

---

## Dependencies and Considerations

### Includes Encadeados (Fundamental)
Os repositórios precisam sobrescrever `AdicionarIncludes` para permitir:
- **PlanoItemRepositorio**: Include `Opcoes` e nested `Opcoes.Links`
- **PlanoItemOpcaoRepositorio**: Include `Links` e `PlanoItem`
- **PlanoRepositorio** (editar): Include `Itens.Opcoes.Links`

Isso é essencial para a tela de Detalhes do Plano carregar toda a árvore.

### Roteamento e Filtro por Pai
- `PlanoItem` sempre pertence a um `Plano` → telas de Index devem filtrar por `PlanoId`
- `PlanoItemOpcao` sempre pertence a um `PlanoItem` → filtrar por `PlanoItemId`
- `PlanoItemOpcaoLink` sempre pertence a uma `PlanoItemOpcao` → filtrar por `PlanoItemOpcaoId`
- As telas de Incluir já devem receber o Id do pai pré-preenchido (via query string / rota)

### Validações de Negócio
- `PlanoItem.ValorOrcamentoEstimado` deve ser ≥ 0
- `PlanoItemOpcaoLink.Url` deve ser URL válida
- `PlanoItemOpcaoLink.Valor` deve ser ≥ 0 (quando informado)
- Campo `Ordem` em cada entidade para ordenação customizada

### Enum de Status (opcional por enquanto)
Por ora, reutilizar padrão: não criar Status específico para Item/Opção. Se necessário, pode ser adicionado em outra fase.

### Soft Delete em Cascata
Quando um Plano for deletado (soft), os Itens/Opções/Links associados permanecem. O `BaseRepositorio` não deleta em cascata automaticamente — comportamento aceitável.

### CSP e Segurança
- URLs de links externos exibidas nas Views devem ter `rel="noopener noreferrer"` e abrir em nova aba (`target="_blank"`)
- Respeitar as políticas de CSP já configuradas no `AppCliente/Startup.cs`

---

## Validation

1. **Build**: `dotnet build c:\dev\github\meusplanos\src\MeusPlanos.slnx` — 0 erros, 0 warnings críticos
2. **Diagnostics**: Executar `GetDiagnostics` no final para verificar erros de análise
3. **Migration**: Executar `script-migration` para validar SQL gerado antes de `update-database`
4. **Swagger Smoke Test**:
   - POST /api/PlanoItem → criar item vinculado a um Plano existente
   - POST /api/PlanoItemOpcao → criar opção vinculada ao Item
   - POST /api/PlanoItemOpcaoLink → criar link com URL e valor
   - GET /api/PlanoItem/{id} com includes → verificar se a árvore retorna completa
5. **UI Smoke Test**:
   - Acessar Detalhes de um Plano existente
   - Clicar em "Gerenciar Itens" → listar Itens do Plano
   - Incluir Item → preencher orçamento
   - Abrir Item → Incluir Opções
   - Abrir Opção → Incluir Links com valores

---

## Risks

| Risco | Probabilidade | Impacto | Mitigação |
|-------|--------------|---------|-----------|
| **Includes encadeados não carregarem filhos netos** | Média | Alto | Testar `ObterPorIdAsync` de PlanoItem com includes aninhados (`ThenInclude`) e validar retorno JSON no Swagger |
| **Performance de includes multiplos** | Média | Médio | Considerar Split Queries do EF Core (`AsSplitQuery()`) se houver N+1 |
| **CSP bloquear links externos nas Views** | Alta | Baixo | Adicionar `rel="noopener"` e `target="_blank"`; NÃO usar iframe, apenas `<a href>` |
| **Migration gerar FK com DeleteBehavior.Cascade** | Média | Alto | Forçar `DeleteBehavior.Restrict` explicitamente nos Maps |
| **Soft Delete conflitar com filtro por pai** | Baixo | Médio | Garantir que `NavegarAsync` exclua registros com `DataDelecao != null` por padrão (já implementado no BaseRepositorio? Verificar — se não, adicionar query filter no Map) |
| **ViewModel com referências circulares** | Média | Médio | `ReferenceLoopHandling.Ignore` já está configurado em ambos os startups. Evitar referências bidirecionais excessivas nos ViewModels |

