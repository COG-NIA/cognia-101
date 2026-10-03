# Cognia - Mental Wellness Support System

A comprehensive web application for mental wellness tracking, professional therapy access, and anonymous peer-to-peer community support.

## Tech Stack

- **Backend**: ASP.NET Core Web API (.NET 10.0)
- **Frontend**: Blazor WebAssembly
- **Real-time**: SignalR
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: ASP.NET Identity with JWT
- **Payment**: Paystack API
- **Video**: Jitsi integration
- **Deployment**: Azure / Docker

## Project Structure

```
Cognia/
├── Cognia.sln                 # Solution file
├── Cognia.API/                # Backend Web API
│   ├── Controllers/           # API controllers
│   ├── Models/                # Data models
│   ├── Data/                  # DbContext and migrations
│   ├── Services/              # Business logic services
│   └── Hubs/                  # SignalR hubs
├── Cognia.Client/             # Blazor WebAssembly frontend
│   ├── Pages/                 # Blazor pages
│   ├── Components/            # Reusable components
│   └── Services/              # Client-side services
└── Cognia.Shared/             # Shared models and DTOs
    └── Models/                # Shared data models
```

## Prerequisites

- .NET 10.0 SDK
- SQL Server (local or Azure)
- Visual Studio 2022 or VS Code
- Git
- (Optional) Docker
- (Optional) Azure account

## Setup Instructions

### 1. Clone the Repository

```bash
git clone <repository-url>
cd Cognia
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure Database

#### Option A: Local SQL Server

1. Install SQL Server Express or Developer Edition
2. Create a new database named `CogniaDB`
3. Update connection string in `Cognia.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CogniaDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

#### Option B: Azure SQL Server

1. Create an Azure SQL Server and database
2. Update connection string in `Cognia.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:<server-name>.database.windows.net,1433;Database=CogniaDB;User ID=<username>;Password=<password>;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}
```

### 4. Run Database Migrations

```bash
cd Cognia.API
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Configure Environment Variables

Create `Cognia.API/appsettings.Development.json`:

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

### 6. Run the Backend API

```bash
cd Cognia.API
dotnet run
```

The API will run on `https://localhost:5001`

### 7. Run the Frontend (Blazor)

Open a second terminal at the repository root. Restore and run the Blazor client:

```bash
cd Cognia.Client
dotnet restore
dotnet run --launch-profile http
```

Open `http://localhost:5000` in your browser. Keep this terminal running while using the frontend.

## Default Admin Account

After running the application and seeding the database:

- **Username**: `admin@cognia.com`
- **Password**: `Admin@123`

**Important**: Change the default admin password immediately after first login.

## Features

### Mood Tracker
- Log daily mood entries (happy, anxious, stressed, neutral)
- Add optional trigger notes
- View weekly/monthly mood trend charts
- Self-awareness dashboard

### Self-Help Hub
- Curated articles on mental wellness
- Breathing exercises and coping strategies
- Categorized content (stress, sleep, grief, academic pressure)
- Accessible without login

### Safe Space Forum
- Anonymous peer-to-peer community
- Topic threads with real-time replies
- Emoji reactions
- Trigger warnings
- Admin content moderation

### Therapist Sessions
- Browse verified therapist profiles
- Book paid 1-on-1 sessions
- Pay via Paystack (MTN MoMo / card)
- Integrated video sessions via Jitsi

### Anonymous Support
- Submit anonymous help requests
- Requests routed to counsellors
- Daily check-in notifications
- Consistent wellbeing tracking

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - User login
- `POST /api/auth/logout` - User logout

### Mood Tracker
- `GET /api/mood` - Get user's mood entries
- `POST /api/mood` - Log new mood entry
- `GET /api/mood/trends` - Get mood trends

### Self-Help Hub
- `GET /api/articles` - Get all articles
- `GET /api/articles/{id}` - Get specific article
- `GET /api/articles/categories` - Get article categories

### Forum
- `GET /api/forum/threads` - Get all threads
- `POST /api/forum/threads` - Create new thread
- `GET /api/forum/threads/{id}/replies` - Get thread replies
- `POST /api/forum/threads/{id}/replies` - Add reply

### Therapist Sessions
- `GET /api/therapists` - Get all therapists
- `GET /api/therapists/{id}` - Get therapist profile
- `POST /api/sessions` - Book a session
- `POST /api/payments` - Process payment

## Development

### Running Tests

```bash
dotnet test
```

### Code Formatting

```bash
dotnet format
```

### Building for Production

```bash
dotnet build -c Release
```

## Deployment

### Azure Deployment

1. Create Azure resources:
   - Azure App Service (Backend)
   - Azure Static Web App (Frontend)
   - Azure SQL Server

2. Configure environment variables in Azure

3. Deploy from GitHub:
   ```bash
   git push origin main
   ```

### Docker Deployment

```bash
# Build backend image
docker build -t cognia-api -f Cognia.API/Dockerfile .

# Build frontend image
docker build -t cognia-client -f Cognia.Client/Dockerfile .

# Run containers
docker-compose up
```

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
