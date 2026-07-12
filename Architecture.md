 # Volunteer Task Management System Architecture

## Overview

The Volunteer Task Management System is designed to coordinate volunteer activities during crisis situations. In emergency events, volunteers often participate in activities such as debris removal, food distribution, and assisting vulnerable individuals. Without proper coordination, some locations may become overcrowded with volunteers while others receive little or no assistance.

The system provides a centralized platform that enables coordinators to create and manage volunteer tasks while allowing volunteers to participate in available activities based on their geographical location.

From an architectural perspective, the project demonstrates the application of Clean Architecture together with CQRS, MediatR, the State Pattern, Optimistic Concurrency Control, and real-time communication using SignalR.

---

# Architectural Goals

The architecture was designed to satisfy both functional and non-functional requirements of the system. The primary objectives are described below.

- **Separation of Concerns:** Each layer is responsible for a single aspect of the application, reducing complexity and making the codebase easier to understand.

- **Maintainability:** The architecture allows individual components to evolve independently with minimal impact on other layers.

- **Low Coupling:** Business logic depends on abstractions rather than concrete implementations, improving flexibility and enabling easier replacement of infrastructure components.

- **Reusability:** Shared functionality is extracted into reusable Base libraries, allowing common services and utilities to be reused across multiple projects.

- **Scalability:** The layered design and clear separation between read and write operations allow the application to grow without introducing excessive complexity.

- **Testability:** Business logic is isolated from external dependencies, making unit testing straightforward.

- **Consistency:** Validation, exception handling, authentication, and other cross-cutting concerns are centralized, ensuring consistent behavior throughout the application.

- **Concurrency Safety:** The system detects conflicting updates using optimistic concurrency control, preventing data inconsistency caused by simultaneous operations.

---

# System Roles

The system defines two primary roles.

## Coordinator

Responsible for:

- Creating volunteer tasks
- Managing task lifecycle
- Monitoring volunteer participation
- Coordinating activities

## Volunteer

Responsible for:

- Browsing available tasks
- Registering for volunteer activities
- Participating in assigned tasks
- Receiving task notifications

---

# Domain Model

The application consists of the following core entities.

| Entity | Responsibility |
|---------|----------------|
| User | Represents both Coordinators and Volunteers |
| VolunteerTask | Represents volunteer activities |
| UserTask | Maps volunteers to their assigned tasks |
| Province | Province information |
| City | City information |
| Region | Region information |
| Neighborhood | Smallest geographical unit |
| NotificationLog | Stores notification history |

---

# Geographical Hierarchy

Volunteer tasks are associated with a geographical hierarchy.

```text
Province
    └── City
            └── Region
                    └── Neighborhood
```

This hierarchy allows filtering volunteer tasks by:

- Province
- City
- Region
- Neighborhood

---

# Task Lifecycle

The lifecycle of each volunteer task is implemented using the State Pattern.

![Task State Diagram](StateDiagram.png)

Each state encapsulates its own behavior and defines only the transitions that are allowed.

This design prevents invalid state transitions while keeping lifecycle logic isolated from the main entity.

---

# Solution Structure

The project follows Clean Architecture.

```text
VolunteerTaskManagement.API
VolunteerTaskManagement.Application
VolunteerTaskManagement.Domain
VolunteerTaskManagement.Infrastructure

Base.Api
Base.Application
Base.Application.Contracts
Base.Domain
Base.Infrastructure
```

---

# Layer Responsibilities

## API

Responsible for:

- REST endpoints
- Dependency Injection
- Authentication configuration
- Authorization
- Swagger
- SignalR configuration

---

## Application

Responsible for:

- CQRS
- MediatR handlers
- Business workflows
- Validation
- DTOs

---

## Domain

Responsible for:

- Entities
- Business rules
- State Pattern
- Domain logic
- Enumerations

The Domain layer contains no infrastructure dependencies.

---

## Infrastructure

Responsible for:

- Entity Framework Core
- Repository implementation
- Unit of Work
- SQL Server
- JWT implementation
- MinIO integration
- SignalR services

---

