using Clients;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<ClientsDbContext>(optionsBuilder =>
    optionsBuilder.UseSqlite("Data Source=../clients.db"));

var app = builder.Build();

app.Run(); 