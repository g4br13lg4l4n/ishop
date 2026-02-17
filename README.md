# iShop

A modern e-commerce API built with ASP.NET Core, following Clean Architecture principles. This project provides a RESTful API for managing products in an online store.

## 🏗️ Architecture

This project follows Clean Architecture with three main layers:

- **API**: Presentation layer containing controllers and API configuration
- **Core**: Domain layer containing business entities and interfaces
- **Infrastructure**: Data access layer containing database context, repositories, migrations, and configurations

## 🛠️ Technology Stack

- **.NET 10.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 10.0.3**
- **SQL Server** (via Docker)
- **OpenAPI/Swagger** support

## 📋 Prerequisites

Before running this project, ensure you have the following installed:

- [.NET SDK 10.0](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (for SQL Server)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (optional, if not using Docker)

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd iShop
```

### 2. Start SQL Server with Docker

The project includes a `docker-compose.yml` file for easy database setup:

```bash
cd Infrastructure
docker-compose up -d
```

This will start a SQL Server 2022 container on port 1433 with:
- **Username**: SA
- **Password**: Password1!
- **Database**: skina (will be created automatically)

### 3. Configure Connection String

Update the connection string in `API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=skina;User Id=SA;Password=Password1!;TrustServerCertificate=True"
  }
}
```

### 4. Run Database Migrations

Navigate to the Infrastructure project and run migrations:

```bash
cd Infrastructure
dotnet ef database update --startup-project ../API
```

### 5. Run the API

Navigate to the API project and run:

```bash
cd API
dotnet run
```

The API will be available at `https://localhost:5001` or `http://localhost:5000` (check `launchSettings.json` for exact ports).

## 📡 API Endpoints

### Products

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get all products (optional: `?brand=&type=&sort=`) |
| GET | `/api/products/{id}` | Get a product by ID |
| GET | `/api/products/brands` | Get distinct brand names (for filters) |
| GET | `/api/products/types` | Get distinct type names (for filters) |
| POST | `/api/products` | Create a new product |
| PUT | `/api/products/{id}` | Update an existing product |
| DELETE | `/api/products/{id}` | Delete a product |

### Product Model

```json
{
  "id": 0,
  "name": "string",
  "description": "string",
  "price": 0.00,
  "pictureUrl": "string",
  "type": "string",
  "brand": "string",
  "quantityInStock": 0
}
```

### Example Requests

**Get all products:**
```bash
GET https://localhost:5001/api/products
```

**Create a product:**
```bash
POST https://localhost:5001/api/products
Content-Type: application/json

{
  "name": "Laptop",
  "description": "High-performance laptop",
  "price": 999.99,
  "pictureUrl": "https://example.com/laptop.jpg",
  "type": "Electronics",
  "brand": "TechBrand",
  "quantityInStock": 10
}
```

**Update a product:**
```bash
PUT https://localhost:5001/api/products/1
Content-Type: application/json

{
  "id": 1,
  "name": "Updated Laptop",
  "description": "Updated description",
  "price": 899.99,
  "pictureUrl": "https://example.com/laptop.jpg",
  "type": "Electronics",
  "brand": "TechBrand",
  "quantityInStock": 5
}
```

**Delete a product:**
```bash
DELETE https://localhost:5001/api/products/1
```

## 📁 Project Structure

```
iShop/
├── API/                              # Presentation layer
│   ├── Controllers/
│   │   └── ProductsController.cs     # Product endpoints (CRUD + filtering)
│   ├── Program.cs                    # Application bootstrap & DI
│   └── appsettings.json              # Configuration
├── Core/                             # Domain layer
│   ├── Entities/
│   │   ├── BaseEntity.cs             # Base entity with Id
│   │   └── Product.cs                # Product entity
│   ├── Interfaces/
│   │   ├── IGenericRepository.cs     # Generic repository contract for entities
│   │   └── ISpecification.cs         # Specification contract for queries
│   └── Specifications/
│       ├── BaseSpecification.cs      # Base implementation of ISpecification<T>
│       └── ProductSpecification.cs   # Product-specific filter/sort specification
└── Infrastructure/                   # Data access layer
    ├── config/
    │   └── ProductConfiguration.cs   # EF Core configuration
    ├── Data/
    │   ├── StoreContext.cs           # DbContext
    │   ├── GenericRepository.cs      # Generic repository implementation
    │   └── SpecificationEvaluator.cs # Applies specifications to EF queries
    ├── Migrations/                   # Database migrations
    └── docker-compose.yml            # SQL Server container setup
```

## 🧱 Repository & Specification Pattern

The products API uses a **generic repository** combined with the **Specification pattern** to keep data access logic reusable and testable:

- **Generic repository interface**: `Core/Interfaces/IGenericRepository.cs`
  - `Task<T?> GetByIdAsync(int id)`
  - `Task<IReadOnlyList<T>> ListAllAsync()`
  - `Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)`
  - `Task<T?> GetEntityWithSpec(ISpecification<T> spec)`
  - `Task<int> CountAsync(ISpecification<T> spec)`
  - `void Add(T entity)`, `Update(T entity)`, `Remove(T entity)`
  - `Task<bool> SaveAllAsync()`, `bool Exists(int id)`

- **Specification contract**: `Core/Interfaces/ISpecification.cs`
  - Exposes `Criteria`, `OrderBy`, `OrderByDescending`, `Includes`, `Skip`, `Take`, and `IsPagingEnabled`
  - Implemented by `BaseSpecification<T>` and specialized specs like `ProductSpecification`