# Shared Base Libraries

The solution contains reusable libraries shared across multiple applications.

## Base.Api

Provides:

- Shared middleware registration
- Swagger configuration
- Common API configuration

## Base.Application

Provides:

- Global exception handling
- Shared exceptions
- Date conversion utilities
- Common helper classes

## Base.Application.Contracts

Contains shared abstractions.

Examples include:

- IRepository
- IUnitOfWork
- IJwtManager
- IMinIoService

It also contains common DTOs used across applications.

## Base.Domain

Contains:

- BaseEntity
- Shared enums
- Common domain abstractions

## Base.Infrastructure

Provides implementations of shared contracts including:

- Generic Repository
- Unit of Work
- JWT Manager
- MinIO Service
- Base DbContext
- Soft Delete implementation

---

# Request Processing Flow

The application uses CQRS together with MediatR.

Every request follows the pipeline below.

```text
Client
    │
    ▼
Controller
    │
    ▼
MediatR
    │
    ▼
ValidationPipelineBehavior
    │
    ▼
FluentValidation
    │
    ▼
Command / Query Handler
    │
    ▼
Unit Of Work
    │
    ▼
Repository
    │
    ▼
Entity Framework Core
    │
    ▼
SQL Server
```

Validation is performed automatically through a custom ValidationPipelineBehavior before requests reach their handlers.

---

# Data Access

Data persistence is implemented using:

- SQL Server
- Entity Framework Core
- Code First
- Generic Repository Pattern
- Unit of Work Pattern

Database schema changes are managed using EF Core Migrations.

---

# Concurrency Control

The application prevents race conditions using Entity Framework Core Optimistic Concurrency Control.

Entities that require concurrency protection use EF Core concurrency tokens.

When multiple users attempt to update the same entity simultaneously, EF Core detects the conflict and throws a DbUpdateConcurrencyException.

Concurrency exceptions are handled centrally by the global ErrorHandlerMiddleware.

---

# Authentication & Authorization

Authentication is implemented using JWT.

After successful authentication the application generates a JWT token containing all required user claims.

Authorization is implemented using Role-Based Access Control (RBAC).

The system currently defines two roles:

- Coordinator
- Volunteer

Protected endpoints use ASP.NET Core Authorize attributes.

---

# Notification Architecture

The application supports both real-time and persistent notifications.

SignalR delivers notifications immediately to connected users.

NotificationLog stores every notification in SQL Server, allowing users to review notifications even after reconnecting.

```text
Task Event
      │
      ▼
NotificationLog
      │
      ├────────► SQL Server
      │
      ▼
SignalR Hub
      │
      ▼
Connected Clients
```

---

# File Storage

Binary files are stored using MinIO object storage.

Only the object identifier is stored in SQL Server.

When a file is requested:

1. The object name is retrieved from SQL Server.
2. The file is downloaded from MinIO.

This keeps the relational database free from large binary objects.

---

# Error Handling

The application provides centralized exception handling through ErrorHandlerMiddleware.

Responsibilities include:

- Handling validation errors
- Handling concurrency exceptions
- Returning consistent API responses

---

# Testing Strategy

Unit tests validate the most critical business rules.

Current test coverage includes:

- Task state transitions
- Prevention of invalid state changes
- Optimistic concurrency behavior
- Race condition scenarios

---

# Architectural Decisions

## Clean Architecture

Chosen to separate business logic from infrastructure concerns while improving maintainability and testability.

## CQRS + MediatR

The application adopts the Command Query Responsibility Segregation (CQRS) pattern to separate operations that modify application state from operations that only retrieve data.

Write operations are implemented as **Commands**, while read operations are implemented as **Queries**.

MediatR acts as an in-process message dispatcher that routes each request to its corresponding handler.

This design provides several architectural advantages:

- Controllers remain thin and only dispatch requests.
- Business logic is encapsulated inside dedicated handlers.
- Commands and Queries evolve independently.
- Cross-cutting concerns such as validation can be implemented once using Pipeline Behaviors instead of duplicating logic across handlers.
- New features can be added by introducing new handlers without affecting existing functionality.

