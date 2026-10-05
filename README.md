# Cognia - Mental Wellness Support System

Cognia is a .NET 10 demo application for personal wellbeing check-ins, self-help reading, a peer forum, and therapist session booking. It consists of a Blazor WebAssembly client and an ASP.NET Core Web API.

## Current Features

- Mood check-ins, notes, and 7/30-day charts. Check-ins are stored in the browser's local storage.
- Searchable and categorized self-help reading in the client.
- Forum threads, replies, and categories through the API.
- Therapist directory, session booking, payment history, and Paystack payment initialization/verification.
- Jitsi meeting links for confirmed sessions.

## Technology

- .NET 10 and ASP.NET Core Web API
- Blazor WebAssembly
- Paystack API integration with a local sandbox fallback
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

## Payments

The API reads Paystack settings from the `Paystack:SecretKey` configuration value. For local development, use .NET user secrets rather than committing credentials:

```bash
dotnet user-secrets init --project Cognia.API/Cognia.API.csproj
dotnet user-secrets set "Paystack:SecretKey" "your-paystack-test-secret-key" --project Cognia.API/Cognia.API.csproj
```

When a Paystack key is not configured, or the Paystack request fails, the API payment service returns a sandbox simulation. Do not use this fallback or test credentials for real transactions.

## API Routes

### Forum

- `GET /api/forum/threads`
- `GET /api/forum/categories`
- `GET /api/forum/threads/{id}`
- `GET /api/forum/threads/{id}/replies`
- `POST /api/forum/threads`
- `POST /api/forum/threads/{id}/replies`

### Therapists

- `GET /api/therapists` (optional `specialization` query parameter)
- `GET /api/therapists/{id}`

### Sessions

- `GET /api/sessions`
- `GET /api/sessions/user/{userId}`
- `GET /api/sessions/{id}`
- `POST /api/sessions`

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

- Forum threads, therapist sessions, and payment records are held in server memory and reset when the API restarts.
- The client has mock therapist/session/payment fallbacks when the API is unavailable.
- Mood entries use browser local storage; they are not sent to the API.
- Authentication, authorization, database persistence, and production deployment configuration are not implemented.

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
