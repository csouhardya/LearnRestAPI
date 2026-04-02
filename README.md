# LearnRestAPI - Code Flow

This repository contains a simple ASP.NET Core Web API (`WebAPI` project) and an application core library (`ApplicationCore` project). The API exposes product listing endpoints and uses MediatR to dispatch queries handled in the application core.

This README explains the request → response flow, where to find key code, and how to run the project locally.

## Projects

- `WebAPI` - ASP.NET Core Web API project. Hosts controllers, DI registration and swagger settings.
- `ApplicationCore` - Business logic, MediatR requests/handlers, models and repository/data-access logic.

## High level request flow

1. Client sends HTTP request to an endpoint defined in `WebAPI/Controllers/ProductsController.cs` (example: `GET /api/products` or `GET /api/products/SearchTerm`).
2. The controller action calls the application service: `IProductService` (implemented by `ProductService`).
3. `ProductService` uses MediatR (`ISender`) to `Send(...)` a query object (for example `GetProductsQuery`, `GetProductsQueryBySearchTerm` or `GetProductQuery`).
4. MediatR locates and invokes the matching request handler implemented in `ApplicationCore` (see `GetQueryHandlers`).
5. The handler uses `IProductsRepository` to access the data store. The repository currently uses ADO.NET/Dapper to query the database.
6. The handler applies filtering, sorting and pagination and returns a result (`List<Product>` or `PageList<Product>`).
7. The `ProductService` returns the result to the controller.
8. The controller wraps the result in an `IActionResult` (usually `Ok(...)`) and the response is sent to the client.

Sequence (short): Client → Controller → Service → MediatR.Send(Query) → Handler → Repository → Database → Handler returns → Service → Controller → Client

## Key files and responsibilities

- `WebAPI/Program.cs` - application startup, dependency injection, MediatR registration and Swagger configuration. Look here to see how `IProductService`, `IProductsRepository`, `IConnectionProvider` and MediatR handlers are registered.
- `WebAPI/Controllers/ProductsController.cs` - exposes REST endpoints and maps request parameters to service calls.
- `ApplicationCore/Interfaces/IProductService.cs` - service contract used by controllers.
- `ApplicationCore/Services/ProductService.cs` - concrete implementation that translates controller calls into MediatR queries.
- `ApplicationCore/Products/Get/GetProductsQuery.cs` - MediatR request/record definitions for product queries.
- `ApplicationCore/Products/Get/GetQueryHandlers.cs` - MediatR handlers that orchestrate repository calls and perform filtering, sorting and pagination.
- `ApplicationCore/Products/Get/PaginationHandler.cs` - helper that materializes paging into a `PageList<T>` envelope.
- `ApplicationCore/Models/Product.cs` - product model used across layers.
- `ApplicationCore/Repositories/ProductsRepository.cs` - repository that uses ADO.NET and Dapper to read/write product data.
- `Databases/DDL/CREATE_products.sql` - DDL showing the expected schema and column names used by the repository.

## Endpoints

- `GET /api/products` - returns full product list (via `GetProductsQuery`).
- `GET /api/products/SearchTerm?searchTerm=&sortBy=&sortOrder=&page=&pageSize=` - returns filtered/paged results (via `GetProductsQueryBySearchTerm`).
- `GET /api/products/Guid?guid=<value>` - returns a single product by GUID (via `GetProductQuery`).

## How to run
0. Change the connection_String environment variable with your value
1. From repository root run (requires .NET 10 SDK):

   ```bash
   dotnet build
   dotnet run --project WebAPI
   ```

2. The `WebAPI` project is configured to open Swagger by default (see `WebAPI/Properties/launchSettings.json` with `launchUrl: "swagger"`), or visit:

   - `https://localhost:<port>/swagger`

## Development notes

- DI and MediatR: Handlers are discovered by the `builder.Services.AddMediatR(...)` call in `Program.cs` which registers handlers from the assembly containing the handler types.
- `ProductService` injects `ISender` (MediatR) — the service itself is registered in DI so controllers can accept `IProductService` in their constructor.
- Repository: `IProductsRepository` is implemented by `ProductsRepository` and uses an injected `IConnectionProvider` to obtain `SqlConnection` instances. There are both ADO.NET and Dapper examples in the repository.
- Sorting uses expression trees in `GetQueryHandlers.SortHelper` to build a `Queryable.OrderBy` call based on a property name. Pagination is handled by `PaginationHandler.Paginate` which returns a `PageList<T>`.
- The database schema is defined in `Databases/DDL/CREATE_products.sql` — ensure the table and stored procedures exist when running the API.

## Troubleshooting

- If you get DI errors about missing services, confirm registrations in `WebAPI/Program.cs` for `IProductService`, `IProductsRepository`, `IConnectionProvider` and MediatR handler registration.
- If Swagger/UI fails, ensure `Swashbuckle.AspNetCore` is referenced in the `WebAPI` project.

If you want, I can add sample SQL scripts to seed data, or wire EF Core instead of the current ADO/Dapper repository implementation.