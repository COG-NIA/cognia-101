# Cognia Developer Setup Guide
 
**Prepared by:** Akubia Edith Elorm (Documentation and Report Writing - ID: 22188216)  
**Project:** Cognia (Student Mental Health Platform)  
**Date:** October 5, 2026 

---

## 1. Overview

This guide explains how to install, configure, run, test, and build the **Cognia Mental Wellness Support System**.

---

## 2. Technology Stack

| Component               | Technology             |
| ----------------------- | ---------------------- |
| Backend                 | ASP.NET Core Web API   |
| Framework               | .NET 10.0              |
| Frontend                | Blazor WebAssembly     |
| Real-time communication | SignalR                |
| Database                | Microsoft SQL Server   |
| ORM                     | Entity Framework Core  |
| Authentication          | ASP.NET Identity + JWT |
| Payments                | Paystack API           |
| Video                   | Jitsi                  |
| Deployment              | Azure / Docker         |
| Language                | C#                     |

---

## 3. Prerequisites

Install the following before setting up Cognia:

* .NET 10.0 SDK
* SQL Server, either local or Azure
* Visual Studio 2022 or VS Code
* Git

### Optional

* Docker
* Azure account

---

## 4. Project Structure

```text
Cognia/
├── Cognia.sln
├── Cognia.API/
│   ├── Controllers/
│   ├── Models/
│   ├── Data/
│   ├── Services/
│   └── Hubs/
├── Cognia.Client/
│   ├── Pages/
│   ├── Components/
│   └── Services/
└── Cognia.Shared/
    └── Models/
```

### Cognia.API

Contains the backend Web API.

### Cognia.Client

Contains the Blazor WebAssembly frontend.

### Cognia.Shared

Contains shared models and DTOs used by the application.

---

## 5. Clone the Repository

Open a terminal and run:

```bash
git clone <repository-url>
cd Cognia
```

---

## 6. Restore Dependencies

From the project root:

```bash
dotnet restore
```

---

## 7. Configure the Database

Cognia uses **Microsoft SQL Server** with **Entity Framework Core**.

### Option A – Local SQL Server

Install SQL Server Express or Developer Edition.

Create a database named:

```text
CogniaDB
```

Update:

```text
Cognia.API/appsettings.json
```

with the appropriate connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CogniaDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### Option B – Azure SQL

Create an Azure SQL Server and database.

Configure the connection string with the appropriate:

* Azure SQL server
* Database name
* Username
* Password

---

## 8. Run Entity Framework Migrations

Navigate to the API project:

```bash
cd Cognia.API
```

Create the initial migration:

```bash
dotnet ef migrations add InitialCreate
```

Update the database:

```bash
dotnet ef database update
```

---

## 9. Configure Development Settings

Create or update:

```text
Cognia.API/appsettings.Development.json
```

The development configuration should contain the required **JWT, Paystack, and Jitsi** settings.

Example:

```json
{
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-at-least-32-characters-long",
    "Issuer": "Cognia",
    "Audience": "CogniaUsers",
    "ExpiryInMinutes": 60
  },
  "Paystack": {
    "PublicKey": "your-paystack-public-key",
    "SecretKey": "your-paystack-secret-key"
  },
  "Jitsi": {
    "BaseUrl": "https://meet.jit.si"
  }
}
```

> **Security:** Do not commit real JWT secrets, Paystack secret keys, passwords, or other sensitive credentials to GitHub.

---

## 10. Run the Backend

From the API directory:

```bash
cd Cognia.API
dotnet run
```

The API runs at:

```text
https://localhost:5001
```

---

## 11. Run the Frontend

Open a second terminal from the project root.

Navigate to the client:

```bash
cd Cognia.Client
```

Restore dependencies:

```bash
dotnet restore
```

Run the application:

```bash
dotnet run --launch-profile http
```

The frontend is available at:

```text
http://localhost:5000
```

Keep both the backend and frontend terminals running while developing.

---

## 12. Default Administrator Account

After the database has been seeded, the default administrator account is:

| Field    | Value              |
| -------- | ------------------ |
| Username | `admin@cognia.com` |
| Password | `Admin@123`        |

### Security Warning

The default password must be changed immediately after the first login.

> **Important:** If these credentials are only intended for local development, they should be clearly marked as development-only credentials and should never be reused in production.

---

## 13. Running Tests

Run the project's tests with:

```bash
dotnet test
```

---

## 14. Code Formatting

To format the project:

```bash
dotnet format
```

---

## 15. Production Build

Create a Release build using:

```bash
dotnet build -c Release
```

---

## 16. Azure Deployment

Cognia can be deployed using Azure services.

The documented deployment architecture includes:

* **Azure App Service** – Backend
* **Azure Static Web App** – Frontend
* **Azure SQL Server** – Database

Configure the required environment variables and application settings in Azure.

The project can be deployed from GitHub using the configured deployment workflow.

---

## 17. Docker Deployment

The backend Docker image can be built using:

```bash
docker build -t cognia-api -f Cognia.API/Dockerfile .
```

The frontend image can be built using:

```bash
docker build -t cognia-client -f Cognia.Client/Dockerfile .
```

Run the containers using:

```bash
docker-compose up
```

---

## 18. Troubleshooting

### Database Connection Failure

Check that:

* SQL Server is running.
* The database exists.
* The connection string is correct.
* Entity Framework migrations have been applied.

### API Does Not Start

Check that:

* .NET 10.0 SDK is installed.
* Required configuration values exist.
* The configured API port is available.

### Frontend Cannot Communicate with API

Check that:

* The backend is running.
* The frontend is configured to use the correct API address.
* The API is accessible from the browser.

### Payment Integration Fails

Check that:

* Paystack credentials are correctly configured.
* The Paystack service is accessible.
* The correct development/production credentials are being used.
