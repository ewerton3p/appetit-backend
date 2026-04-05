# Appetit Backend — Guia de Arquitetura

## Stack e Versões

- **.NET 10** (net10.0)
- **EF Core 9.0** com provider **Pomelo.EntityFrameworkCore.MySql 9.0.0** (MySQL)
- **JWT Bearer** para autenticação (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- **Scalar** para documentação da API (substitui Swagger)
- Cultura padrão: **pt-BR**

---

## Arquitetura — Clean Architecture

O projeto é dividido em 4 assemblies com dependências unidirecionais:

```
Appetit (API)
├── depende de: Appetit.Application, Appetit.Infrastructure
Appetit.Application
├── depende de: Appetit.Domain, Appetit.Infrastructure
Appetit.Infrastructure
├── depende de: Appetit.Domain
Appetit.Domain
└── sem dependências externas
```

### Responsabilidades por camada

| Projeto | Responsabilidade | Contém |
|---|---|---|
| `Appetit` (API) | Entrada HTTP | Controllers, Extensions, Middlewares, Program.cs |
| `Appetit.Application` | Regras de negócio | Services, Interfaces de Service, DTOs, Extensions de mapeamento |
| `Appetit.Domain` | Núcleo do domínio | Models (Entities), Exceptions, Responses, Utils, Constants |
| `Appetit.Infrastructure` | Acesso a dados / infra | DbContext, Repositories, TokenService, Migrations |

---

## Onde criar cada arquivo

### Models (Entidades)
**Local:** `Appetit.Domain/Models/NomeDoModel.cs`

- Sempre herdar de `Entity` (classe abstrata em `Appetit.Domain/Entity.cs`)
- Campos string usam tamanho padrão `[MaxLength(255)]`
- Datas sempre `DateTimeOffset` inicializadas com `DateUtils.GetCurrentUtcDateTime()`
- Após criar o model, adicionar `DbSet<NomeDoModel>` em `ApplicationDbContext`

```csharp
using Appetit.Domain.Common.Utils;
using System.ComponentModel.DataAnnotations;

namespace Appetit.Domain.Models
{
    public class Product : Entity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public User? CreatedBy { get; set; }
        public int? CreatedById { get; set; }

        public User? UpdatedBy { get; set; }
        public int? UpdatedById { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateUtils.GetCurrentUtcDateTime();
        public DateTimeOffset UpdatedAt { get; set; } = DateUtils.GetCurrentUtcDateTime();
    }
}
```

### Campos de auditoria de usuário

Todo Model deve incluir os campos de auditoria abaixo para rastrear quem criou e quem atualizou o registro:

```csharp
public User? CreatedBy { get; set; }
public int? CreatedById { get; set; }

public User? UpdatedBy { get; set; }
public int? UpdatedById { get; set; }
```

- São `nullable` pois o EF os trata como foreign keys opcionais.
- `CreatedById` é preenchido no Service no momento da criação via `_userService.GetLoggedUserId()`.
- `UpdatedById` é preenchido no Service no momento da atualização via `_userService.GetLoggedUserId()`.

### DTOs
**Local:** `Appetit.Application/DTOs/NomeDoRecurso/`

Criar 3 arquivos por recurso:
- `NomeDoRecursoCreateDTO.cs` — payload de criação/atualização
- `NomeDoRecursoViewDTO.cs` — dados retornados ao cliente
- `NomeDoRecursoExtension.cs` — métodos de mapeamento (extensões estáticas)

```csharp
// NomeDoRecursoExtension.cs
namespace Appetit.Application.DTOs.Product
{
    public static class ProductExtension
    {
        public static ProductViewDTO ToProductViewDTO(this Domain.Models.Product product) => new()
        {
            Id = product.Id,
            Name = product.Name,
            CreatedById = product.CreatedById,
            CreatedByName = product.CreatedBy?.Name ?? "",
            UpdatedById = product.UpdatedById,
            UpdatedByName = product.UpdatedBy?.Name ?? "",
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };

        // Criação: instancia uma nova entidade a partir do DTO
        public static Domain.Models.Product ToProduct(this ProductCreateDTO dto) => new()
        {
            Name = dto.Name
        };

        // Atualização: aplica os campos do DTO na entidade existente, preservando Id e rastreamento do EF
        public static Domain.Models.Product ToProduct(this ProductCreateDTO dto, Domain.Models.Product product)
        {
            product.Name = dto.Name;
            return product;
        }
    }
}
```

A sobrecarga que recebe a entidade existente é o padrão para **Update**: preserva o `Id` e o rastreamento do EF Core, evitando que o `DbContext` interprete o objeto como um novo insert. No service, usar assim:

```csharp
// Update — passa a entidade buscada do banco para a sobrecarga
updatedProduct.ToProduct(product);
product.UpdatedById = await _userService.GetLoggedUserId();
await _productRepository.Update(product);
```

### Interfaces de Service
**Local:** `Appetit.Application/Interfaces/INomeDoRecursoService.cs`

### Services
**Local:** `Appetit.Application/Services/NomeDoRecursoService.cs`

- Implementa `INomeDoRecursoService`
- O auto-registro funciona porque o nome segue o padrão `NomeDaClasse` : `INomeDaClasse`

### Interfaces de Repository
**Local:** `Appetit.Infrastructure/Data/Repositories/Interfaces/INomeDoRecursoRepository.cs`

### Repositories
**Local:** `Appetit.Infrastructure/Data/Repositories/NomeDoRecursoRepository.cs`

- Implementa `INomeDoRecursoRepository`
- Injeta `IRepositoryBase<TEntity>` e `ApplicationDbContext`
- Sempre atualizar `UpdatedAt` com `DateUtils.GetCurrentUtcDateTime()` no método `Update`
- O auto-registro funciona porque o nome segue o padrão `NomeDaClasse` : `INomeDaClasse`

#### `GetByIdAsync` com navegação de auditoria

Quando o endpoint de busca por ID precisa retornar os dados de `CreatedBy` e `UpdatedBy`, sobrescreva `GetByIdAsync` usando o `_dbContext` diretamente com `.Include()`:

```csharp
public async Task<Category?> GetByIdAsync(int id)
{
    return await _dbContext.Categories
        .Include(c => c.CreatedBy)
        .Include(c => c.UpdatedBy)
        .FirstOrDefaultAsync(c => c.Id == id);
}
```

Lembre de adicionar `using Microsoft.EntityFrameworkCore;` para ter acesso ao `Include` e `FirstOrDefaultAsync`.

### Controllers
**Local:** `Appetit/Controllers/NomeDoRecursoController.cs`

- Sempre adicionar `[Authorize]` na classe (exceto `AuthController`)
- Padrão de rota: `[Route("api/[controller]")]`

### Migrations
Geradas na pasta `Appetit.Infrastructure/Migrations/`

Comandos (executar na raiz do projeto):
```bash
dotnet ef migrations add NomeDaMigration --project Appetit.Infrastructure --startup-project Appetit
dotnet ef database update --project Appetit.Infrastructure --startup-project Appetit
```

---

## Dependency Injection — Auto-registration

### Services (`Appetit.Application`)
Arquivo: `Appetit/Extensions/DependencyInjectionExtension.cs`

Registra automaticamente **todos** os serviços do assembly `Appetit.Application` cujo nome da classe corresponde a `I{NomeDaClasse}`. Exemplo: `CategoryService` → `ICategoryService`.

**Regra:** o nome da interface **deve** ser `I` + nome exato da classe de implementação.

### Repositories (`Appetit.Infrastructure`)
Arquivo: `Appetit/Extensions/RepositoriesInjectionExtension.cs`

Registra automaticamente **todos** os repositories do assembly `Appetit.Infrastructure` pela mesma convenção de nomenclatura. O `IRepositoryBase<>` é registrado explicitamente como genérico.

**Não é necessário** registrar manualmente services ou repositories novos, desde que a convenção `NomeDaClasse : INomeDaClasse` seja respeitada.

---

## Datas — UTC obrigatório

**Sempre** usar o helper para obter a data/hora atual:

```csharp
using Appetit.Domain.Common.Utils;

DateUtils.GetCurrentUtcDateTime() // retorna DateTimeOffset.UtcNow
```

Tipo dos campos de data: `DateTimeOffset` (não `DateTime`).

---

## Padrão de Respostas

### `Response` — sem dados
**Usar quando:** operações que não retornam dados (Create, Update, Delete).

```csharp
// Appetit.Domain/Common/Responses/Response.cs
public class Response
{
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; } = true;
    public string? Details { get; set; }
}
```

### `ResponseData<T>` — com dados
**Usar quando:** operações que retornam dados (Get, Login).

```csharp
// Appetit.Domain/Common/Responses/ResponseData.cs
public class ResponseData<T>
{
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
    public Pagination? Pagination { get; set; }
    public string? Details { get; set; }
}
```

### Paginação
Usar `Pagination` quando o endpoint lista coleções:

```csharp
response.Pagination = new Pagination(page, totalCount);
// Pagination calcula TotalPages automaticamente com base em Paging.DefaultPageSize (50)
```

O `RepositoryBase` tem `GetWithPagination(page, filter, order, descending)` que retorna `PaginatedResult<TEntity>` com `Items` e `TotalCount`.

`PagingUtils.GetPageOffset(page)` calcula o offset de paginação.

---

## Padrão de Controllers

```csharp
[Route("api/[controller]")]
[ApiController]
[Authorize]  // SEMPRE em controllers protegidos
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // GET: api/products?page=1&search=
    [HttpGet]
    public async Task<ActionResult<ResponseData<List<ProductViewDTO>>>> GetProducts(int page = 1, string search = "")
        => Ok(await _productService.GetProducts(page, search));

    // GET: api/products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseData<ProductViewDTO>>> GetProductById(int id)
        => Ok(await _productService.GetProductById(id));

    // POST: api/products/product
    [HttpPost("Product")]
    public async Task<ActionResult<Response>> CreateProduct(ProductCreateDTO newProduct)
        => Created(nameof(CreateProduct), await _productService.CreateProduct(newProduct));

    // PUT: api/products/product/5
    [HttpPut("Product/{id}")]
    public async Task<ActionResult<Response>> UpdateProduct(ProductCreateDTO updatedProduct, int id)
        => Ok(await _productService.UpdateProduct(id, updatedProduct));

    // DELETE: api/products/product/5
    [HttpDelete("Product/{id}")]
    public async Task<ActionResult<Response>> DeleteProduct(int id)
        => Ok(await _productService.DeleteProduct(id));
}
```

### Verbos HTTP e retornos

| Operação | Verbo | Retorno HTTP | Tipo de Response |
|---|---|---|---|
| Listar (paginado) | `GET` | `200 Ok` | `ResponseData<List<XViewDTO>>` |
| Buscar por ID | `GET` | `200 Ok` | `ResponseData<XViewDTO>` |
| Criar | `POST` | `201 Created` | `Response` |
| Atualizar | `PUT` | `200 Ok` | `Response` |
| Deletar | `DELETE` | `200 Ok` | `Response` |

---

## Padrão de Services

```csharp
public class ProductService : IProductService
{
    private readonly IUserService _userService;
    private readonly IProductRepository _productRepository;

    public ProductService(IUserService userService, IProductRepository productRepository)
    {
        _userService = userService;
        _productRepository = productRepository;
    }

    public async Task<ResponseData<List<ProductViewDTO>>> GetProducts(int page, string search)
    {
        ResponseData<List<ProductViewDTO>> response = new();

        Expression<Func<Product, bool>> filter = p => string.IsNullOrEmpty(search) || p.Name.Contains(search);
        Expression<Func<Product, object>> order = p => p.Name;

        var result = await _productRepository.GetWithPagination(page, filter, order);
        response.Data = result.Items?.Select(p => p.ToProductViewDTO()).ToList();
        response.Pagination = new Pagination(page, result.TotalCount);

        if (response.Data == null || response.Data.Count == 0)
            response.Message = "Nenhum produto encontrado.";

        return response;
    }

    public async Task<ResponseData<ProductViewDTO>> GetProductById(int id)
    {
        ResponseData<ProductViewDTO> response = new();
        Product? product = await _productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Produto com o código: {id} não foi localizado.");
        response.Data = product.ToProductViewDTO();
        return response;
    }

    public async Task<Response> CreateProduct(ProductCreateDTO newProduct)
    {
        Response response = new();
        var product = newProduct.ToProduct();
        product.CreatedById = await _userService.GetLoggedUserId();
        await _productRepository.AddAsync(product);
        response.Message = "Produto criado com sucesso.";
        return response;
    }

    public async Task<Response> UpdateProduct(int id, ProductCreateDTO updatedProduct)
    {
        Response response = new();
        Product? product = await _productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Produto com o código: {id} não foi localizado.");
        product.Name = updatedProduct.Name;
        product.UpdatedById = await _userService.GetLoggedUserId();
        await _productRepository.Update(product);
        response.Message = "Produto atualizado com sucesso.";
        return response;
    }

    public async Task<Response> DeleteProduct(int id)
    {
        Response response = new();
        try
        {
            Product? product = await _productRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Produto não localizado.");
            await _productRepository.DeleteAsync(product);
            response.Message = "Produto excluído com sucesso.";
        }
        catch (DbUpdateException ex)
        {
            response.Success = false;
            response.Message = ex.InnerException?.Message.Contains("REFERENCE", StringComparison.OrdinalIgnoreCase) == true
                ? "Não é possível excluir este produto porque ele está associado a outros registros."
                : "Ocorreu um erro ao tentar excluir o produto.";
        }
        return response;
    }
}
```

---

## Exceções — Tratamento Global

As exceções são capturadas pelo `ExceptionMiddleware` (`Appetit/Middlewares/ExceptionMiddleware.cs`) e convertidas em respostas JSON com o status HTTP correto. **Não usar try/catch para estas exceções nos services** (exceto `DbUpdateException`).

| Exceção | Status HTTP | Uso |
|---|---|---|
| `NotFoundException` | `404 Not Found` | Registro não encontrado |
| `BadRequestException` | `400 Bad Request` | Dados inválidos / regra de negócio |
| `InternalServerErrorException` | `500 Internal Server Error` | Erros inesperados de infra |

Localização: `Appetit.Domain/Common/Exceptions/`

```csharp
throw new NotFoundException("Produto não localizado.");
throw new BadRequestException("E-mail já cadastrado.");
throw new InternalServerErrorException("Falha ao processar.");
```

O `Details` na resposta de erro expõe o stack trace apenas em **Development**.

---

## Usuário Logado — IUserService

`IUserService` expõe dois métodos para recuperar o contexto do usuário autenticado:

| Método | Retorno | Quando usar |
|---|---|---|
| `GetLoggedUserId()` | `Task<int>` | Apenas o ID, sem query ao banco (lê o claim `sub` do JWT) |
| `GetLoggedUser()` | `Task<User>` | Objeto `User` completo (faz query ao banco) |

**Prefira `GetLoggedUserId()`** quando só precisar da FK de auditoria — evita uma query desnecessária.

Injetar `IUserService` no Service que precisar:

```csharp
public class ProductService : IProductService
{
    private readonly IUserService _userService;
    private readonly IProductRepository _productRepository;

    public ProductService(IUserService userService, IProductRepository productRepository)
    {
        _userService = userService;
        _productRepository = productRepository;
    }

    public async Task<Response> CreateProduct(ProductCreateDTO newProduct)
    {
        Response response = new();
        var product = newProduct.ToProduct();
        product.CreatedById = await _userService.GetLoggedUserId();
        await _productRepository.AddAsync(product);
        response.Message = "Produto criado com sucesso.";
        return response;
    }

    public async Task<Response> UpdateProduct(int id, ProductCreateDTO updatedProduct)
    {
        Response response = new();
        var product = await _productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Produto com o código: {id} não foi localizado.");
        product.Name = updatedProduct.Name;
        product.UpdatedById = await _userService.GetLoggedUserId();
        await _productRepository.Update(product);
        response.Message = "Produto atualizado com sucesso.";
        return response;
    }
}
```

---

## Autenticação JWT

### Fluxo
1. `POST /api/auth/register` — cria usuário (senha hasheada com `IPasswordHasher<User>`)
2. `POST /api/auth/login` — valida credenciais e retorna `LoginResponseDTO` com `Token` e `User`
3. Demais endpoints requerem header `Authorization: Bearer {token}`

### Configuração
Registrada em `Appetit/Extensions/AuthServiceExtension.cs` via `AddJWTAuth(configuration)`.

### TokenService
`Appetit.Infrastructure/Services/TokenService.cs` implementa `ITokenService` e gera o JWT com claims: `sub` (id), `email`, `name`, `jti`.

**Nota:** `ITokenService` / `TokenService` estão em `Appetit.Infrastructure/Services/` — o auto-registro de repositories também captura esses serviços de infra pela convenção `NomeDaClasse : INomeDaClasse`.

---

## Extensions (Appetit/Extensions)

| Arquivo | Método | Função |
|---|---|---|
| `DependencyInjectionExtension.cs` | `AddDependencyInjections()` | Auto-registra services de `Appetit.Application` |
| `RepositoriesInjectionExtension.cs` | `AddRepositoriesInjections()` | Auto-registra repositories e services de `Appetit.Infrastructure` |
| `AuthServiceExtension.cs` | `AddJWTAuth(config)` | Configura JWT Bearer + `IPasswordHasher<User>` |

---

## Configuração — appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=appetit;..."
  },
  "Jwt": {
    "Key": "chave-secreta-minimo-32-caracteres",
    "Issuer": "Appetit",
    "Audience": "Appetit",
    "ExpirationHours": 24
  }
}
```

- A `DefaultConnection` usa **MySQL** via Pomelo. Migrations assembly: `Appetit.Infrastructure`.
- `Jwt:Key` deve ter **no mínimo 32 caracteres**.
- `Jwt:ExpirationHours` controla a validade do token.

---

## Repositório Base — Métodos disponíveis

`IRepositoryBase<TEntity>` / `RepositoryBase<TEntity>` em `Appetit.Infrastructure/Data/Repositories/`

| Método | Descrição |
|---|---|
| `GetByIdAsync(int id)` | Busca por PK |
| `Get(filter, order)` | Lista com filtro e ordenação opcionais |
| `GetWithPagination(page, filter, order, descending)` | Lista paginada |
| `GetWithPagination(query, page, filter, order, descending)` | Lista paginada com IQueryable customizado |
| `Count(filter)` | Conta registros |
| `AddAsync(entity)` | Insere e salva |
| `Update(entity)` | Atualiza e salva |
| `DeleteAsync(entity)` | Remove e salva |

Repositories específicos delegam para `IRepositoryBase<TEntity>` e podem usar `ApplicationDbContext` diretamente para queries customizadas.

---

## Checklist para novo CRUD

1. **Domain:** criar `Appetit.Domain/Models/NomeDoRecurso.cs` herdando `Entity`
2. **DbContext:** adicionar `DbSet<NomeDoRecurso>` em `ApplicationDbContext`
3. **Migration:** `dotnet ef migrations add AddNomeDoRecurso ...`
4. **DTOs:** criar em `Appetit.Application/DTOs/NomeDoRecurso/` os 3 arquivos (CreateDTO, ViewDTO, Extension)
5. **Interface de Service:** criar `Appetit.Application/Interfaces/INomeDoRecursoService.cs`
6. **Service:** criar `Appetit.Application/Services/NomeDoRecursoService.cs`
7. **Interface de Repository:** criar `Appetit.Infrastructure/Data/Repositories/Interfaces/INomeDoRecursoRepository.cs`
8. **Repository:** criar `Appetit.Infrastructure/Data/Repositories/NomeDoRecursoRepository.cs`
9. **Controller:** criar `Appetit/Controllers/NomeDoRecursoController.cs` com `[Authorize]`
