using ChatServer.Configurations;
using ChatServer.DbContext;
using ChatServer.Hubs;
using ChatServer.Repositories.Implementations;
using ChatServer.Repositories.Interfaces;
using ChatServer.Services.Implementations;
using ChatServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<AuthConfiguration>(
    builder.Configuration.GetSection("Auth"));
builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repos section

builder.Services.AddTransient<IUserRepository, UserRepository>();

// Services section

builder.Services.AddTransient<IUserService, UserService>();



builder.Services.AddControllers();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<ChatHub>("/chat");

app.Run();
