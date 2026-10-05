using Cognia.API.Hubs;
using Cognia.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddSignalR();
builder.Services.AddScoped<IPaystackService, PaystackService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ForumCors", policy =>
    {
        policy
            .WithOrigins("http://localhost:5000", "https://localhost:7104")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("ForumCors");
app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();
