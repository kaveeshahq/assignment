# FlowDesk Task Board API

A lightweight Task Board backend service for FlowDesk, built with ASP.NET Core.

## Project Structure

- **FlowDesk.Api** - ASP.NET Core REST API
- **FlowDesk.Domain** - Domain entities and interfaces
- **FlowDesk.Data** - Entity Framework Core database layer
- **FlowDesk.Services** - Business logic layer
- **FlowDesk.Tests** - Unit and integration tests

## Architecture

The project follows a layered architecture:

- **Presentation Layer** (API) - Controllers and request handling
- **Business Logic Layer** (Services) - Business rules and validation
- **Data Access Layer** (Data) - EF Core and repositories
- **Domain Layer** (Domain) - Entities and interfaces

## Prerequisites

- .NET 10.0+
- SQL Server 2019+ (or SQL Server Express)

## Getting Started

### 1. Setup Database

```bash
dotnet ef database update --project FlowDesk.Data
```

### 2. Run the API

```bash
dotnet run --project FlowDesk.Api
```

The API will be available at `https://localhost:5001`

## Features

- Task creation and management
- Project overview with filtering and sorting
- Task status workflow (To Do, In Progress, Done)
- Task archiving
- Access control and authorization
- Structured logging with Serilog
- Comprehensive error handling

## API Documentation

See the Postman collection for API endpoints and examples.

## Design Decisions

### Architecture

- Layered architecture for separation of concerns
- Dependency injection for loose coupling
- Repository pattern for data access
- Service layer for business logic

### Database

- SQL Server for persistence
- Entity Framework Core for ORM
- Code-first migrations for version control

### Validation

- FluentValidation for input validation
- Business rule validation in services

### Logging

- Serilog for structured logging
- Log levels: Debug, Information, Warning, Error, Fatal

### Error Handling

- Global exception middleware
- Consistent error response format
- Meaningful error messages for clients

## Known Limitations

(Will be updated during development)

## Future Improvements

(Will be updated during development)