- **Implementation**: `Infrastructure/Data/GenericRepository.cs`
  - Wraps `StoreContext` / `DbSet<T>`
  - Delegates query composition to `SpecificationEvaluator<T>.GetQuery(...)`

- **Specification evaluator**: `Infrastructure/Data/SpecificationEvaluator.cs`
  - Applies `Criteria`, sorting, and paging from an `ISpecification<T>` to an `IQueryable<T>`

- **Example spec**: `Core/Specifications/ProductSpecification.cs`
  - Builds a query based on `brand`, `type`, and `sort` (price ascending/descending or name)

`ProductsController` depends on `IGenericRepository<Product>` and passes a `ProductSpecification` to retrieve filtered and sorted products.

## 🔁 Data Flow

High-level request/data flow for the products API:

- **GET `/api/products` (list, filter, sort)**  
  1. The client calls `/api/products?brand=Nike&type=Shoes&sort=priceAsc`.  
  2. `ProductsController.GetProducts` creates a `ProductSpecification` with the query parameters.  
  3. The controller calls `IGenericRepository<Product>.ListAsync(spec)`.  
  4. `GenericRepository` calls `SpecificationEvaluator<Product>.GetQuery(context.Set<Product>(), spec)` to apply filters, sorting, and paging.  
  5. EF Core translates the resulting LINQ query to SQL and executes it against SQL Server.  
  6. The list of `Product` entities is returned to the controller and serialized as JSON in the HTTP response.

- **GET `/api/products/{id}` (details)**  
  1. Controller calls `repo.GetByIdAsync(id)`.  
  2. `GenericRepository` uses `context.Set<Product>().FindAsync(id)`.  
  3. The product (or `null`) is returned and mapped to `200 OK` / `404 NotFound`.

- **POST / PUT / DELETE (write operations)**  
  1. The controller calls `Add` / `Update` / `Remove` on `IGenericRepository<Product>`.  
  2. Changes are tracked by `StoreContext`.  
  3. `SaveAllAsync` calls `context.SaveChangesAsync()`, which persists changes to SQL Server.  
  4. The controller returns `201 Created`, `200 OK`, or appropriate error responses based on the result.

## 📋 Brands and Types Flow

The API exposes **distinct lists of brands and types** from products so clients can build filters (e.g. dropdowns) without loading full product data. Both flows use the same pattern: a **projection specification** (`ISpecification<T, TResult>`) that selects a single column and applies `Distinct`.

### Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products/brands` | List of distinct product brand names |
| GET | `/api/products/types`  | List of distinct product type names |

### Flow (brands and types are identical in shape)

1. **Request**  
   Client calls `GET /api/products/brands` or `GET /api/products/types`.

2. **Controller**  
   `ProductsController.GetBrands()` or `GetTypes()` creates a **BrandListSpecification** or **TypeListSpecification** (no query parameters) and calls:
   - `repo.ListAsync(spec)`  
   where the repository method used is `ListAsync<TResult>(ISpecification<T, TResult> spec)`, with `T = Product` and `TResult = string`.

3. **Specification**  
   - **BrandListSpecification** extends `BaseSpecification<Product, string>` and in its constructor calls `AddSelect(p => p.Brand)` and `SetIsDistinct()`.  
   - **TypeListSpecification** extends `BaseSpecification<Product, string>` and calls `AddSelect(p => p.Type)` and `SetIsDistinct()`.  
   So the spec describes: “from `Product`, select this string property and return distinct values.”

4. **Repository**  
   `GenericRepository<Product>.ListAsync(spec)` calls `ApplySpecification(spec)`, which uses **SpecificationEvaluator** `GetQuery<T, TResult>`: applies (optional) criteria, ordering, paging, then **projects** with `spec.Select` and applies **Distinct** when `spec.IsDistinct` is true.

5. **Database**  
   EF Core turns that into a SQL query over the `Products` table that selects only the chosen column and uses `DISTINCT` (e.g. `SELECT DISTINCT Brand FROM Products`), so only unique brands or types are returned.

6. **Response**  
   The controller receives `IReadOnlyList<string>` and returns it as JSON (e.g. `["Nike", "Adidas", "Puma"]`).

### Summary

- **Brands**: `BrandListSpecification` → select `Product.Brand` → distinct → `GET /api/products/brands` returns `string[]`.  
- **Types**: `TypeListSpecification` → select `Product.Type` → distinct → `GET /api/products/types` returns `string[]`.  
- Same pipeline: controller → generic repository `ListAsync<TResult>` → specification evaluator (project + distinct) → SQL → JSON list of strings.

## 🔧 Development

### Adding a New Migration

```bash
cd Infrastructure
dotnet ef migrations add MigrationName --startup-project ../API
```

### Updating the Database

```bash
cd Infrastructure
dotnet ef database update --startup-project ../API
```

### Removing the Last Migration

```bash
cd Infrastructure
dotnet ef migrations remove --startup-project ../API
```

## 🧪 Testing

You can test the API endpoints using:
- **Postman**
- **Swagger UI** (if enabled in `Program.cs`)
- **curl** or **HTTPie**
- **Visual Studio** REST Client

## 📝 Notes

- The project uses Entity Framework Core Code-First approach
- Database migrations are stored in `Infrastructure/Migrations/`
- The `BaseEntity` class provides a common `Id` property for all entities
- The API uses a repository pattern via `IProductRepository` and `ProductRepository` instead of accessing `StoreContext` directly from controllers
- Product prices are stored as `decimal(18,2)` in the database
- The API uses nullable reference types (enabled in project settings)

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License.

## 👤 Author

Created as part of .NET development learning journey.

---

**Happy Coding! 🚀**
