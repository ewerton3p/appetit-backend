# Appetit — Backend API

API REST construída com ASP.NET Core 10 seguindo Clean Architecture.

## Tecnologias

- .NET 10
- Entity Framework Core 9 + Pomelo (MySQL)
- Scalar (documentação de API)

## Arquitetura

```
Appetit.Domain          → Entidades, modelos, exceções, utilitários
Appetit.Infrastructure  → DbContext, repositórios, migrations
Appetit.Application     → DTOs, interfaces, serviços
Appetit.API             → Controllers, extensions, Program.cs
```

O registro de serviços e repositórios é feito automaticamente por convenção: toda classe `Foo` que implementar `IFoo` é registrada como `Scoped` sem necessidade de configuração manual.

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MySQL 8+
- `dotnet-ef` instalado globalmente:

```bash
dotnet tool install --global dotnet-ef
```

## Configuração

### 1. appsettings.json

O arquivo `appsettings.json` contém a connection string com credenciais de exemplo. Ajuste para o seu ambiente antes de rodar:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=appetit;User=root;Password=SUA_SENHA;"
  }
}
```

> Para não expor credenciais no repositório, prefira usar `appsettings.Development.json` (já ignorado pelo `.gitignore` padrão do .NET) ou variáveis de ambiente:
>
> ```bash
> export ConnectionStrings__DefaultConnection="Server=...;Password=...;"
> ```

### 2. Banco de dados

Com a connection string configurada, aplique as migrations:

```bash
cd Appetit.Infrastructure
dotnet ef database update --startup-project ../Appetit
```

Ou pelo diretório raiz da solution:

```bash
dotnet ef database update --project Appetit.Infrastructure --startup-project Appetit
```

## Executando

```bash
cd Appetit
dotnet run
```

## Documentação da API

Disponível apenas em modo Development em:

```
https://localhost:{porta}/scalar/v1
```

## Endpoints disponíveis

| Método | Rota                        | Descrição                        |
|--------|-----------------------------|----------------------------------|
| GET    | /api/categories             | Lista categorias com paginação   |
| GET    | /api/categories/{id}        | Busca categoria por ID           |
| POST   | /api/categories/category    | Cria nova categoria              |
| PUT    | /api/categories/category/{id} | Atualiza categoria             |
| DELETE | /api/categories/category/{id} | Remove categoria               |

### Parâmetros de listagem

`GET /api/categories?page=1&search=texto`

| Parâmetro | Tipo   | Padrão | Descrição                   |
|-----------|--------|--------|-----------------------------|
| page      | int    | 1      | Número da página            |
| search    | string | ""     | Filtro por nome da categoria |

## Adicionando novos recursos

1. Crie a entidade em `Appetit.Domain/Models`
2. Adicione o `DbSet` no `ApplicationDbContext`
3. Crie a interface do repositório em `Appetit.Infrastructure/Data/Repositories/Interfaces/IFooRepository.cs`
4. Implemente o repositório em `Appetit.Infrastructure/Data/Repositories/FooRepository.cs` herdando de `RepositoryBase<T>`
5. Crie a interface do serviço em `Appetit.Application/Interfaces/IFooService.cs`
6. Implemente o serviço em `Appetit.Application/Services/FooService.cs`
7. Crie o controller em `Appetit.API/Controllers/FooController.cs`
8. Gere a migration: `dotnet ef migrations add NomeDaMigration --project Appetit.Infrastructure --startup-project Appetit`

> O registro no DI é automático — nenhuma alteração em `Program.cs` é necessária.
