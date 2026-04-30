# LearnRestAPI

A layered ASP.NET Core Web API project demonstrating:

* JWT Authentication & Authorization
* CQRS with MediatR
* Repository Pattern
* Dapper + SQL Stored Procedures
* Redis Caching
* Kafka Producer/Consumers
* Background Jobs
* Serilog Logging
* Pagination & Dynamic Sorting
* Inventory and Order Processing

---

# How To Setup The Project

## Prerequisites (Windows)

### Enable Virtualization

Enable the following from BIOS:

* WSL
* SVM / Virtualization

---

## Install Required Tools

Install:

* Docker Desktop (recommended)
  OR
* Docker Engine via WSL

Start Docker before running the project.

---

# Configuration

Update the following values inside:

* `appsettings.json`
* `launchSettings.json`

## Database Connection

```json
environmentVariables:Database_Connection_String
```

---

## Redis

```json
environmentVariables:Redis_Connection_String
```

---

## JWT Authentication

```json
Jwt:Key
Jwt:Issuer
Jwt:Audience
```

---

## Kafka Configuration

Update all Kafka related variables inside:

```json
Kafka:BootstrapServers
Kafka:AllowAutoCreateTopics
Kafka:Acks
Kafka:MessageSendMaxRetries
Kafka:RetryBackoffMs
Kafka:EnableIdempotence
```

---

## Email Configuration

Update email variables:

```json
Email:Address
Email:Password
```

---

## Verify Application Ports

Verify ports inside:

```text
Properties/launchSettings.json
```

---

# Up docker for Redis, Zookeeper, Kafka

Run:

```bash
docker-compose up
```

This starts:

* Redis Container
* Kafka Container
* zookeeper Container

---

# Setup Database

## Run SQL Scripts

Execute all required SQL scripts.

Ensure database objects exist:

* Users
* Products
* Orders
* Inventory
* Stored Procedures

If not run in order:

```sql
DDL queries
Table Value Parameters
Stored Procedures
```

---

# Run Application

```bash
dotnet build
```

```bash
dotnet run
```

Swagger should become available after startup.

---

# Authentication Flow

## Register User

Call:

```http
POST /api/user/register
```

---

## Login

Call:

```http
POST /api/user/login
```

Response returns:

* JWT Token

---

## Use JWT Token

Add token in request header:

```http
Authorization: Bearer <token>
```

Protected APIs validate the JWT token before allowing access.

---

# High Level Architecture

```text
Client
   ↓
Controllers
   ↓
Services
   ↓
MediatR Queries / Commands
   ↓
Handlers
   ↓
Repositories
   ↓
Dapper
   ↓
SQL Stored Procedures
   ↓
SQL Server
```

---

# Solution Architecture

## WebAPI Layer

Responsible for:

* API endpoints
* HTTP request handling
* Authentication
* Swagger
* Dependency Injection
* Middleware registration

Contains:

* Controllers
* Program.cs
* Middleware configuration

---

## ApplicationCore Layer

Responsible for:

* Business logic
* CQRS queries/commands
* Handlers
* Services
* Interfaces
* DTOs
* Kafka integrations
* Caching abstractions

---

## Infrastructure/Data Access Layer

Responsible for:

* Repository implementations
* Database connections
* Dapper execution
* Stored procedure calls
* Redis caching implementation

---

# Program.cs Flow

`Program.cs` is the application entry point.

Main responsibilities:

* Configure dependency injection
* Configure JWT authentication
* Configure Redis
* Configure Kafka
* Configure Serilog
* Register MediatR handlers
* Register repositories/services
* Configure Swagger
* Build middleware pipeline

---

# Dependency Injection Flow

Services registered in `Program.cs`:

```text
IProductService → ProductService
IOrderService → OrderService
IInventoryService → InventoryService
IUserService → UserService

IProductsRepository → ProductsRepository
IOrdersRepository → OrdersRepository
IInventoryRepository → InventoryRepository
```

These registrations allow constructor injection throughout the application.

