using Base.Api.Registration;
using VolunteerTaskManagement.Api.Hubs;
using VolunteerTaskManagement.Gateway.Registration;
using VolunteerTaskManagement.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices(builder.Configuration);
builder.Services.AddSingleton<NotificationHub>();
builder.Services.AddSignalR();

var app = builder.Build();

VolunteerTaskManagementSeed.SeedDatabase(app);

app.UseCustomMiddlewares(builder);
app.MapHub<NotificationHub>("/NotifRoot");

app.Run();
