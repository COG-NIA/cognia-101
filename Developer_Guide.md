# Cognia Developer Guide

**Prepared by:** Akubia Edith Elorm (Documentation and Report Writing - ID: 22188216)  
**Project:** Cognia (Student Mental Health Platform)  
**Date:** October 5, 2026 

---

## 1. System Overview

Cognia is a web-based **Mental Wellness Support System** providing:

* Mood tracking
* Self-help resources
* Anonymous peer-to-peer discussions
* Therapist sessions
* Online video sessions
* Payment processing
* Anonymous support

The system consists of a **Blazor WebAssembly frontend**, **ASP.NET Core Web API backend**, **SQL Server database**, and several external integrations.

---

# 2. Architecture

Cognia follows a separated frontend/backend architecture.

```text
                    ┌─────────────────────┐
                    │  Blazor WebAssembly │
                    │    Cognia.Client   │
                    └──────────┬──────────┘
                               │
                         HTTP / API
                               │
                    ┌──────────▼──────────┐
                    │ ASP.NET Core Web API│
                    │     Cognia.API     │
                    └──────┬──────┬───────┘
                           │      │
                ┌──────────▼─┐  ┌─▼─────────┐
                │ SQL Server │  │  SignalR  │
                │  EF Core   │  │   Hubs    │
                └────────────┘  └───────────┘
                           │
             ┌─────────────┼─────────────┐
             │             │             │
          Paystack       Jitsi       ASP.NET
          Payments       Video       Identity
```

---

# 3. Backend Structure

The backend is located in:

```text
Cognia.API/
```

It contains:

```text
Controllers/
Models/
Data/
Services/
Hubs/
```

## Controllers

Controllers expose HTTP API endpoints to the frontend.

Examples include endpoints for:

* Authentication
* Mood tracking
* Articles
* Forum functionality
* Therapists
* Sessions
* Payments

## Models

Contains backend data models used by the application.

## Data

Contains the Entity Framework Core database context and migrations.

## Services

Contains business logic and supporting application services.

## Hubs

Contains SignalR hubs responsible for real-time communication.

---

# 4. Frontend Structure

The frontend is located in:

```text
Cognia.Client/
```

It contains:

```text
Pages/
Components/
Services/
```

### Pages

Contains the main Blazor pages.

### Components

Contains reusable user-interface components.

### Services

Contains client-side services used to communicate with the backend and support application functionality.

---

# 5. Shared Project

The:

```text
Cognia.Shared/
```

project contains shared models and DTOs.

This allows the frontend and backend to use consistent data structures.

---

# 6. Authentication

Cognia uses:

* ASP.NET Identity
* JWT authentication

Authentication is handled through the authentication API endpoints:

```http
POST /api/auth/register
POST /api/auth/login
POST /api/auth/logout
```

Protected API resources should validate the user's authentication token before processing requests.

---

# 7. Database

Cognia uses:

* Microsoft SQL Server
* Entity Framework Core

Database configuration is located within the API project's configuration.

Database changes should be managed using Entity Framework Core migrations.

Typical development commands include:

```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

---

# 8. SignalR

SignalR provides Cognia's real-time communication functionality.

The backend stores SignalR hubs under:

```text
Cognia.API/Hubs/
```

The Safe Space forum uses SignalR to support real-time replies.

Developers modifying forum functionality should ensure that real-time communication remains synchronized with the underlying API and database operations.

---

# 9. Paystack Integration

Paystack provides payment processing for therapist sessions.

The documented payment endpoint is:

```http
POST /api/payments
```

The application's configuration contains:

```text
Paystack:PublicKey
Paystack:SecretKey
```

> **Security:** Secret credentials must be protected and should not be committed to source control.

---

# 10. Jitsi Integration

Cognia uses **Jitsi** for online therapy video sessions.

The Jitsi base URL is configured through:

```text
Jitsi:BaseUrl
```

The development configuration uses:

```text
https://meet.jit.si
```

---

# 11. Core Modules

## Mood Tracker

Responsible for:

* Mood logging
* Trigger notes
* Mood history
* Weekly trends
* Monthly trends
* Self-awareness dashboard

## Self-Help Hub

Responsible for:

* Articles
* Categories
* Breathing exercises
* Coping strategies

## Safe Space

Responsible for:

* Forum threads
* Replies
* Real-time communication
* Emoji reactions
* Trigger warnings
* Content moderation

## Therapist Sessions

Responsible for:

* Therapist profiles
* Session booking
* Payments
* Jitsi video sessions

## Anonymous Support

Responsible for:

* Anonymous help requests
* Counsellor routing
* Daily check-in notifications
* Wellbeing tracking

---

# 12. Development Workflow

Before making changes:

1. Pull the latest project changes.
2. Create a development branch.
3. Make the required changes.
4. Test the changes.
5. Format the code.
6. Commit the changes.
7. Push the branch.
8. Create a pull request where applicable.

Run:

```bash
dotnet test
```

to execute tests.

Run:

```bash
dotnet format
```

to maintain consistent formatting.

---

# 13. Production Build

Use:

```bash
dotnet build -c Release
```

to create a Release build.

Production deployment is supported through:

* Azure
* Docker

---

# 14. Security Guidelines

Developers must:

* Never commit passwords to GitHub.
* Never commit Paystack secret keys.
* Never expose JWT signing secrets.
* Validate user input.
* Protect authenticated endpoints.
* Apply appropriate authorization.
* Protect user and mental-wellness information.
* Follow secure coding practices.

---

# 15. Important Development Rule

When adding a new feature, developers should update the relevant:

* API endpoint documentation
* User documentation
* Developer documentation
* Tests
* Database migrations, if required
* README

This keeps the project's technical documentation synchronized with the implementation.
