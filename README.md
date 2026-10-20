# Cognia - Mental Wellness Support System

Cognia is a .NET 10 application for personal wellbeing check-ins, self-help reading, a peer forum, and therapist session booking. It consists of a Blazor WebAssembly client and an ASP.NET Core Web API backed by MongoDB.

## Current Features

- Account registration and sign-in with JWT authentication.
- Private mood check-ins stored in MongoDB and displayed in 7/30-day charts.
- Self-help articles stored in MongoDB and managed by administrators.
- Forum threads and replies stored in MongoDB; posting requires an account.
- Therapist directory, session booking, and payment history stored in MongoDB.
- Paystack checkout and verification using the configured Paystack API key.
- Jitsi meeting links for confirmed sessions.

## Technology

- .NET 10 and ASP.NET Core Web API
- Blazor WebAssembly
- MongoDB.Driver
- JWT bearer authentication
- Paystack API integration
- Jitsi meeting links

## Project Structure

```
Cognia.slnx
Cognia.API/       ASP.NET Core API, controllers, and Paystack service
Cognia.Client/    Blazor WebAssembly pages and client services
Cognia.Shared/    Models shared by the API and client
Cognia.Tests/     xUnit tests
```

## Prerequisites

- .NET 10 SDK
- A browser
- Git, if cloning the repository

## Run Locally

From the repository root, restore the solution:

```bash
dotnet restore Cognia.slnx
```

Configure local secrets. Do not put connection strings, signing keys, bootstrap tokens, or Paystack keys in tracked `appsettings*.json` files:

```powershell
dotnet user-secrets init --project Cognia.API/Cognia.API.csproj
dotnet user-secrets set "MongoDb:ConnectionString" "<your-rotated-mongodb-atlas-connection-string>" --project Cognia.API/Cognia.API.csproj
dotnet user-secrets set "MongoDb:DatabaseName" "Cognia" --project Cognia.API/Cognia.API.csproj
dotnet user-secrets set "Jwt:SigningKey" "<random-secret-at-least-32-bytes>" --project Cognia.API/Cognia.API.csproj
dotnet user-secrets set "Admin:BootstrapToken" "<one-time-random-bootstrap-token>" --project Cognia.API/Cognia.API.csproj
dotnet user-secrets set "Paystack:SecretKey" "<rotated-paystack-test-secret-key>" --project Cognia.API/Cognia.API.csproj
```

In MongoDB Atlas, allow the API host's outbound IP in Network Access and use a database user with access only to the Cognia database. The connection string's password must be URL-encoded if it contains reserved characters.

Start the API in one terminal using its HTTPS launch profile:

```bash
dotnet run --project Cognia.API/Cognia.API.csproj --launch-profile https
```

The API listens at `https://localhost:7214`, which is the address configured in the client. Trust the local ASP.NET Core development certificate if prompted or if the browser reports a certificate error.

Start the client in a second terminal:

```bash
dotnet run --project Cognia.Client/Cognia.Client.csproj --launch-profile http
```

Open `http://localhost:5000`.

## First Administrator

The database starts empty. Create the first administrator once by calling `POST /api/auth/bootstrap-admin` with the `X-Admin-Bootstrap-Token` header set to `Admin:BootstrapToken` and a JSON body containing `email`, `displayName`, and a password of at least 12 characters. The endpoint is disabled after an admin account exists. Sign in at `/login`; use the resulting admin account to create actual therapist and article records through the admin-only `POST /api/therapists` and `POST /api/articles` endpoints.

For Azure, add these values under the API App Service's **Configuration** settings instead of user-secrets: `MongoDb__ConnectionString`, `MongoDb__DatabaseName`, `Jwt__SigningKey`, `Jwt__Issuer`, `Jwt__Audience`, `Admin__BootstrapToken`, `Paystack__SecretKey`, and `Cors__ClientOrigin`. Set the frontend's `ApiBaseUrl` in `Cognia.Client/wwwroot/appsettings.json` to the deployed API origin before publishing the client.

## API Routes

### Accounts

- `POST /api/auth/register`
- `POST /api/auth/login`
- `POST /api/auth/bootstrap-admin` (one-time bootstrap token required)

### Mood

- `GET /api/mood` (authenticated; returns only the caller's entries)
- `POST /api/mood` (authenticated)

### Articles

- `GET /api/articles`
- `GET /api/articles/{id}`
- `POST /api/articles` (Admin role required)

### Forum

- `GET /api/forum/threads`
- `GET /api/forum/categories`
- `GET /api/forum/threads/{id}`
- `GET /api/forum/threads/{id}/replies`
- `POST /api/forum/threads` (authenticated)
- `POST /api/forum/threads/{id}/replies` (authenticated)

### Therapists

- `GET /api/therapists` (optional `specialization` query parameter)
- `GET /api/therapists/{id}`

### Sessions

- `GET /api/sessions`
- `GET /api/sessions/{id}`
- `POST /api/sessions` (authenticated)

### Payments

- `GET /api/payments`
- `POST /api/payments/initialize`
- `GET /api/payments/verify/{reference}`

OpenAPI is available from the API in Development at `/openapi/v1.json`.

## Development

Run the tests and build the solution from the repository root:

```bash
dotnet test Cognia.slnx
dotnet build Cognia.slnx -c Release
```

## Demo Limitations

- MongoDB starts with no sample users, therapists, articles, forum threads, or payment records. Add real content through the administrator APIs.
- Paystack must be configured for checkout; missing configuration and provider failures return errors instead of fake successes.
- Use HTTPS and rotate any credentials that have been shared or committed. Configure allowed frontend origin, MongoDB network access, and production secrets before public deployment.

## Team

- **Team Lead**: Papah Kweku Okae Quansah (22031632)
- **UI/UX Designer**: Noye Magdalene Norkai (22032918)
- **Backend Developer**: Beatrice Bansah (22100696)
- **Database Admin**: Asare Boateng Abel (22241111)
- **Frontend Developer**: Adade Priscilla Adwoa (22049777)
- **Forum Developer**: Marie-Anne Dzifa Hayibor (22242639)
- **Payment Integration**: Lois Osei-Bonsu (22044680)
- **Real-time Features**: Josephine Tetteh (22124731)
- **QA Lead**: Tetteh Joel Oglie Nathan (22182128)
- **Documentation**: Akubia Edith Elorm (22188216)
- **DevOps**: Edmond Dare (22018183)
- **Security**: Vical Divine Aghe (22250179)

## License

This project is for academic purposes for the University of Ghana, Department of Computer Science, DCIT 308 – .NET Ecosystem.

## Support

For issues and questions, please create an issue in the GitHub repository or contact the team lead.