---

# JWT Authentication Flow

JWT configuration is loaded from:

```json
Jwt
```

The login endpoint itself does NOT require JWT authentication.

Reason:

* User does not yet have a token during login
* Login endpoint is responsible for generating the JWT token

---

# Register Flow

```http
POST /api/user/register
```

Flow:

```text
Client
   ↓
UserController
   ↓
UserService
   ↓
RegisterUserCommand
   ↓
Handler
   ↓
PasswordHelper Hashes Password
   ↓
Repository
   ↓
Stored Procedure
   ↓
User Saved In Database
```

Passwords are NOT stored as plain text.

The password is hashed using `PasswordHelper.cs` before storing in the database.

---

# Login Flow

```http
POST /api/user/login
```

The login endpoint is publicly accessible.

No JWT token is required for login.

Flow:

```text
Client
   ↓
UserController
   ↓
UserService
   ↓
Login Command/Query
   ↓
Handler
   ↓
Repository
   ↓
Fetch User From Database
   ↓
Compare Hashed Password Using PasswordHelper
   ↓
Generate JWT Token
   ↓
Return JWT Token
```

During login:

1. Username/email is validated
2. User record is fetched from database
3. Stored hashed password is retrieved
4. PasswordHelper compares entered password with stored hash
5. JWT token is generated if credentials are valid
6. JWT token is returned to client

If credentials are invalid:

* Unauthorized response is returned

---

# Using JWT Token

After successful login:

```text
Client Receives JWT Token
   ↓
Client Stores Token
   ↓
Client Sends Token In Authorization Header
```

Header format:

```http
Authorization: Bearer <token>
```

Protected APIs validate the JWT token before allowing access.

Flow:

```text
Client Request
   ↓
JWT Authentication Middleware
   ↓
Validate Token
   ↓
Extract Claims
   ↓
Authorize Request
   ↓
Controller Executes
```

If token is invalid:

* 401 Unauthorized is returned

---

# JWT Role Authorization

Role based authorization using:

```text
JWT.Claims.Role
```

is planned but not fully implemented yet.

Future implementation may allow:

```text
[Authorize(Roles = "Admin")]
```

Examples:

* Admin only inventory APIs
* User specific order APIs
* Restricted management endpoints

---

# Request Execution Flow

Example:

```http
GET /api/products
```

Flow:

```text
Client
   ↓
ProductsController
   ↓
ProductService
   ↓
MediatR Send()
   ↓
GetProductsQuery
   ↓
GetProductsQueryHandler
   ↓
IProductsRepository
   ↓
ProductsRepository
   ↓
Dapper
   ↓
Stored Procedure
   ↓
SQL Server
```

Data then returns back through the same layers.

---

# Controllers

Controllers only handle:

* HTTP requests
* Request parameters
* Returning HTTP responses

Controllers do NOT contain:

* SQL logic
* Business logic
* Database access code

Examples:

* ProductsController
* OrdersController
* InventoryController
* UserController

---

# Services Layer

Services act as abstraction between controllers and MediatR.

Responsibilities:

* Forward requests
* Coordinate business operations
* Keep controllers lightweight

Example:

```text
Controller
   ↓
Service
   ↓
Mediator
```

---

# CQRS + MediatR Implementation

The project uses MediatR for CQRS style request handling.

## Query/Command Flow

```text
Controller
   ↓
Service
   ↓
ISender.Send()
   ↓
Query / Command
   ↓
Handler
```

---

## Queries

Queries are used for read operations.

Examples:

```text
GetProductsQuery
GetOrdersQuery
GetInventoryQuery
```

---

## Commands

Commands are used for write operations.

Examples:

```text
AddProductCommand
CreateOrderCommand
UpdateInventoryCommand
```

---

# Handlers

Handlers contain actual business logic.

Responsibilities:

* Validation
* Processing requests
* Calling repositories
* Data transformation
* Pagination/sorting logic

Example:

```text
GetProductsQueryHandler
```

Flow:

```text
Query
   ↓
Handler
   ↓
Repository
```

---

# Repository Pattern

Repositories abstract database access.

Responsibilities:

* Execute SQL queries
* Execute stored procedures
* Map results
* Return domain models

Examples:

```text
ProductsRepository
OrdersRepository
InventoryRepository
```

---

# Dapper Integration

The project uses Dapper instead of Entity Framework.

Advantages:

* Lightweight
* Faster execution
* Full SQL control
* Better stored procedure integration

Flow:

```text
Repository
   ↓
SqlConnection
   ↓
Dapper ExecuteAsync()
   ↓
Stored Procedure
```

---

# Stored Procedure Flow

Most database operations are executed through SQL stored procedures.

Example:

```text
Controller
   ↓
Handler
   ↓
Repository
   ↓
Stored Procedure
   ↓
SQL Tables
```

Benefits:

* Better SQL encapsulation
* Reusable database logic
* Easier optimization
* Reduced inline SQL in application code

---

# Products Module Flow

## Get Products

```http
GET /api/products
```

Flow:

```text
ProductsController
   ↓
ProductService
   ↓
GetProductsQuery
   ↓
GetProductsQueryHandler
   ↓
ProductsRepository
   ↓
GetProducts Stored Procedure
```

---

## Add Product

```http
POST /api/products
```

Flow:

```text
ProductsController
   ↓
ProductService
   ↓
AddProductCommand
   ↓
AddProductCommandHandler
   ↓
ProductsRepository
   ↓
AddProduct Stored Procedure
```

---

# Orders Module Flow

Orders module handles order creation and processing.

## Create Order Flow

```http
POST /api/orders
```

Flow:

```text
OrdersController
   ↓
OrderService
   ↓
CreateOrderCommand
   ↓
CreateOrderHandler
   ↓
OrdersRepository
   ↓
CreateOrder Stored Procedure
   ↓
Publish Kafka Event
```

After order creation:

* Kafka producer publishes event
* Inventory consumer updates stock
* Email consumer sends notification

---

# Inventory Module Flow

Inventory module manages stock quantities.

## Inventory Update Flow

```text
Kafka Consumer Receives Order Event
   ↓
InventoryJob
   ↓
Inventory Service
   ↓
UpdateInventoryCommand
   ↓
Repository
   ↓
Database
```

Responsibilities:

* Reduce stock
* Validate inventory
* Prevent invalid stock states

---

# User Module Flow

## Register Flow

```text
UserController
   ↓
UserService
   ↓
RegisterUserCommand
   ↓
Handler
   ↓
Repository
   ↓
RegisterUser Stored Procedure
```

---

## Login Flow

```text
UserController
   ↓
UserService
   ↓
Validate Credentials
   ↓
Generate JWT Token
   ↓
Return Token
```

---

# Redis Caching

The project integrates Redis caching.

Redis is used to reduce repeated database calls.

---

# Why Caching Is Used

Without caching:

```text
Every Request
   ↓
Database Call
```

With Redis:

```text
Request
   ↓
Check Redis Cache
   ↓
Cache Hit → Return Cached Data
OR
Cache Miss → Fetch From Database → Store In Redis
```

Benefits:

* Faster response times
* Reduced SQL load
* Better scalability
* Lower database traffic

---

# Cached Modules

Caching is mainly useful for:

* Product list
* Frequently accessed data
* Read-heavy endpoints

Examples:

```text
GET /api/products
GET /api/inventory
```

---

# Redis Caching Flow

```text
Controller
   ↓
Service
   ↓
Check Redis Cache
   ↓
If Exists → Return Cached Data
Else
   ↓
Repository
   ↓
Database
   ↓
Store Response In Redis
```

---

# Kafka Integration

Kafka is used for asynchronous event-driven processing.

Kafka helps decouple modules.

Instead of directly calling inventory/email logic synchronously:

```text
Order Created
   ↓
Publish Event
   ↓
Consumers Process Independently
```

This improves:

