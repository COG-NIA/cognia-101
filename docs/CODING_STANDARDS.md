# Cognia Coding Standards

## Purpose
This document defines the baseline standards for all team members working on the Cognia platform. The goal is to keep the solution consistent, readable, testable, and production-ready.

## Architectural Principles
- Keep the solution layered: API, Client, and Shared.
- Do not put business logic directly in Razor pages or endpoint handlers when it belongs in services or domain models.
- Prefer DTOs and shared contracts over duplicated models across projects.
- Fail fast with explicit validation and friendly HTTP/API error responses.
- Use async patterns for I/O-heavy work and avoid blocking calls in the ASP.NET pipeline.

## Naming Conventions
- Use PascalCase for classes, records, enums, methods, and public properties.
- Use camelCase for private fields, local variables, and parameters.
- Prefix private fields with `_` to improve clarity.
- Use `I` prefixes only for interfaces.
- Use descriptive names that communicate intent; avoid abbreviations except well-known domain terms.

## C# Code Style
- Enable nullable reference types and keep warnings under review.
- Prefer expression-bodied members only when they improve readability.
- Keep methods focused and generally under 25 lines when possible.
- Avoid deep nesting and early-return patterns when they harm readability.
- Use `var` only when the type is obvious and improves readability.

## API Standards
- Expose RESTful endpoint names that match domain actions.
- Validate incoming model payloads before persisting data.
- Use `HttpGet`, `HttpPost`, `HttpPut`, `HttpDelete`, or equivalent attribute-based routing consistently.
- Return proper status codes and descriptive messages.
- Log operational errors using ASP.NET Core logging rather than swallowing exceptions.

## Frontend Standards
- Keep Razor components focused and reusable.
- Keep UI logic separated from business logic where possible.
- Use accessible markup and semantic HTML.
- Ensure forms include labels, validation feedback, and keyboard-friendly interactions.
- Keep page-level data fetching explicit and easy to trace.

## Data and Persistence
- Store relational concerns in EF Core models and DbContext classes.
- Add migrations for schema changes instead of modifying data model structure without a migration.
- Keep sensitive configuration in environment variables or appsettings for the active environment.
- Do not commit secrets or production connection strings to source control.

## Testing Expectations
- Add unit or integration tests for new business logic, API endpoints, and critical flows.
- Prefer regression tests for bug fixes.
- Validate UI behavior for major flows before merging.

## Review and Quality Gate
Before merging any change:
1. Ensure the code compiles.
2. Review warnings and fix avoidable ones.
3. Confirm naming, structure, and formatting match this standard.
4. Verify the change solves the stated task without introducing unrelated drift.

## Team Lead Note
The team lead is responsible for maintaining architectural consistency, tracking blockers, and ensuring the repository remains clean, versioned, and understandable for all contributors.