By combining CQRS with MediatR, the application achieves better modularity, maintainability, and extensibility.

## Repository + Unit of Work

The infrastructure layer follows the Repository and Unit of Work patterns.

The Repository pattern abstracts data access operations behind a generic interface, allowing the Application layer to work with entities without depending directly on Entity Framework Core.

The Unit of Work coordinates all repository operations within a single business transaction. Instead of saving changes after every repository call, all modifications are accumulated and committed together.

This approach provides several advantages:

- Clear separation between business logic and persistence logic.
- Centralized transaction management.
- Reduced duplication of CRUD operations through generic repositories.
- Easier testing by depending on abstractions rather than concrete implementations.
- Consistent persistence behavior across the application.

## State Pattern

The lifecycle of a volunteer task is implemented using the State Pattern.

Instead of relying on large conditional (`if`/`switch`) statements to determine which operations are allowed in each task state, every state is represented by a dedicated class that encapsulates its own behavior.

Each state is responsible for:

- Defining the operations that are allowed.
- Determining the next valid state.
- Preventing invalid state transitions.

For example:

- A task in the **Registered** state can transition to **In Progress** or **Cancelled**.
- A task in the **Confirmed** state cannot transition to any other state.

This design offers several advantages:

- Eliminates complex conditional logic.
- Improves readability.
- Makes the task lifecycle easier to maintain.
- Allows new states to be added with minimal changes to existing code.
- Enforces business rules directly within the domain model.

## Optimistic Concurrency

Since multiple users may interact with the same volunteer task simultaneously, the system uses Entity Framework Core's optimistic concurrency mechanism to prevent conflicting updates.

Each protected entity contains a concurrency token that is verified during update operations.

If another user modifies the same entity before the current transaction is committed, EF Core detects the conflict and throws a `DbUpdateConcurrencyException`.

Compared to pessimistic locking, optimistic concurrency offers several benefits:

- No long-lived database locks.
- Better scalability for web applications.
- Higher throughput under normal workloads.
- Automatic conflict detection with minimal implementation complexity.

Concurrency exceptions are handled centrally through the global ErrorHandlerMiddleware, ensuring consistent API responses.

## Shared Base Libraries

Rather than placing common infrastructure code inside each project, the solution introduces a reusable set of Base libraries.

These libraries contain generic functionality that is independent of the Volunteer Task Management domain and can be reused in future applications.

Examples include:

- Generic repositories
- Unit of Work
- JWT management
- MinIO integration
- Shared DTOs
- Base entities
- Common middleware
- Exception handling
- Date conversion utilities

This architecture provides several advantages:

- Reduces code duplication.
- Encourages consistency across projects.
- Simplifies maintenance.
- Improves long-term extensibility.
- Allows domain-specific projects to focus only on business requirements.

## SignalR + NotificationLog

The notification subsystem combines real-time communication with persistent storage.

SignalR is responsible for delivering notifications immediately to connected users, providing an interactive user experience.

However, users may be offline when notifications are generated. To avoid losing important events, every notification is also stored in the `NotificationLog` entity.

This hybrid approach provides:

- Instant notifications for connected users.
- Reliable notification history.
- Ability to review missed notifications after reconnecting.
- Better user experience without sacrificing reliability.

## JWT + RBAC

Authentication is implemented using JSON Web Tokens (JWT).

After successful authentication, the application generates a token containing the user's identity and authorization claims.

Authorization is implemented using Role-Based Access Control (RBAC), where permissions are determined by the user's assigned role.

This design offers several advantages:

- Stateless authentication.
- No server-side session management.
- Better scalability for distributed applications.
- Clear separation of responsibilities between Coordinators and Volunteers.
- Simple integration with ASP.NET Core's built-in authorization framework.

---

# Technologies

| Category | Technology |
|----------|------------|
| Framework | ASP.NET Core |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Messaging | MediatR |
| Validation | FluentValidation |
| Authentication | JWT |
| Authorization | RBAC |
| Real-time Communication | SignalR |
| Object Storage | MinIO |
| Testing | xUnit |
