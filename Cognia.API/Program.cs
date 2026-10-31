using System.Text;
using Cognia.API.Configuration;
using Cognia.API.Data;
using Cognia.API.Models;
using Cognia.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var localEnvPath = Path.Combine(builder.Environment.ContentRootPath, ".env");
if (File.Exists(localEnvPath))
{
    DotNetEnv.Env.Load(localEnvPath);
    builder.Configuration.AddEnvironmentVariables();
}

var isDevelopment = builder.Environment.IsDevelopment();

var mongoConnectionString = RuntimeConfig.GetMongoConnectionString(
    builder.Configuration["MongoDb:ConnectionString"],
    isDevelopment);
var mongoDatabaseName = builder.Configuration["MongoDb:DatabaseName"] ?? "Cognia";
var jwtSigningKey = RuntimeConfig.GetJwtSigningKey(
    builder.Configuration["Jwt:SigningKey"],
    isDevelopment);

var mongoClient = new MongoClient(mongoConnectionString);
builder.Services.AddSingleton(mongoClient.GetDatabase(mongoDatabaseName));
builder.Services.AddSingleton<CogniaMongoDatabase>();
builder.Services.AddScoped<IPasswordHasher<UserAccount>, PasswordHasher<UserAccount>>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddCors(options => options.AddPolicy("Client", policy =>
    policy.WithOrigins(builder.Configuration["Cors:ClientOrigin"] ?? "http://localhost:5000")
        .AllowAnyHeader()
        .AllowAnyMethod()));
builder.Services.AddScoped<IPaystackService, PaystackService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "Cognia",
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "CogniaClient",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();
await app.Services.GetRequiredService<CogniaMongoDatabase>().EnsureIndexesAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("Client");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
