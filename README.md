# 🚀 LearnRestAPI

> A structured ASP.NET Core Web API demonstrating **CQRS (MediatR)**, **JWT Authentication**, and **Redis Caching (Dockerized)** with clear separation of concerns.

---

# 🧭 Quick Start Guide

## 🖥️ Prerequisites (Windows)

* Enable **WSL / SVM (Virtualization)** from BIOS
* Install **Docker Desktop** (or Docker Engine via WSL)
* Start Docker

---

## ⚙️ Configuration

Update the following in `appsettings.json`:

```json
ConnectionStrings:DefaultConnection
Redis:ConnectionString
Jwt:Key
Jwt:Issuer
Jwt:Audience
```

Also verify ports in:

```
launchSettings.json
```

---

## 🐳 Start Redis

```bash
docker-compose up
```

---

## 🗄️ Setup Database

* Run required SQL scripts
* Ensure tables like `Users`, `Products` exist

---

## ▶️ Run Application

```bash
dotnet build
dotnet run
```

---

## 🔐 Authentication Flow

1. Register user
2. Login
3. Copy JWT token
4. Use in APIs:

```http
Authorization: Bearer <token>
```

---

# 🧠 Architecture Overview

```
Controller → Service → MediatR → Handler → Repository → Database
                                      ↓
                                   Redis Cache
```

---

# 🔄 Application Flow (End-to-End)

## 🟢 Application Startup

* Loads configuration from `appsettings.json`
* Registers:

  * Controllers
  * Services
  * MediatR
  * Repositories
  * Redis
  * JWT Authentication

### Middleware Pipeline

```
UseRouting → UseAuthentication → UseAuthorization → MapControllers
```

---

# 🔐 JWT Authentication Deep Dive

## ⚙️ Setup (Program.cs)

* Uses:

  * Secret Key
  * Issuer
  * Audience

* Validates:

  * Token signature
  * Expiry
  * Issuer & Audience

---

## 🧾 Token Generation (UserController)

### Flow

```
Request → Validate User → Create Claims → Generate Token → Return Token
```

### Claims Include

* Username
* Role

---

## 🔄 Authenticated Request Flow

```
Request → UseAuthentication → Token Validation → Claims Attached → UseAuthorization → Controller
```

---

# 📦 Product Module

---

## 🎯 Controller Layer

Handles endpoints:

* `GET /products`
* `POST /products`
* `PUT /products`
* `DELETE /products`

```
Controller → ProductService
```

---

## 🔧 Service Layer

Acts as abstraction over MediatR:

```
_sender.Send(...)
```

| Operation | Request              |
| --------- | -------------------- |
| Get       | GetProductsQuery     |
| Add       | AddProductCommand    |
| Update    | UpdateProductCommand |
| Delete    | DeleteProductCommand |

---

## 🧠 MediatR Layer

Routes requests to handlers:

```
Query/Command → Handler
```

---

# 🧩 Handlers (Core Logic)

---

## 🟢 GetProductsHandler

```
1. Check Redis (key: "all_products")
2. If hit → return data
3. If miss:
   → Fetch from DB
   → Store in Redis
   → Return
```

---

## 🟡 AddProductHandler

```
1. Receive DTO
2. Map to entity
3. Save via repository
4. Return result
```

---

## 🟠 UpdateProductHandler

```
1. Fetch product
2. Update fields
3. Save changes
4. Return updated entity
```

---

## 🔴 DeleteProductHandler

```
1. Find product
2. Delete from repository
3. Persist
4. Return status
```

---

# ⚠️ Caching Behavior

| Operation | Cache Usage     |
| --------- | --------------- |
| GET       | Uses Redis      |
| ADD       | No cache update |
| UPDATE    | No cache update |
| DELETE    | No cache update |

### Cache Key

```
all_products
```

---

# 🧱 Repository Layer

### Interface

```
IProductsRepository
```

### Responsibilities

* Fetch data
* Insert
* Update
* Delete

> No business logic here

---

# 🧠 Caching Layer

### Interface

```
ICachingService
```

### Implementation

```
RedisCachingService
```

### Responsibilities

* Serialize data
* Store in Redis
* Retrieve data
* Abstract Redis logic

---

# 🐳 Redis Setup

* Defined in `docker-compose.yml`
* Runs as separate container
* Connected via configuration

---

# 🔗 Full System Flow

```
Client Request
      ↓
Controller
      ↓
Service
      ↓
MediatR
      ↓
Handler
      ↓
 ┌───────────────┐
 │ Redis Cache   │ (GET only)
 └──────┬────────┘
        ↓ (miss)
   Repository
        ↓
   Database
        ↓
   Cache Store
        ↓
Response → Client
```

---

# 🧩 Design Summary

* Controllers → thin
* Services → MediatR abstraction
* Handlers → business logic
* Repositories → data access
* Cache → read optimization
* JWT → centralized auth

---

# 📌 Final Mental Model

```
Auth Flow:
Login → Generate JWT → Use Token → Access APIs

Data Flow:
Controller → Service → MediatR → Handler → Repo → DB → Cache → Response
```

---

> This README is designed to give a **complete mental model** of how the application works — from setup to execution to internal flow.
