# 🏗️ Clean Architecture Template

A **simple ASP.NET Core solution** implementing Clean Architecture principles without external libraries like MediatR, AutoMapper, or FluentValidation. Built with native C# capabilities for clarity and maintainability.

> **Based on**: [CleanArchitecture.WebApi](https://github.com/iammukeshm/CleanArchitecture.WebApi) by Mukesh Murugan  
> **Updated**: Modernized and removed external dependencies for a cleaner, simpler approach

## 🎯 Architecture Overview

```
src/
├── Domain/              # Core business logic (entities, value objects)
├── Application/         # Use cases, DTOs, service interfaces
├── Infrastructure.Identity/      # ASP.NET Core Identity integration
├── Infrastructure.Persistence/   # Entity Framework Core data access
├── Infrastructure.Shared/        # Cross-cutting concerns
└── WebApi/             # Controllers and API configuration
```

## ✨ Key Features

- **No External Abstractions**: Direct service calls, manual mapping, domain validation
- **Clean Dependencies**: Outer layers depend on inner layers only
- **Native C# Only**: Uses built-in .NET capabilities
- **Security Ready**: ASP.NET Core Identity with JWT support
- **Testable**: Easy unit and integration testing

## 🛠️ Stack

- **Framework**: ASP.NET Core 8+
- **Database**: Entity Framework Core + SQL Server
- **Authentication**: ASP.NET Core Identity
- **Documentation**: Swagger/OpenAPI

---

**License**: MIT | **Focus**: Simplicity over complexity