* Scalability
* Reliability
* Performance
* Loose coupling

---

# Kafka Producer

Kafka producer publishes messages after important events.

Main use case:

* Order creation
* Inventory Updation
* Sending Order confirmation Email

Example:

```text
Order Created
   ↓
Kafka Producer
   ↓
Publish Order Event
```

---

# Kafka Consumers

The project contains Kafka consumers:

* InventoryJob
* EmailJob

These run as background consumers.

---

# InventoryJob Consumer

Responsibilities:

* Consume order events
* Update inventory stock
* Maintain inventory consistency

Flow:

```text
Kafka Topic
   ↓
InventoryJob Consumer
   ↓
Inventory Service
   ↓
Update Inventory
```

---

# EmailJob Consumer

Responsibilities:

* Consume order events
* Send confirmation emails
* Notify users

Flow:

```text
Kafka Topic
   ↓
EmailJob Consumer
   ↓
Email Service
   ↓
Send Email
```

---

# Why Kafka Is Important Here

Without Kafka:

```text
Create Order
   ↓
Update Inventory
   ↓
Send Email
   ↓
Return Response
```

This increases response time.

With Kafka:

```text
Create Order
   ↓
Publish Event
   ↓
Return Response Immediately

Consumers process tasks separately.
```

This creates asynchronous processing.

---

# Serilog Logging

The project uses Serilog for structured logging.

Serilog is configured from:

```json
appsettings.json
```

---

# Serilog Responsibilities

* Log API requests
* Log errors
* Log exceptions
* Log Kafka processing
* Log background jobs
* Log debugging information

---

# Logging Flow

```text
Request
   ↓
Controller/Service
   ↓
ILogger / Serilog
   ↓
Console/File/Sink
```

---

# Why Serilog Is Used

Advantages:

* Structured logs
* Better debugging
* Production monitoring
* Easy sink integration
* Centralized logging

---

# Pagination & Sorting

The project supports:

* Pagination
* Dynamic sorting
* Search filtering

---

# Pagination Flow

```text
Query Parameters
   ↓
Handler
   ↓
Repository
   ↓
Stored Procedure / Query
   ↓
Paged Result
```

Examples:

```http
?pageNumber=1&pageSize=10
```

---

# Dynamic Sorting

Sorting is implemented dynamically using expression trees.

Example:

```http
?sortBy=Price
```

Runtime conversion:

```text
"Price"
   ↓
Expression Tree
   ↓
x => x.Price
```

---

# Overall End-To-End Order Flow

```text
Client Creates Order
   ↓
OrdersController
   ↓
OrderService
   ↓
CreateOrderCommand
   ↓
CreateOrderHandler
   ↓
OrdersRepository
   ↓
SQL Stored Procedure
   ↓
Order Saved
   ↓
Kafka Producer Publishes Event
   ↓
InventoryJob Consumes Event
   ↓
Inventory Updated
   ↓
EmailJob Consumes Event
   ↓
Confirmation Email Sent
```

---

# Technologies Used

## Backend

* ASP.NET Core Web API
* C#
* MediatR
* Dapper
* SQL Server
* Redis
* Kafka
* Serilog

---

# Design Patterns Used

* Repository Pattern
* CQRS Pattern
* Dependency Injection
* Mediator Pattern
* Background Consumer Pattern
* Caching Pattern

---

# Future Improvements

Possible future enhancements:

* Role based endpoint access 
* FluentValidation
* Global Exception Middleware
* ProblemDetails
* Health Checks
* Retry Policies
* Circuit Breakers
* API Versioning
* Unit Tests

---

# Summary

This project demonstrates a scalable enterprise-style ASP.NET Core Web API architecture using:

* Layered architecture
* CQRS with MediatR
* Repository pattern
* Dapper + Stored Procedures
* Redis caching
* Kafka event-driven processing
* JWT authentication
* Serilog logging

The project separates concerns cleanly and demonstrates how modern distributed backend systems communicate asynchronously while remaining modular and maintainable.
